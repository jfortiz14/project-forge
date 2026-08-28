using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

IReadOnlyCollection<KeyValuePair<string, string>> Metadata(params (string Key, string Value)[] pairs) =>
    pairs.Select(pair => new KeyValuePair<string, string>(pair.Key, pair.Value)).ToArray();

Check("Required enum values", () =>
{
    Require(Enum.GetNames<IntakeState>().SequenceEqual(new[] { "Queued", "Processing", "Completed", "Failed", "DeadLettered" }), "IntakeState does not have exactly the required values.");
    Require(Enum.GetNames<IntakeDecision>().SequenceEqual(new[] { "Accepted", "Conflict", "NotClaimable", "Claimed", "Completed", "Failed" }), "IntakeDecision does not have exactly the required values.");
});

Check("Request factory accepts a client idempotency key", () =>
{
    var factory = typeof(IntakeRequest).GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
    Require(factory is not null, "IntakeRequest.Create is missing.");
    Require(factory!.GetParameters().Any(parameter => parameter.ParameterType == typeof(string) && parameter.Name == "idempotencyKey"), "IntakeRequest.Create has no client-supplied idempotencyKey parameter.");
});

Check("Fingerprint is metadata-order invariant", () =>
{
    var first = IntakeRequest.Create(new Uri("https://example.test/container/blob"), Metadata(("b", "2"), ("a", "1")));
    var second = IntakeRequest.Create(new Uri("https://example.test/container/blob"), Metadata(("a", "1"), ("b", "2")));
    Require(first.Fingerprint.SequenceEqual(second.Fingerprint), "Equivalent metadata in a different order changed the fingerprint.");
});

Check("Fingerprint includes the normalized URI", () =>
{
    var first = IntakeRequest.Create(new Uri("https://example.test/container/blob?version=one"), Metadata(("a", "1")));
    var second = IntakeRequest.Create(new Uri("https://example.test/container/blob?version=two"), Metadata(("a", "1")));
    Require(!first.Fingerprint.SequenceEqual(second.Fingerprint), "Different normalized URI values produced the same fingerprint.");
});

Check("Request fingerprint is immutable to callers", () =>
{
    var request = IntakeRequest.Create(new Uri("https://example.test/container/blob"), Metadata(("a", "1")));
    var original = request.Fingerprint[0];
    request.Fingerprint[0] ^= 0xFF;
    Require(request.Fingerprint[0] == original, "Fingerprint array is exposed mutably.");
});

Check("UTC and lease validation", () =>
{
    var request = IntakeRequest.Create(new Uri("https://example.test/container/blob"), Metadata(("a", "1")));
    var record = IntakeRecord.CreateQueued("record-1", "correlation-1", request, 7);
    var nonUtcThrown = false;
    try
    {
        record.TryClaim("worker-1", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Local), TimeSpan.FromMinutes(1), 7);
    }
    catch (ArgumentException)
    {
        nonUtcThrown = true;
    }

    Require(nonUtcThrown, "Non-UTC nowUtc was accepted.");
});

Check("Claim creates a distinct new concurrency token", () =>
{
    var request = IntakeRequest.Create(new Uri("https://example.test/container/blob"), Metadata(("a", "1")));
    var record = IntakeRecord.CreateQueued("record-2", "correlation-2", request, 7);
    var claim = record.TryClaim("worker-2", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc), TimeSpan.FromMinutes(1), 7);
    Require(claim.Decision == IntakeDecision.Claimed && claim.Record is not null, "Queued record was not claimed.");
    Require(claim.Record!.ConcurrencyToken != 7, "Claim did not apply a distinct new concurrency token.");
});

Check("Completion clears the lease", () =>
{
    var request = IntakeRequest.Create(new Uri("https://example.test/container/blob"), Metadata(("a", "1")));
    var record = IntakeRecord.CreateQueued("record-3", "correlation-3", request, 7);
    var claim = record.TryClaim("worker-3", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc), TimeSpan.FromMinutes(1), 7);
    Require(claim.Record is not null, "Claim did not return a record.");
    var completion = claim.Record!.TryComplete("worker-3", claim.Record.ConcurrencyToken, 9);
    Require(completion.Decision == IntakeDecision.Completed && completion.Record?.State == IntakeState.Completed, "Matching worker/token could not complete the record.");
    Require(completion.Record!.LeaseExpiresAtUtc is null, "Completed record retains a lease.");
});

Console.WriteLine($"RESULT failures={failures}");
return failures == 0 ? 0 : 1;
