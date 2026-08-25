using System;
using System.Collections.Generic;
using System.Linq;
using Forge.DocumentIntake;

var failures = 0;
void Check(string name, Action action)
{
    try { action(); Console.WriteLine($"PASS {name}"); }
    catch (Exception exception) { failures++; Console.WriteLine($"FAIL {name}: {exception.Message}"); }
}
void Require(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
IReadOnlyDictionary<string, string> Metadata(params (string Key, string Value)[] items) => items.ToDictionary(item => item.Key, item => item.Value, StringComparer.Ordinal);

Check("Request rejects blank idempotency key", () =>
{
    try { IntakeRequest.Create(" ", new Uri("https://example.test/c/b"), Metadata(("a", "1"))); throw new InvalidOperationException("No exception."); }
    catch (ArgumentException) { }
});
Check("Fingerprint uses normalized URI and sorted metadata", () =>
{
    var first = IntakeRequest.Create("key", new Uri("HTTPS://EXAMPLE.TEST:443/c/b/?v=1"), Metadata(("b", "2"), ("a", "1")));
    var second = IntakeRequest.Create("key", new Uri("https://example.test/c/b?v=1"), Metadata(("a", "1"), ("b", "2")));
    var differentQuery = IntakeRequest.Create("key", new Uri("https://example.test/c/b?v=2"), Metadata(("a", "1"), ("b", "2")));
    Require(first.Fingerprint == second.Fingerprint, "Equivalent normalized requests differ.");
    Require(first.Fingerprint != differentQuery.Fingerprint, "Query difference is absent from fingerprint.");
});
Check("Claim and completion transition", () =>
{
    var request = IntakeRequest.Create("key", new Uri("https://example.test/c/b"), Metadata(("a", "1")));
    var queued = IntakeRecord.CreateQueued(request, "record", "correlation", 1);
    var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    var claim = queued.TryClaim("worker", now, TimeSpan.FromMinutes(1), 1, 2);
    Require(claim.Decision == IntakeDecision.Claimed && claim.Record?.State == IntakeState.Processing && claim.Record.ConcurrencyToken == 2, "Claim is invalid.");
    var completed = claim.Record!.TryComplete("worker", 2, 3);
    Require(completed.Decision == IntakeDecision.Completed && completed.Record?.State == IntakeState.Completed && completed.Record.LeaseExpiryUtc is null, "Completion is invalid.");
});
Check("Expired processing lease can be reclaimed", () =>
{
    var request = IntakeRequest.Create("key", new Uri("https://example.test/c/b"), Metadata(("a", "1")));
    var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    var first = IntakeRecord.CreateQueued(request, "record", "correlation", 1).TryClaim("worker-a", now, TimeSpan.FromSeconds(1), 1, 2).Record!;
    var reclaimed = first.TryClaim("worker-b", now.AddSeconds(1), TimeSpan.FromMinutes(1), 2, 3);
    Require(reclaimed.Decision == IntakeDecision.Claimed && reclaimed.Record?.WorkerAttemptId == "worker-b", "Expired lease was not reclaimed.");
});
Console.WriteLine($"RESULT failures={failures}");
return failures == 0 ? 0 : 1;
