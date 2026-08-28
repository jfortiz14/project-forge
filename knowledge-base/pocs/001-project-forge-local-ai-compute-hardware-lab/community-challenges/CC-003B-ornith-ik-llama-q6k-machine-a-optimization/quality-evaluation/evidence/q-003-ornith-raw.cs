```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Forge.DocumentIntake;

namespace Forge.DocumentIntake.Tests
{
    internal sealed class OrderMetadata : IReadOnlyDictionary<string, string>
    {
        private readonly List<KeyValuePair<string, string>> _entries;

        public OrderMetadata(IEnumerable<KeyValuePair<string, string>> entries)
        {
            _entries = new List<KeyValuePair<string, string>>(entries);
        }

        public string this[string key]
        {
            get
            {
                foreach (var entry in _entries)
                {
                    if (entry.Key == key)
                    {
                        return entry.Value;
                    }
                }
                throw new KeyNotFoundException(key);
            }
        }

        public IEnumerable<string> Keys => _entries.Select(entry => entry.Key);
        public IEnumerable<string> Values => _entries.Select(entry => entry.Value);
        public int Count => _entries.Count;
        public bool ContainsKey(string key) => _entries.Any(entry => entry.Key == key);

        public bool TryGetValue(string key, out string value)
        {
            foreach (var entry in _entries)
            {
                if (entry.Key == key)
                {
                    value = entry.Value;
                    return true;
                }
            }
            value = null!;
            return false;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _entries.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal sealed class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
    }

    internal static class Harness
    {
        private static int _failures;

        private static readonly DateTime BaseUtc = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan LeaseDuration = TimeSpan.FromMinutes(30);
        private const string BlobUri = "https://example.com/docs/abc123?tenant=acme";

        public static int Main()
        {
            Run("blank idempotency key is rejected with ArgumentException", () =>
            {
                bool threw = false;
                try
                {
                    IntakeRequest.Create("", new Uri(BlobUri), Metadata("acme"));
                }
                catch (ArgumentException)
                {
                    threw = true;
                }
                Assert(threw, "expected ArgumentException for a blank idempotency key");
            });

            Run("equivalent URI plus reordered metadata yields the same fingerprint", () =>
            {
                var orderedA = new OrderMetadata(new List<KeyValuePair<string, string>>
                {
                    new("tenant", "acme"),
                    new("locale", "en-US"),
                    new("priority", "high"),
                });
                var orderedB = new OrderMetadata(new List<KeyValuePair<string, string>>
                {
                    new("priority", "high"),
                    new("tenant", "acme"),
                    new("locale", "en-US"),
                });

                var requestA = IntakeRequest.Create("fingerprint-order", new Uri(BlobUri), orderedA);
                var requestB = IntakeRequest.Create("fingerprint-order", new Uri(BlobUri), orderedB);

                Assert(requestA.Fingerprint == requestB.Fingerprint,
                    $"expected identical fingerprints but got '{requestA.Fingerprint}' and '{requestB.Fingerprint}'");
            });

            Run("different URI query values produce different fingerprints", () =>
            {
                var requestA = IntakeRequest.Create("fingerprint-query",
                    new Uri("https://example.com/docs/abc123?tenant=acme"), Metadata("acme"));
                var requestB = IntakeRequest.Create("fingerprint-query",
                    new Uri("https://example.com/docs/abc123?tenant=globex"), Metadata("acme"));

                Assert(requestA.Fingerprint != requestB.Fingerprint,
                    "expected different fingerprints for different URI query values");
            });

            Run("a queued record claims with the expected new token", () =>
            {
                var request = IntakeRequest.Create("claim-basic", new Uri(BlobUri), Metadata("acme"));
                var record = IntakeRecord.CreateQueued(request, "id-1", "correlation-1", 1);

                var result = record.TryClaim("worker-1", BaseUtc, LeaseDuration, 1, 2);

                Assert(result.Decision == IntakeDecision.Claimed, $"expected Claimed but got {result.Decision}");
                Assert(result.Record is { State: IntakeState.Processing }, "expected Processing state after claim");
                Assert(result.Record!.ConcurrencyToken == 2, "expected concurrency token 2 after claim");
                Assert(result.Record!.WorkerAttemptId == "worker-1", "expected worker-1 as worker attempt id");
                Assert(result.Record!.LeaseExpiryUtc == BaseUtc + LeaseDuration, "expected a lease expiry to be set");
            });

            Run("reusing the same claim token is rejected with ArgumentException", () =>
            {
                var request = IntakeRequest.Create("claim-sametoken", new Uri(BlobUri), Metadata("acme"));
                var record = IntakeRecord.CreateQueued(request, "id-1", "correlation-1", 1);

                bool threw = false;
                try
                {
                    record.TryClaim("worker-1", BaseUtc, LeaseDuration, 1, 1);
                }
                catch (ArgumentException)
                {
                    threw = true;
                }
                Assert(threw, "expected ArgumentException when the expected token equals the new token");
            });

            Run("an active unexpired lease cannot be claimed by a second worker", () =>
            {
                var request = IntakeRequest.Create("claim-active-lease", new Uri(BlobUri), Metadata("acme"));
                var record = IntakeRecord.CreateQueued(request, "id-1", "correlation-1", 1);

                var first = record.TryClaim("worker-1", BaseUtc, LeaseDuration, 1, 2);
                Assert(first.Decision == IntakeDecision.Claimed, $"first claim should succeed but got {first.Decision}");

                var second = record.TryClaim("worker-2", BaseUtc + TimeSpan.FromMinutes(1), LeaseDuration, 2, 3);
                Assert(second.Decision == IntakeDecision.NotClaimable, $"expected NotClaimable but got {second.Decision}");
            });

            Run("an expired lease can be reclaimed by a second worker with a new token", () =>
            {
                var request = IntakeRequest.Create("claim-expired-lease", new Uri(BlobUri), Metadata("acme"));
                var record = IntakeRecord.CreateQueued(request, "id-1", "correlation-1", 1);

                record.TryClaim("worker-1", BaseUtc, LeaseDuration, 1, 2);

                var reclaimedTime = BaseUtc + LeaseDuration + TimeSpan.FromMinutes(5);
                var second = record.TryClaim("worker-2", reclaimedTime, LeaseDuration, 2, 3);

                Assert(second.Decision == IntakeDecision.Claimed, $"expected Claimed but got {second.Decision}");
                Assert(second.Record is { WorkerAttemptId: "worker-2", ConcurrencyToken: 3 },
                    "expected worker-2 to hold token 3 after reclaim");
                Assert(second.Record!.LeaseExpiryUtc == reclaimedTime + LeaseDuration, "expected a fresh lease expiry");
            });

            Run("a matching worker and token completes the record and clears the lease", () =>
            {
                var request = IntakeRequest.Create("complete-basic", new Uri(BlobUri), Metadata("acme"));
                var record = IntakeRecord.CreateQueued(request, "id-1", "correlation-1", 1);

                record.TryClaim("worker-1", BaseUtc, LeaseDuration, 1, 2);

                var result = record.TryComplete("worker-1", 2, 3);

                Assert(result.Decision == IntakeDecision.Completed, $"expected Completed but got {result.Decision}");
                Assert(result.Record is { State: IntakeState.Completed }, "expected Completed state");
                Assert(result.Record!.WorkerAttemptId is null, "expected no worker attempt id after completion");
                Assert(result.Record!.LeaseExpiryUtc is null, "expected no lease expiry after completion");
            });

            Console.WriteLine(_failures == 0
                ? "RESULT: all checks passed"
                : $"RESULT: {_failures} check(s) failed");

            return _failures == 0 ? 0 : 1;
        }

        private static void Run(string name, Action test)
        {
            try
            {
                test();
                Console.WriteLine($"PASS: {name}");
            }
            catch (Exception ex)
            {
                _failures++;
                Console.WriteLine($"FAIL: {name} ({ex.GetType().Name}: {ex.Message})");
            }
        }

        private static void Assert(bool condition, string failureMessage)
        {
            if (!condition)
            {
                throw new AssertionException(failureMessage);
            }
        }

        private static IReadOnlyDictionary<string, string> Metadata(string tenant)
        {
            return new Dictionary<string, string>
            {
                ["tenant"] = tenant,
                ["locale"] = "en-US",
                ["priority"] = "normal",
            };
        }
}
```