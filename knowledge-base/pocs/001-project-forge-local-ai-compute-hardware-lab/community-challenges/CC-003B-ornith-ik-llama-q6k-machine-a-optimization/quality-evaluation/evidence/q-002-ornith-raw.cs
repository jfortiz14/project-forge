```csharp
#nullable enable

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
        private readonly IReadOnlyDictionary<string, string> _metadata;

        private IntakeRequest(
            string idempotencyKey,
            IReadOnlyDictionary<string, string> metadata,
            Uri blob,
            string fingerprint)
        {
            IdempotencyKey = idempotencyKey;
            _metadata = metadata;
            Blob = blob;
            Fingerprint = fingerprint;
        }

        public string IdempotencyKey { get; }

        public IReadOnlyDictionary<string, string> Metadata => _metadata;

        public Uri Blob { get; }

        public string Fingerprint { get; }

        public static IntakeRequest Create(
            string idempotencyKey,
            IReadOnlyDictionary<string, string> metadata,
            string blobReference)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                throw new ArgumentException("Idempotency key must not be blank.", nameof(idempotencyKey));
            }

            if (metadata is null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (string.IsNullOrWhiteSpace(blobReference))
            {
                throw new ArgumentException("Blob reference must not be blank.", nameof(blobReference));
            }

            Uri? parsed;
            if (!Uri.TryCreate(blobReference, UriKind.Absolute, out parsed) ||
                parsed is null ||
                parsed.Scheme != Uri.UriSchemeHttp && parsed.Scheme != Uri.UriSchemeHttps)
            {
                throw new ArgumentException(
                    "Blob reference must be an absolute HTTP or HTTPS URI.", nameof(blobReference));
            }

            var normalized = parsed!.AbsoluteUri;
            var copy = new Dictionary<string, string>(metadata, StringComparer.Ordinal);
            var fingerprint = ComputeFingerprint(normalized, copy);

            return new IntakeRequest(idempotencyKey, copy, parsed!, fingerprint);
        }

        private static string ComputeFingerprint(
            string normalizedUri,
            IReadOnlyDictionary<string, string> metadata)
        {
            var entries = metadata
                .OrderBy(kvp => kvp.Key, StringComparer.Ordinal)
                .ThenBy(kvp => kvp.Value, StringComparer.Ordinal);

            var buffer = new List<byte>();
            AppendField(buffer, normalizedUri);
            foreach (var kvp in entries)
            {
                AppendField(buffer, kvp.Key);
                AppendField(buffer, kvp.Value);
            }

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(buffer.ToArray());
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        }

        private static void AppendField(List<byte> output, string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            output.Add((byte)(valueBytes.Length >> 24));
            output.Add((byte)(valueBytes.Length >> 16));
            output.Add((byte)(valueBytes.Length >> 8));
            output.Add((byte)valueBytes.Length);
            output.AddRange(valueBytes);
        }
    }

    public sealed class IntakeRecord
    {
        private IntakeRecord(
            IntakeRequest request,
            string identifier,
            string correlationId,
            IntakeState state,
            int concurrencyToken,
            string? workerAttemptId,
            DateTime? leaseExpiryUtc)
        {
            Request = request;
            Identifier = identifier;
            CorrelationId = correlationId;
            State = state;
            ConcurrencyToken = concurrencyToken;
            WorkerAttemptId = workerAttemptId;
            LeaseExpiryUtc = leaseExpiryUtc;
        }

        public IntakeRequest Request { get; }

        public string Identifier { get; }

        public string CorrelationId { get; }

        public IntakeState State { get; }

        public int ConcurrencyToken { get; }

        public string? WorkerAttemptId { get; }

        public DateTime? LeaseExpiryUtc { get; }

        public string Fingerprint => Request.Fingerprint;

        public static IntakeRecord CreateQueued(
            IntakeRequest request,
            string identifier,
            string correlationId,
            int concurrencyToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Identifier must not be blank.", nameof(identifier));
            }

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentException("Correlation ID must not be blank.", nameof(correlationId));
            }

            return new IntakeRecord(
                request, identifier, correlationId, IntakeState.Queued, concurrencyToken, null, null);
        }

        public bool Matches(IntakeRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return string.Equals(Fingerprint, request.Fingerprint, StringComparison.Ordinal);
        }

        public TransitionResult TryClaim(
            string workerAttemptId,
            DateTime nowUtc,
            TimeSpan leaseDuration,
            int expectedConcurrencyToken)
        {
            if (string.IsNullOrWhiteSpace(workerAttemptId))
            {
                throw new ArgumentException("Worker attempt ID must not be blank.", nameof(workerAttemptId));
            }

            if (nowUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("nowUtc must be a UTC timestamp.", nameof(nowUtc));
            }

            if (leaseDuration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Lease duration must be positive.", nameof(leaseDuration));
            }

            var canClaim = State == IntakeState.Queued ||
                (State == IntakeState.Processing && LeaseExpiryUtc.HasValue && LeaseExpiryUtc.Value <= nowUtc);

            if (!canClaim || expectedConcurrencyToken != ConcurrencyToken)
            {
                return TransitionResult.NotClaimable();
            }

            var expiry = nowUtc.Add(leaseDuration);
            var processing = new IntakeRecord(
                Request, Identifier, CorrelationId, IntakeState.Processing,
                expectedConcurrencyToken, workerAttemptId, expiry);
            return TransitionResult.Claimed(processing);
        }

        public TransitionResult TryComplete(
            string workerAttemptId,
            int expectedConcurrencyToken,
            int newConcurrencyToken)
        {
            if (string.IsNullOrWhiteSpace(workerAttemptId))
            {
                throw new ArgumentException("Worker attempt ID must not be blank.", nameof(workerAttemptId));
            }

            if (State != IntakeState.Processing ||
                WorkerAttemptId != workerAttemptId ||
                ConcurrencyToken != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable();
            }

            var completed = new IntakeRecord(
                Request, Identifier, CorrelationId, IntakeState.Completed,
                newConcurrencyToken, workerAttemptId, null);
            return TransitionResult.Completed(completed);
        }

        public TransitionResult TryFail(
            string workerAttemptId,
            int expectedConcurrencyToken,
            int newConcurrencyToken)
        {
            if (string.IsNullOrWhiteSpace(workerAttemptId))
            {
                throw new ArgumentException("Worker attempt ID must not be blank.", nameof(workerAttemptId));
            }

            if (State != IntakeState.Processing ||
                WorkerAttemptId != workerAttemptId ||
                ConcurrencyToken != expectedConcurrencyToken)
            {
                return TransitionResult.NotClaimable();
            }

            var failed = new IntakeRecord(
                Request, Identifier, CorrelationId, IntakeState.Failed,
                newConcurrencyToken, workerAttemptId, null);
            return TransitionResult.Failed(failed);
        }
    }

    public sealed class TransitionResult
    {
        private TransitionResult(IntakeDecision decision, IntakeRecord? record)
        {
            Decision = decision;
            Record = record;
        }

        public IntakeDecision Decision { get; }

        public IntakeRecord? Record { get; }

        public static TransitionResult Claimed(IntakeRecord record) => new(IntakeDecision.Claimed, record);

        public static TransitionResult Completed(IntakeRecord record) => new(IntakeDecision.Completed, record);

        public static TransitionResult Failed(IntakeRecord record) => new(IntakeDecision.Failed, record);

        public static TransitionResult NotClaimable() => new(IntakeDecision.NotClaimable, null);
    }
}
```