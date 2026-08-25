using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Forge.DocumentIntake;

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
    private IntakeRequest(string idempotencyKey, Uri blobReference, IReadOnlyDictionary<string, string> metadata, string fingerprint)
    {
        IdempotencyKey = idempotencyKey;
        BlobReference = blobReference;
        Metadata = metadata;
        Fingerprint = fingerprint;
    }

    public string IdempotencyKey { get; }
    public Uri BlobReference { get; }
    public IReadOnlyDictionary<string, string> Metadata { get; }
    public string Fingerprint { get; }

    public static IntakeRequest Create(string idempotencyKey, Uri blobReference, IReadOnlyDictionary<string, string> metadata)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) throw new ArgumentException("Idempotency key must not be blank.", nameof(idempotencyKey));
        if (blobReference is null) throw new ArgumentNullException(nameof(blobReference));
        if (metadata is null) throw new ArgumentNullException(nameof(metadata));
        if (!blobReference.IsAbsoluteUri || (blobReference.Scheme != Uri.UriSchemeHttp && blobReference.Scheme != Uri.UriSchemeHttps))
            throw new ArgumentException("Blob reference must be an absolute HTTP or HTTPS URI.", nameof(blobReference));

        var normalized = Normalize(blobReference);
        var copy = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var pair in metadata)
        {
            if (string.IsNullOrEmpty(pair.Key)) throw new ArgumentException("Metadata keys must not be empty.", nameof(metadata));
            if (pair.Value is null) throw new ArgumentException("Metadata values must not be null.", nameof(metadata));
            copy.Add(pair.Key, pair.Value);
        }

        var readonlyMetadata = new ReadOnlyDictionary<string, string>(copy);
        return new IntakeRequest(idempotencyKey, normalized, readonlyMetadata, CalculateFingerprint(normalized, readonlyMetadata));
    }

    private static Uri Normalize(Uri value)
    {
        var builder = new UriBuilder(value)
        {
            Scheme = value.Scheme.ToLowerInvariant(),
            Host = value.Host.ToLowerInvariant(),
            Port = value.IsDefaultPort ? -1 : value.Port,
        };
        builder.Path = builder.Path == "/" ? "/" : builder.Path.TrimEnd('/');
        return builder.Uri;
    }

    private static string CalculateFingerprint(Uri normalized, IReadOnlyDictionary<string, string> metadata)
    {
        var builder = new StringBuilder(normalized.AbsoluteUri);
        foreach (var pair in metadata.OrderBy(pair => pair.Key, StringComparer.Ordinal).ThenBy(pair => pair.Value, StringComparer.Ordinal))
            builder.Append('\n').Append(pair.Key).Append('=').Append(pair.Value);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString())));
    }
}

public sealed class IntakeRecord
{
    private IntakeRecord(string identifier, string correlationId, IntakeRequest request, IntakeState state, string? workerAttemptId, DateTime? leaseExpiryUtc, long concurrencyToken)
    {
        Identifier = identifier;
        CorrelationId = correlationId;
        Request = request;
        State = state;
        WorkerAttemptId = workerAttemptId;
        LeaseExpiryUtc = leaseExpiryUtc;
        ConcurrencyToken = concurrencyToken;
    }

    public string Identifier { get; }
    public string CorrelationId { get; }
    public IntakeRequest Request { get; }
    public IntakeState State { get; }
    public string? WorkerAttemptId { get; }
    public DateTime? LeaseExpiryUtc { get; }
    public long ConcurrencyToken { get; }

    public static IntakeRecord CreateQueued(IntakeRequest request, string identifier, string correlationId, long concurrencyToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(identifier)) throw new ArgumentException("Identifier must not be blank.", nameof(identifier));
        if (string.IsNullOrWhiteSpace(correlationId)) throw new ArgumentException("Correlation ID must not be blank.", nameof(correlationId));
        return new IntakeRecord(identifier, correlationId, request, IntakeState.Queued, null, null, concurrencyToken);
    }

    public bool Matches(IntakeRequest request) => request is not null && StringComparer.Ordinal.Equals(Request.Fingerprint, request.Fingerprint);

    public TransitionResult TryClaim(string workerAttemptId, DateTime nowUtc, TimeSpan leaseDuration, long expectedConcurrencyToken, long newConcurrencyToken)
    {
        if (string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID must not be blank.", nameof(workerAttemptId));
        if (nowUtc.Kind != DateTimeKind.Utc) throw new ArgumentException("nowUtc must be UTC.", nameof(nowUtc));
        if (leaseDuration <= TimeSpan.Zero) throw new ArgumentException("Lease duration must be positive.", nameof(leaseDuration));
        if (newConcurrencyToken == expectedConcurrencyToken) throw new ArgumentException("New concurrency token must differ.", nameof(newConcurrencyToken));
        var claimable = State == IntakeState.Queued || (State == IntakeState.Processing && LeaseExpiryUtc <= nowUtc);
        if (!claimable || ConcurrencyToken != expectedConcurrencyToken) return TransitionResult.NotClaimable();
        return TransitionResult.Claimed(new IntakeRecord(Identifier, CorrelationId, Request, IntakeState.Processing, workerAttemptId, nowUtc.Add(leaseDuration), newConcurrencyToken));
    }

    public TransitionResult TryComplete(string workerAttemptId, long expectedConcurrencyToken, long newConcurrencyToken) =>
        Transition(workerAttemptId, expectedConcurrencyToken, newConcurrencyToken, IntakeState.Completed, IntakeDecision.Completed);

    public TransitionResult TryFail(string workerAttemptId, long expectedConcurrencyToken, long newConcurrencyToken) =>
        Transition(workerAttemptId, expectedConcurrencyToken, newConcurrencyToken, IntakeState.Failed, IntakeDecision.Failed);

    private TransitionResult Transition(string workerAttemptId, long expectedConcurrencyToken, long newConcurrencyToken, IntakeState target, IntakeDecision decision)
    {
        if (string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID must not be blank.", nameof(workerAttemptId));
        if (newConcurrencyToken == expectedConcurrencyToken) throw new ArgumentException("New concurrency token must differ.", nameof(newConcurrencyToken));
        if (State != IntakeState.Processing || WorkerAttemptId != workerAttemptId || ConcurrencyToken != expectedConcurrencyToken) return TransitionResult.NotClaimable();
        return new TransitionResult(decision, new IntakeRecord(Identifier, CorrelationId, Request, target, null, null, newConcurrencyToken));
    }
}

public sealed record TransitionResult(IntakeDecision Decision, IntakeRecord? Record)
{
    public static TransitionResult NotClaimable() => new(IntakeDecision.NotClaimable, null);
    public static TransitionResult Claimed(IntakeRecord record) => new(IntakeDecision.Claimed, record);
}
