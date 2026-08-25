using System;
using System.Collections.Generic;
using System.Globalization;

internal static class Program
{
    private static int Main()
    {
        int failures = 0;
        int checks = 0;

        void Check(bool condition, string label)
        {
            checks++;
            if (condition)
            {
                Console.WriteLine("PASS  " + label);
            }
            else
            {
                Console.WriteLine("FAIL  " + label);
                failures++;
            }
        }

        void CheckException<TException>(Action action, string label)
            where TException : Exception
        {
            checks++;
            try
            {
                action();
                Console.WriteLine("FAIL  " + label + " (no exception thrown)");
                failures++;
            }
            catch (TException)
            {
                Console.WriteLine("PASS  " + label);
            }
            catch (Exception other)
            {
                Console.WriteLine("FAIL  " + label + " (wrong exception: " + other.GetType().Name + ")");
                failures++;
            }
        }

        Uri BuildUri(string path, string query)
        {
            string raw = "https://example.org/documents/" + path;
            if (!string.IsNullOrEmpty(query))
            {
                raw += "?" + query;
            }

            return new Uri(raw, UriKind.Absolute);
        }

        // 1. Blank idempotency key is rejected with ArgumentException.
        CheckException<ArgumentException>(
            () => IntakeRequest.Create("   ", BuildUri("a", "v=1"), new Dictionary<string, string>()),
            "blank idempotency key rejected");

        // 2. Equivalent normalized URI plus metadata in different insertion order
        //    produces the same fingerprint.
        var metaOrder1 = new Dictionary<string, string> { { "tenant", "acme" }, { "source", "portal" } };
        var metaOrder2 = new Dictionary<string, string> { { "source", "portal" }, { "tenant", "acme" } };

        var requestA = IntakeRequest.Create("key-2", BuildUri("report", "id=42"), metaOrder1);
        var requestB = IntakeRequest.Create("key-2", BuildUri("report", "id=42"), metaOrder2);
        Check(
            string.Equals(requestA.Fingerprint, requestB.Fingerprint, StringComparison.Ordinal),
            "metadata insertion order does not change fingerprint");

        // 3. Different URI query values produce different fingerprints.
        var requestC = IntakeRequest.Create("key-3", BuildUri("report", "id=43"), metaOrder1);
        Check(
            !string.Equals(requestA.Fingerprint, requestC.Fingerprint, StringComparison.Ordinal),
            "different query values produce different fingerprints");

        // 4. A queued record claims successfully with the expected token and a
        //    distinct new token.
        var request4 = IntakeRequest.Create("key-4", BuildUri("doc", "id=4"), metaOrder1);
        var record4 = IntakeRecord.CreateQueued(
            request4,
            "record-4",
            "correlation-4",
            100);
        var claim4 = record4.TryClaim(
            "worker-1",
            new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            TimeSpan.FromMinutes(30),
            100,
            101);
        Check(
            claim4.Decision == IntakeDecision.Claimed &&
            claim4.Record != null &&
            claim4.Record.ConcurrencyToken == 101 &&
            claim4.Record.State == IntakeState.Processing,
            "queued record claims with expected and new token");

        // 5. Reusing the same token as the new claim token is rejected with
        //    ArgumentException.
        var request5 = IntakeRequest.Create("key-5", BuildUri("doc", "id=5"), metaOrder1);
        var record5 = IntakeRecord.CreateQueued(
            request5,
            "record-5",
            "correlation-5",
            200);
        CheckException<ArgumentException>(
            () => record5.TryClaim(
                "worker-1",
                new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                TimeSpan.FromMinutes(30),
                200,
                200),
            "reusing same token as new claim token rejected");

        // 6. An active unexpired Processing lease cannot be claimed by a second worker.
        var request6 = IntakeRequest.Create("key-6", BuildUri("doc", "id=6"), metaOrder1);
        var record6 = IntakeRecord.CreateQueued(
            request6,
            "record-6",
            "correlation-6",
            300);
        var claim6a = record6.TryClaim(
            "worker-1",
            new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            TimeSpan.FromMinutes(30),
            300,
            301);
        var claim6b = record6.TryClaim(
            "worker-2",
            new DateTime(2024, 1, 1, 12, 5, 0, DateTimeKind.Utc),
            TimeSpan.FromMinutes(30),
            301,
            302);
        Check(
            claim6a.Record != null && claim6a.Record.State == IntakeState.Processing &&
            claim6b.Decision != IntakeDecision.Claimed,
            "active unexpired lease cannot be claimed by second worker");

        // 7. An expired Processing lease can be reclaimed by a second worker with a
        //    new token.
        var request7 = IntakeRequest.Create("key-7", BuildUri("doc", "id=7"), metaOrder1);
        var record7 = IntakeRecord.CreateQueued(
            request7,
            "record-7",
            "correlation-7",
            400);
        var claim7a = record7.TryClaim(
            "worker-1",
            new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            TimeSpan.FromMinutes(30),
            400,
            401);
        var claim7b = record7.TryClaim(
            "worker-2",
            new DateTime(2024, 1, 1, 13, 0, 10, DateTimeKind.Utc),
            TimeSpan.FromMinutes(30),
            401,
            402);
        Check(
            claim7a.Record != null && claim7a.Record.State == IntakeState.Processing &&
            claim7b.Decision == IntakeDecision.Claimed &&
            claim7b.Record != null &&
            claim7b.Record.ConcurrencyToken == 402,
            "expired lease reclaimed by second worker with new token");

        // 8. A matching worker/token completes a record and the completed record has
        //    no lease.
        var request8 = IntakeRequest.Create("key-8", BuildUri("doc", "id=8"), metaOrder1);
        var record8 = IntakeRecord.CreateQueued(
            request8,
            "record-8",
            "correlation-8",
            500);
        var claim8 = record8.TryClaim(
            "worker-9",
            new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            TimeSpan.FromMinutes(30),
            500,
            501);
        var complete8 = record8.TryComplete("worker-9", 501, 502);
        Check(
            complete8.Decision == IntakeDecision.Completed &&
            complete8.Record != null &&
            complete8.Record.State == IntakeState.Completed &&
            string.IsNullOrEmpty(complete8.Record.WorkerAttemptId) &&
            complete8.Record.LeaseExpiryUtc == default,
            "matching worker/token completes record with no lease");

        Console.WriteLine();
        Console.WriteLine(checks - failures + "/" + checks + " checks passed");

        return failures == 0 ? 0 : 1;
    }
}