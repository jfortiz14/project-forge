# Azure C# Reference Architecture v1 — Document Intake

> **Purpose:** Human-reviewed implementation boundary for FORGE Quality Contract v1.  
> **Scope:** Synthetic/public workload only. This is an evaluation architecture, not a production deployment template.

## 1. Components and Responsibilities

- **Intake API:** ASP.NET Core endpoint accepts document metadata, a blob reference, and a client-supplied `Idempotency-Key`. It never accepts document content.
- **Status store:** Azure Table Storage is the durable source of truth for the idempotency record and processing state.
- **Queue:** Azure Service Bus transports a compact message containing the record identifier and correlation ID.
- **Worker:** A separate .NET worker reads the queue, claims a queued record, processes it, and records the terminal outcome.
- **Observability:** OpenTelemetry-compatible structured logs, traces, and metrics flow with a correlation ID. The precise exporter/configuration is an environment decision.

## 2. Idempotency and Intake Behavior

The client must provide `Idempotency-Key`; the API must not generate it for the client. The API derives a deterministic record key from the approved request scope and the key. It conditionally inserts a `Queued` record into Table Storage before sending the Service Bus message.

If the record already exists and its immutable request fingerprint matches, the API returns the existing accepted/status result without enqueuing duplicate work. If the key exists with a different fingerprint, the API rejects the request as a conflict. The request fingerprint includes the normalized blob reference and stable metadata fields selected by the contract.

## 3. Concurrency and State Model

Allowed durable states are `Queued`, `Processing`, `Completed`, `Failed`, and `DeadLettered`. A worker claims `Queued` using an ETag-based conditional update that records a worker attempt ID and lease-expiry timestamp. Only the holder of a valid claim may write the completion/failure result.

If a worker stops before completion, a later worker may reclaim an expired `Processing` record through another conditional update. Every state transition is validated; duplicate queue deliveries become no-ops when a record is already terminal or actively leased.

## 4. Identity, Authorization, and References

Use separate Microsoft Entra managed identities for API and worker when practical. Grant least privilege at the resource scope: API can send to its queue and create/read intake records; worker can receive/settle queue messages, read the approved blob reference, and read/update status records. No connection strings or secrets belong in application configuration.

The worker validates that a blob reference belongs to an approved storage/account/container scope before accessing it. Authorization details and role assignments are deployment artifacts, not hard-coded in C#.

## 5. Failure and Dead-Letter Operations

Transient failures leave the work eligible for the configured, environment-owned retry policy. On retry exhaustion or non-retryable failure, the handler records a failure classification and correlation ID, then allows the message to enter the Service Bus dead-letter path according to configured policy.

Dead-letter inspection and replay require an authorized operator or a separately authorized operational tool. Replay criteria, remediation, audit logging, and a maximum replay policy must be defined outside the worker; the worker must not automatically requeue all dead-letter messages.

## 6. Readiness, Observability, and Tests

Liveness checks only prove the process is running. Readiness checks verify configuration/identity acquisition and required dependency access according to an explicit timeout policy; they must not create test data. Emit intake acceptance, enqueue, claim, completion, retry, terminal failure, and dead-letter metrics/traces with the correlation ID.

The implementation must have tests for same-key/same-fingerprint retries, same-key/different-fingerprint conflict, duplicate queue delivery, concurrent claim attempts, expired-lease recovery, transient and non-retryable failures, dead-letter handoff, authorization failure, cancellation, and correlation propagation. Use unit tests for state logic and controlled integration tests for Azure resource interactions.
