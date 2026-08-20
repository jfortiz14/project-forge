namespace Forge.DocumentIntake
{
    public readonly record struct IntakeRequest(
        string IdempotencyKey,
        string BlobReference,
        object Metadata,
        string Fingerprint);

    public static class IntakeRequestExtensions
    {
        public static IntakeRequest Create(string idempotencyKey, string blobReference, object metadata)
        {
            if (string.IsNullOrEmpty(idempotencyKey))
                throw new ArgumentNullException(nameof(idempotencyKey));
            if (string.IsNullOrEmpty(blobReference))
                throw new ArgumentNullException(nameof(blobReference));
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            var normalizedUri = new Uri(blobReference).AbsoluteUri;
            var metadataDictionary = metadata as IDictionary<string, object> ?? new Dictionary<string, object>(metadata);
            var sortedMetadata = metadataDictionary.OrderBy(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(normalizedUri);
            foreach (var kv in sortedMetadata)
            {
                stringBuilder.Append($"#{kv.Key}:{kv.Value}");
            }

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
            var fingerprint = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return new IntakeRequest(idempotencyKey, normalizedUri, metadata, fingerprint);
        }

        public static bool Matches(this IntakeRequest request, IntakeRequest other)
        {
            return request.IdempotencyKey == other.IdempotencyKey &&
                   request.Fingerprint == other.Fingerprint;
        }
    }

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

    public readonly record struct IntakeRecord(
        string Id,
        string CorrelationId,
        string ConcurrencyToken,
        IntakeState State,
        IntakeRequest Request,
        string WorkerAttemptId,
        DateTime LeaseExpiry,
        IntakeDecision Decision);

    public static class IntakeRecordExtensions
    {
        public static IntakeRecord CreateQueued(string id, string correlationId, string concurrencyToken)
        {
            return new IntakeRecord(id, correlationId, concurrencyToken, IntakeState.Queued, default, null,
                DateTime.MinValue, IntakeDecision.Accepted);
        }

        public static IntakeDecision TryClaim(
            this IntakeRecord record,
            string workerAttemptId,
            DateTime nowUtc,
            TimeSpan leaseDuration,
            string expectedConcurrencyToken,
            string newConcurrencyToken)
        {
            if (record.State != IntakeState.Queued && record.State != IntakeState.Processing)
                return IntakeDecision.NotClaimable;

            if (record.ConcurrencyToken != expectedConcurrencyToken)
                return IntakeDecision.NotClaimable;

            if (record.State == IntakeState.Processing && record.LeaseExpiry < nowUtc)
                return IntakeDecision.NotClaimable;

            var newLeaseExpiry = nowUtc + leaseDuration;
            if (leaseDuration <= TimeSpan.Zero)
                throw new ArgumentException("leaseDuration must be positive");

            return IntakeDecision.Claimed;
        }

        public static IntakeDecision TryComplete(
            this IntakeRecord record,
            string workerAttemptId,
            string expectedConcurrencyToken,
            string newConcurrencyToken)
        {
            if (record.State != IntakeState.Processing)
                return IntakeDecision.NotClaimable;

            if (record.WorkerAttemptId != workerAttemptId)
                return IntakeDecision.NotClaimable;

            if (record.ConcurrencyToken != expectedConcurrencyToken)
                return IntakeDecision.NotClaimable;

            return IntakeDecision.Completed;
        }

        public static IntakeDecision TryFail(
            this IntakeRecord record,
            string workerAttemptId,
            string expectedConcurrencyToken,
            string newConcurrencyToken)
        {
            if (record.State != IntakeState.Processing)
                return IntakeDecision.NotClaimable;

            if (record.WorkerAttemptId != workerAttemptId)
                return IntakeDecision.NotClaimable;

            if (record.ConcurrencyToken != expectedConcurrencyToken)
                return IntakeDecision.NotClaimable;

            return IntakeDecision.Failed;
        }
    }
}
