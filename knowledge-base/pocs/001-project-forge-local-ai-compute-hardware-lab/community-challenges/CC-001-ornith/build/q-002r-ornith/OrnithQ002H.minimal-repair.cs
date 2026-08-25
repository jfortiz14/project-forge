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

    public sealed class IntakeRequest
    {
        private readonly Uri _uri;
        private readonly byte[] _fingerprint;

        private IntakeRequest(Uri uri, IReadOnlyDictionary<string, string> metadata, byte[] fingerprint)
        {
            _uri = uri;
            Metadata = metadata;
            _fingerprint = fingerprint;
        }

        public Uri Uri => _uri;

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public byte[] Fingerprint => _fingerprint;

        public static IntakeRequest Create(Uri uri, IReadOnlyCollection<KeyValuePair<string, string>> metadata)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (string.IsNullOrWhiteSpace(uri.AbsoluteUri))
            {
                throw new ArgumentException("A URI must be supplied.", nameof(uri));
            }

            if (metadata is null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("The blob reference must be an absolute URI.", nameof(uri));
            }

            var scheme = uri.Scheme;
            if (!string.Equals(scheme, "http", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(scheme, "https", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("The blob reference must be an absolute HTTP or HTTPS URI.", nameof(uri));
            }

            var normalized = NormalizeUri(uri);
            var fingerprint = ComputeFingerprint(normalized, metadata);

            var metadataCopy = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var entry in metadata)
            {
                if (entry.Key is null)
                {
                    throw new ArgumentException("Metadata keys must not be null.", nameof(metadata));
                }

                if (entry.Value is null)
                {
                    throw new ArgumentException("Metadata values must not be null.", nameof(metadata));
                }

                metadataCopy[entry.Key] = entry.Value;
            }

            return new IntakeRequest(normalized, metadataCopy, fingerprint);
        }

        private static Uri NormalizeUri(Uri uri)
        {
            var builder = new UriBuilder(uri);
            builder.Port = -1;
            builder.Path = builder.Path.TrimEnd('/');

            if (builder.Path.Length == 0)
            {
                builder.Path = "/";
            }

            return builder.Uri;
        }

        private static byte[] ComputeFingerprint(Uri normalizedUri, IReadOnlyCollection<KeyValuePair<string, string>> metadata)
        {
            var builder = new StringBuilder();
            builder.Append(normalizedUri.Scheme).Append("://").Append(normalizedUri.Host).Append(normalizedUri.LocalPath);

            var sortedEntries = metadata
                .OrderBy(e => e.Key, StringComparer.Ordinal)
                .ThenBy(e => e.Value, StringComparer.Ordinal)
                .ToArray();

            foreach (var entry in sortedEntries)
            {
                builder.Append('\n');
                builder.Append(entry.Key).Append('=').Append(entry.Value);
            }

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());

            using var sha = SHA256.Create();
            return sha.ComputeHash(bytes);
        }
    }

    public sealed class IntakeRecord
    {
        private readonly IntakeState _state;
        private readonly string _idempotencyKey;
        private readonly string _correlationId;
        private readonly IntakeRequest _request;
        private readonly string _workerAttemptId;
        private readonly DateTime? _leaseExpiresAtUtc;
        private readonly long _concurrencyToken;

        private IntakeRecord(IntakeState state, string idempotencyKey, string correlationId, IntakeRequest request,
            string workerAttemptId, DateTime? leaseExpiresAtUtc, long concurrencyToken)
        {
            _state = state;
            _idempotencyKey = idempotencyKey;
            _correlationId = correlationId;
            _request = request;
            _workerAttemptId = workerAttemptId;
            _leaseExpiresAtUtc = leaseExpiresAtUtc;
            _concurrencyToken = concurrencyToken;
        }

        public IntakeState State => _state;

        public string IdempotencyKey => _idempotencyKey;

        public string CorrelationId => _correlationId;

        public IntakeRequest Request => _request;

        public string WorkerAttemptId => _workerAttemptId;

        public DateTime? LeaseExpiresAtUtc => _leaseExpiresAtUtc;

        public long ConcurrencyToken => _concurrencyToken;

        public static IntakeRecord CreateQueued(string idempotencyKey, string correlationId, IntakeRequest request, long concurrencyToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                throw new ArgumentException("The record identifier must not be blank.", nameof(idempotencyKey));
            }

            if (correlationId is null)
            {
                throw new ArgumentNullException(nameof(correlationId));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new IntakeRecord(IntakeState.Queued, idempotencyKey, correlationId, request, null, null, concurrencyToken);
        }

        public TransitionResult TryClaim(string workerAttemptId, DateTime nowUtc, TimeSpan leaseDuration, long expectedConcurrencyToken)
        {
            if (workerAttemptId is null)
            {
                throw new ArgumentNullException(nameof(workerAttemptId));
            }

            if (leaseDuration <= TimeSpan.Zero)
            {
                throw new ArgumentException("The lease duration must be positive.", nameof(leaseDuration));
            }

            if (nowUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("nowUtc must be expressed in UTC.", nameof(nowUtc));
            }

            var expected = _request.Fingerprint;
            var actual = _concurrencyToken;

            var permitted = _state == IntakeState.Queued ||
                            (_state == IntakeState.Processing && _leaseExpiresAtUtc.HasValue && _leaseExpiresAtUtc.Value < nowUtc);

            if (!permitted || actual != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable();
            }

            var processing = new IntakeRecord(
                IntakeState.Processing,
                _idempotencyKey,
                _correlationId,
                _request,
                workerAttemptId,
                nowUtc.Add(leaseDuration),
                expectedConcurrencyToken);

            return TransitionResult.Claimed(processing);
        }

        public TransitionResult TryComplete(string workerAttemptId, long expectedConcurrencyToken, long newConcurrencyToken)
        {
            if (workerAttemptId is null)
            {
                throw new ArgumentNullException(nameof(workerAttemptId));
            }

            if (_state != IntakeState.Processing)
            {
                return TransitionResult.NotClaimable();
            }

            if (!string.Equals(_workerAttemptId, workerAttemptId, StringComparison.Ordinal) ||
                _concurrencyToken != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable();
            }

            var completed = new IntakeRecord(
                IntakeState.Completed,
                _idempotencyKey,
                _correlationId,
                _request,
                null,
                null,
                newConcurrencyToken);

            return TransitionResult.Completed(completed);
        }

        public TransitionResult TryFail(string workerAttemptId, long expectedConcurrencyToken, long newConcurrencyToken)
        {
            if (workerAttemptId is null)
            {
                throw new ArgumentNullException(nameof(workerAttemptId));
            }

            if (_state != IntakeState.Processing)
            {
                return TransitionResult.NotClaimable();
            }

            if (!string.Equals(_workerAttemptId, workerAttemptId, StringComparison.Ordinal) ||
                _concurrencyToken != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable();
            }

            var failed = new IntakeRecord(
                IntakeState.Failed,
                _idempotencyKey,
                _correlationId,
                _request,
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

            return System.Linq.Enumerable.SequenceEqual(_request.Fingerprint, request.Fingerprint);
        }
    }

    public sealed class TransitionResult
    {
        private readonly IntakeDecision _decision;
        private readonly IntakeRecord? _record;

        private TransitionResult(IntakeDecision decision, IntakeRecord? record)
        {
            _decision = decision;
            _record = record;
        }

        public IntakeDecision Decision => _decision;

        public IntakeRecord? Record => _record;

        public bool HasRecord => _record is not null;

        public static TransitionResult Accepted(IntakeRecord record)
        {
            return new TransitionResult(IntakeDecision.Accepted, record);
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

        public static TransitionResult Conflict()
        {
            return new TransitionResult(IntakeDecision.Conflict, null);
        }

        public static TransitionResult NotClaimable()
        {
            return new TransitionResult(IntakeDecision.NotClaimable, null);
        }

        public static TransitionResult NotClaimable(IntakeRecord record)
        {
            return new TransitionResult(IntakeDecision.NotClaimable, record);
        }

        public static TransitionResult NotClaimable(IntakeRecord record, IntakeDecision decision)
        {
            return new TransitionResult(decision, record);
        }
    }
}
