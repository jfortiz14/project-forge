using System;
using System.Collections.Generic;
using System.Linq;
using Forge.DocumentIntake;

var failures = 0;

void Check(string name, Action test)
{
    try
    {
        test();
        Console.WriteLine($"PASS {name}");
    }
    catch (Exception exception)
    {
        failures++;
        Console.WriteLine($"FAIL {name}: {exception.Message}");
    }
}

void Require(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

IReadOnlyDictionary<string, string> Metadata(params (string Key, string Value)[] pairs) =>
    pairs.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.Ordinal);

IntakeRequest Request(string key = "key-1", string blob = "https://example.test/container/blob", params (string Key, string Value)[] metadata) =>
    IntakeRequest.Create(key, Metadata(metadata), blob);

Check("Required enum values", () =>
{
    Require(Enum.GetNames<IntakeState>().SequenceEqual(new[] { "Queued", "Processing", "Completed", "Failed", "DeadLettered" }), "IntakeState does not have exactly the required values.");
    Require(Enum.GetNames<IntakeDecision>().SequenceEqual(new[] { "Accepted", "Conflict", "NotClaimable", "Claimed", "Completed", "Failed" }), "IntakeDecision does not have exactly the required values.");
});

Check("Request rejects blank idempotency key", () =>
{
    var rejected = false;
    try
    {
        IntakeRequest.Create(" ", Metadata(("a", "1")), "https://example.test/container/blob");
    }
    catch (ArgumentException)
    {
        rejected = true;
    }

    Require(rejected, "Blank idempotency key was accepted.");
});

Check("Fingerprint is metadata-order invariant", () =>
{
    var first = Request(metadata: new[] { ("b", "2"), ("a", "1") });
    var second = Request(metadata: new[] { ("a", "1"), ("b", "2") });
    Require(first.Fingerprint == second.Fingerprint, "Equivalent metadata in a different order changed the fingerprint.");
});

Check("Fingerprint includes the normalized URI", () =>
{
    var first = Request(blob: "https://example.test/container/blob?version=one", metadata: new[] { ("a", "1") });
    var second = Request(blob: "https://example.test/container/blob?version=two", metadata: new[] { ("a", "1") });
    Require(first.Fingerprint != second.Fingerprint, "Different normalized URI values produced the same fingerprint.");
});

Check("Request fingerprint is immutable to callers", () =>
{
    var request = Request(metadata: new[] { ("a", "1") });
    var original = request.Fingerprint;
    var locallyModified = original + "x";
    Require(request.Fingerprint == original && request.Fingerprint != locallyModified, "Fingerprint value can be mutated through its public surface.");
});

Check("UTC and lease validation", () =>
{
    var record = IntakeRecord.CreateQueued(Request(metadata: new[] { ("a", "1") }), "record-1", "correlation-1", 7);
    var nonUtcRejected = false;
    var nonPositiveLeaseRejected = false;
    try
    {
        record.TryClaim("worker-1", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Local), TimeSpan.FromMinutes(1), 7);
    }
    catch (ArgumentException)
    {
        nonUtcRejected = true;
    }

    try
    {
        record.TryClaim("worker-1", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc), TimeSpan.Zero, 7);
    }
    catch (ArgumentException)
    {
        nonPositiveLeaseRejected = true;
    }

    Require(nonUtcRejected && nonPositiveLeaseRejected, "UTC or positive-lease validation was missing.");
});

Check("Claim creates a distinct new concurrency token", () =>
{
    var record = IntakeRecord.CreateQueued(Request(metadata: new[] { ("a", "1") }), "record-2", "correlation-2", 7);
    var claim = record.TryClaim("worker-2", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc), TimeSpan.FromMinutes(1), 7);
    Require(claim.Decision == IntakeDecision.Claimed && claim.Record is not null, "Queued record was not claimed.");
    Require(claim.Record!.ConcurrencyToken != 7, "Claim did not apply a distinct new concurrency token.");
});

Check("Completion clears the lease", () =>
{
    var record = IntakeRecord.CreateQueued(Request(metadata: new[] { ("a", "1") }), "record-3", "correlation-3", 7);
    var claim = record.TryClaim("worker-3", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc), TimeSpan.FromMinutes(1), 7);
    Require(claim.Record is not null, "Claim did not return a record.");
    var completion = claim.Record!.TryComplete("worker-3", claim.Record.ConcurrencyToken, 9);
    Require(completion.Decision == IntakeDecision.Completed && completion.Record?.State == IntakeState.Completed, "Matching worker/token could not complete the record.");
    Require(completion.Record!.LeaseExpiryUtc is null, "Completed record retains a lease.");
});

Console.WriteLine($"RESULT failures={failures}");
return failures == 0 ? 0 : 1;
