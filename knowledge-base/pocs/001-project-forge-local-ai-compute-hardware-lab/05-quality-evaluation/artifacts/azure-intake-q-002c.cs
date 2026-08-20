using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Forge.DocumentIntake
{
    public enum IntakeState { Queued, Processing, Completed, Failed, DeadLettered }
    public enum IntakeDecision { Accepted, Conflict, NotClaimable, Claimed, Completed, Failed }

    public record IntakeRequest(string IdempotencyKey, string BlobReference, IReadOnlyDictionary<string, string> Metadata, string Fingerprint);
    public record IntakeRecord(Guid Identifier, string CorrelationId, string ConcurrencyToken, IntakeState State, IntakeRequest Request, string WorkerAttemptId, DateTime LeaseExpiry, string NewConcurrencyToken);
    public record TransitionResult(IntakeDecision Decision, IntakeRecord? NewRecord);

    public static class IntakeRequestExtensions
    {
        public static IntakeRequest Create(string idempotencyKey, string blobReference, IReadOnlyDictionary<string, string> metadata)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey)) throw new ArgumentException("Idempotency key cannot be blank.", nameof(idempotencyKey));
            if (string.IsNullOrWhiteSpace(blobReference)) throw new ArgumentException("Blob reference cannot be blank.", nameof(blobReference));
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));

            Uri uri;
            if (!Uri.TryCreate(blobReference, UriKind.Absolute, out uri) || !uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) && !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Blob reference must be an absolute HTTP/HTTPS URI.", nameof(blobReference));

            var sortedMetadata = metadata.OrderBy(kv => kv.Key).ThenBy(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
            using var sha256 = SHA256.Create();
            var combined = $"{uri.AbsoluteUri}{string.Join(";", sortedMetadata.Select(kv => $"{kv.Key}:{kv.Value}"))}";
            var fingerprint = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(combined))).Replace("-", "").ToLowerInvariant();
            return new IntakeRequest(idemp, uri.AbsoluteUri, sortedMetadata, fingerprint);
        }
    }

    public static class IntakeRecordExtensions
    {
        public static IntakeRecord CreateQueued(Guid identifier, string correlationId, string concurrencyToken, IntakeRequest request)
        {
            if (identifier == Guid.Empty) throw new ArgumentException("Identifier cannot be empty.", nameof(identifier));
            if (string.IsNullOrWhiteSpace(correlationId)) throw new ArgumentException("Correlation ID cannot be blank.", nameof(correlationId));
            if (string.IsNullOrWhiteSpace(concurrencyToken)) throw new ArgumentException("Concurrency token cannot be blank.", nameof(concurrencyToken));
            if (request == null) throw new ArgumentNullException(nameof(request));
            return new IntakeRecord(identifier, correlationId, concurrencyToken, IntakeState.Queued, request, null, DateTime.MinValue, null);
        }

        public static TransitionResult TryClaim(this IntakeRecord record, string workerAttemptId, DateTime nowUtc, TimeSpan leaseDuration, string expectedConcurrencyToken, string newConcurrencyToken)
        {
            if (string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID cannot be blank.", nameof(workerAttemptId));
            if (leaseDuration <= TimeSpan.Zero) throw new ArgumentException("Lease duration must be positive.", nameof(leaseDuration));
            if (string.IsNullOrWhiteSpace(expectedConcurrencyToken)) throw new ArgumentException("Expected concurrency token cannot be blank.", nameof(expectedConcurrencyToken));
            if (string.IsNullOrWhiteSpace(newConcurrencyToken)) throw new ArgumentException("New concurrency token cannot be blank.", nameof(newConcurrencyToken));
            if (record.State == IntakeState.Completed || record.State == IntakeState.Failed || record.State == IntakeState.DeadLettered) return new TransitionResult(IntakeDecision.NotClaimable, null);
            if (record.State == IntakeState.Queued && record.ConcurrencyToken == expectedConcurrencyToken) return new TransitionResult(IntakeDecision.Claimed, new IntakeRecord(record.Identifier, record.CorrelationId, newConcurrencyToken, IntakeState.Processing, record.Request, workerAttemptId, nowUtc + leaseDuration, null));
            if (record.State == IntakeState.Processing && record.LeaseExpiry < nowUtc && record.ConcurrencyToken == expectedConcurrencyToken) return new TransitionResult(IntakeDecision.Claimed, new IntakeRecord(record.Identifier, record.CorrelationId, newConcurrencyToken, IntakeState.Processing, record.Request, workerAttemptId, nowUtc + leaseDuration, null));
            return new TransitionResult(IntakeDecision.NotClaimable, null);
        }

        public static TransitionResult TryComplete(this IntakeRecord record, string workerAttemptId, string expectedConcurrencyToken, string newConcurrencyToken)
        {
            if (string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID cannot be blank.", nameof(workerAttemptId));
            if (string.IsNullOrWhiteSpace(expectedConcurrencyToken)) throw new ArgumentException("Expected concurrency token cannot be blank.", nameof(expectedConcurrencyToken));
            if (string.IsNullOrWhiteSpace(newConcurrencyToken)) throw new ArgumentException("New concurrency token cannot be blank.", nameof(newConcurrencyToken));
            if (record.State != IntakeState.Processing || record.WorkerAttemptId != workerAttemptId || record.ConcurrencyToken != expectedConcurrencyToken) return new TransitionResult(IntakeDecision.NotClaimable, null);
            return new TransitionResult(IntakeDecision.Completed, new IntakeRecord(record.Identifier, record.CorrelationId, newConcurrencyToken, IntakeState.Completed, record.Request, null, DateTime.MinValue, null));
        }

        public static TransitionResult TryFail(this IntakeRecord record, string workerAttemptId, string expectedConcurrencyToken, string newConcurrencyToken)
        {
            if (string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID cannot be blank.", nameof(workerAttemptId));
            if (string.IsNullOrWhiteSpace(expectedConcurrencyToken)) throw new ArgumentException("Expected concurrency token cannot be blank.", nameof(expectedConcurrencyToken));
            if (string.IsNullOrWhiteSpace(newConcurrencyToken)) throw new ArgumentException("New concurrency token cannot be blank.", nameof(newConcurrencyToken));
            if (record.State != IntakeState.Processing || record.WorkerAttemptId != workerAttemptId || record.ConcurrencyToken != expectedConcurrencyToken) return new TransitionResult(IntakeDecision.NotClaimable, null);
            return new TransitionResult(IntakeDecision.Failed, new IntakeRecord(record.Identifier, record.CorrelationId, newConcurrencyToken, IntakeState.Failed, record.Request, null, DateTime.MinValue, null));
        }

        public static bool Matches(this IntakeRecord record, IntakeRequest request) => record.Request.IdempotencyKey == request.IdempotencyKey && record.Request.BlobReference == request.BlobReference && record.Request.Metadata.SequenceEqual(request.Metadata);
    }
}
