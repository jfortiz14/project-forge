using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Forge.DocumentIntake
{
    public enum IntakeState
    {
        Queued,
        Processing,
        Completed,
        Failed,
        DeadLettered
    }

    public enum IntakeDecision
    {
        Accepted,
        Conflict,
        NotClaimable,
        Claimed,
        Completed,
        Failed
    }

    public sealed class Fingerprint
    {
        private readonly string _value;

        public Fingerprint(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value)
ArgumentNullException(nameof(value));
        }

        public string Value => _value;

        public override bool Equals(object? obj)
        {
            return obj is Fingerprint other && _value == other._value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value;
        }
    }

    public sealed class UriReference
    {
        private readonly Uri _uri;

        private UriReference(Uri uri)
        {
            _uri = uri;
        }

        public Uri Uri => _uri;

        public static UriReference FromAbsoluteUri(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (uri.IsAbsoluteUri == false
                && (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.Ur
Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Blob reference must be an abso
absolute HTTP or HTTPS URI.", nameof(uri));
            }

            return new UriReference(uri);
        }

        public Fingerprint CreateFingerprint(IEnumerable<KeyValuePair<strin
CreateFingerprint(IEnumerable<KeyValuePair<string, string>>? metadata)
        {
            var builder = new StringBuilder();
            builder.Append(_uri.Scheme).Append('|').Append(_uri.Host).Appen
builder.Append(_uri.Scheme).Append('|').Append(_uri.Host).Append('|').Appenbuilder.Append(_uri.Scheme).Append('|').Append(_uri.Host).Appen('|').Append(_uri.Port);

            var path = _uri.AbsolutePath;
            if (string.IsNullOrEmpty(path) && _uri.Scheme == Uri.UriSchemeH
Uri.UriSchemeHttp)
            {
                path = "/";
            }

            builder.Append('|').Append(path);

            builder.Append('|').Append(_uri.Query);
            if (string.IsNullOrEmpty(_uri.Query) == false)
            {
                builder.Append('#');
            }

            builder.Append(_uri.Fragment);

            var pairs = (metadata ?? Enumerable.Empty<KeyValuePair<string, 
string>>())
                .Select(p => new KeyValuePair<string, string>(
                    p.Key ?? throw new ArgumentNullException(nameof(p.Key))
ArgumentNullException(nameof(p.Key)),
                    p.Value ?? throw new ArgumentNullException(nameof(p.Val
ArgumentNullException(nameof(p.Value))))
                .OrderBy(p => p.Key, StringComparer.Ordinal)
                .ThenBy(p => p.Value, StringComparer.Ordinal);

            foreach (var pair in pairs)
            {
                builder.Append('|').Append(pair.Key).Append('=').Append(pai
builder.Append('|').Append(pair.Key).Append('=').Append(pair.Value);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());
            byte[] hash = SHA256.HashData(bytes);

            return new Fingerprint(Convert.ToHexString(hash));
        }
    }

    public sealed class IntakeRequest
    {
        public UriReference BlobReference { get; }
        public Fingerprint Fingerprint { get; }

        private IntakeRequest(UriReference blobReference, Fingerprint finge
fingerprint)
        {
            BlobReference = blobReference;
            Fingerprint = fingerprint;
        }

        public static IntakeRequest Create(
            string idempotencyKey,
            Uri blobReference,
            IEnumerable<KeyValuePair<string, string>>? metadata)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                throw new ArgumentException("Idempotency key must not be bl
blank.", nameof(idempotencyKey));
            }

            if (metadata is null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            var normalized = UriReference.FromAbsoluteUri(blobReference);
            var fingerprint = normalized.CreateFingerprint(metadata);

            return new IntakeRequest(normalized, fingerprint);
        }
    }

    public sealed class IntakeRecord
    {
        public string Id { get; }
        public string CorrelationId { get; }
        public IntakeState State { get; }
        public Fingerprint Fingerprint { get; }
        public string? WorkerAttemptId { get; }
        public DateTimeOffset? LeaseExpiryUtc { get; }
        public long ConcurrencyToken { get; }

        private IntakeRecord(
            string id,
            string correlationId,
            IntakeState state,
            Fingerprint fingerprint,
            string? workerAttemptId,
            DateTimeOffset? leaseExpiryUtc,
            long concurrencyToken)
        {
            Id = id;
            CorrelationId = correlationId;
            State = state;
            Fingerprint = fingerprint;
            WorkerAttemptId = workerAttemptId;
            LeaseExpiryUtc = leaseExpiryUtc;
            ConcurrencyToken = concurrencyToken;
        }

        public static IntakeRecord CreateQueued(
            IntakeRequest request,
            string id,
            string correlationId,
            long concurrencyToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Identifier must not be blank."
blank.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentException("Correlation ID must not be bla
blank.", nameof(correlationId));
            }

            return new IntakeRecord(
                id,
                correlationId,
                IntakeState.Queued,
                request.Fingerprint,
                null,
                null,
                concurrencyToken);
        }

        public TransitionResult TryClaim(
            string workerAttemptId,
            DateTimeOffset nowUtc,
            TimeSpan leaseDuration,
            long expectedConcurrencyToken)
        {
            if (workerAttemptId is null)
            {
                throw new ArgumentNullException(nameof(workerAttemptId));
            }

            if (leaseDuration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Lease duration must be positiv
positive.", nameof(leaseDuration));
            }

            if (nowUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("nowUtc must be UTC.", nameof(n
nameof(nowUtc));
            }

            if (State != IntakeState.Queued)
            {
                if (State == IntakeState.Processing && LeaseExpiryUtc.HasVa
LeaseExpiryUtc.HasValue && LeaseExpiryUtc.Value < nowUtc)
                {
                    // lease expired, permitted below
                }
                else
                {
                    return TransitionResult.NotClaimable(this);
                }
            }

            if (ConcurrencyToken != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable(this);
            }

            var processing = new IntakeRecord(
                Id,
                CorrelationId,
                IntakeState.Processing,
                Fingerprint,
                workerAttemptId,
                nowUtc.Add(leaseDuration),
                expectedConcurrencyToken);

            return TransitionResult.Claimed(processing);
        }

        public TransitionResult TryComplete(
            string workerAttemptId,
            long expectedConcurrencyToken,
            long newConcurrencyToken)
        {
            if (workerAttemptId is null)
            {
                throw new ArgumentNullException(nameof(workerAttemptId));
            }

            if (State != IntakeState.Processing
                || string.Equals(WorkerAttemptId, workerAttemptId, StringCo
StringComparison.Ordinal) == false
                || ConcurrencyToken != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable(this);
            }

            var completed = new IntakeRecord(
                Id,
                CorrelationId,
                IntakeState.Completed,
                Fingerprint,
                null,
                null,
                newConcurrencyToken);

            return TransitionResult.Completed(completed);
        }

        public TransitionResult TryFail(
            string workerAttemptId,
            long expectedConcurrencyToken,
            long newConcurrencyToken)
        {
            if (workerAttemptId is null)
            {
                throw new ArgumentNullException(nameof(workerAttemptId));
            }

            if (State != IntakeState.Processing
                || string.Equals(WorkerAttemptId, workerAttemptId, StringCo
StringComparison.Ordinal) == false
                || ConcurrencyToken != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable(this);
            }

            var failed = new IntakeRecord(
                Id,
                CorrelationId,
                IntakeState.Failed,
                Fingerprint,
                null,
                null,
                newConcurrencyToken);

            return TransitionResult.Failed(failed);
        }

        public bool Matches(IntakeRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return Fingerprint.Equals(request.Fingerprint);
        }
    }

    public sealed class TransitionResult
    {
        public IntakeDecision Decision { get; }
        public IntakeRecord? Record { get; }

        private TransitionResult(IntakeDecision decision, IntakeRecord? rec
record)
        {
            Decision = decision;
            Record = record;
        }

        public static TransitionResult Claimed(IntakeRecord record)
        {
            return new TransitionResult(IntakeDecision.Claimed, record);
        }

        public static TransitionResult Completed(IntakeRecord record)
        {
            return new TransitionResult(IntakeDecision.Completed, record);
        }

        public static TransitionResult Failed(IntakeRecord record)
        {
            return new TransitionResult(IntakeDecision.Failed, record);
        }

        public static TransitionResult NotClaimable(IntakeRecord record)
        {
            return new TransitionResult(IntakeDecision.NotClaimable, record
record);
        }
    }
}