# Azure C# Domain Contract v1 — Intake State

> **Purpose:** Compile-first implementation unit for FORGE Quality Contract v1.

The deliverable is one raw C# file in namespace `Forge.DocumentIntake` using BCL namespaces only.

It must define immutable `IntakeRequest` and `IntakeRecord` types; `IntakeState` (`Queued`, `Processing`, `Completed`, `Failed`, `DeadLettered`); and `IntakeDecision` (`Accepted`, `Conflict`, `NotClaimable`, `Claimed`, `Completed`, `Failed`).

`IntakeRequest.Create` validates nonblank idempotency key, an absolute HTTP/HTTPS blob reference, and non-null metadata. It normalizes the URI and calculates a SHA-256 fingerprint from that normalized URI plus metadata sorted by ordinal key/value. Metadata order must not affect the fingerprint.

`IntakeRecord.CreateQueued` makes a new record with supplied identifier/correlation ID/concurrency token. It exposes pure methods only:

- `Matches(IntakeRequest)` compares fingerprints.
- `TryClaim(workerAttemptId, nowUtc, leaseDuration, expectedConcurrencyToken)` allows only `Queued`, or `Processing` with expired lease; returns a new `Processing` record with the supplied attempt ID, a future lease expiry, and a new supplied concurrency token. Any other state/token mismatch returns `NotClaimable`.
- `TryComplete(workerAttemptId, expectedConcurrencyToken, newConcurrencyToken)` succeeds only for matching `Processing` worker/token and returns `Completed` with no lease.
- `TryFail(workerAttemptId, expectedConcurrencyToken, newConcurrencyToken)` succeeds only for matching `Processing` worker/token and returns `Failed` with no lease.

All time values are UTC and `leaseDuration` must be positive. Reject invalid inputs with `ArgumentException`/`ArgumentNullException`. Do not use I/O, asynchronous code, Azure SDKs, external packages, mutable static state, code fences, explanation, or tests.
