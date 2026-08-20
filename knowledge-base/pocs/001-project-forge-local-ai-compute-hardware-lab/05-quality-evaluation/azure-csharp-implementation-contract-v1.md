# Azure C# Implementation Contract v1 — Intake Domain

> **Reference architecture:** `azure-csharp-reference-architecture-v1.md`  
> **Purpose:** Small, executable domain boundary for LLM quality evaluation.  
> **Non-goal:** No Azure SDK, network call, persistence implementation, or production deployment code.

## Required Source API

Produce a single C# source file in namespace `Forge.DocumentIntake` containing:

- `IntakeState`: `Queued`, `Processing`, `Completed`, `Failed`, `DeadLettered`.
- Immutable request/record/result types required by the services below.
- `IIntakeStore` asynchronous abstraction for conditional record creation, record read, claim, completion, and terminal failure. Each conditional mutation must receive the expected concurrency token and return a success/conflict result; it must not overwrite blindly.
- `IWorkQueue` asynchronous abstraction to enqueue the accepted record identifier and correlation ID.
- `IntakeApiService.AcceptAsync` accepting a client-supplied idempotency key, normalized blob reference, immutable metadata, and `CancellationToken`.
- `IntakeWorkerService.TryProcessAsync` accepting a record identifier, worker attempt ID, current timestamp, lease duration, and `CancellationToken`.

## Required Semantics

- Reject null/blank idempotency keys and invalid blob references.
- Compute a deterministic request fingerprint from the normalized blob reference plus metadata in key-sorted order. Do not use a randomly generated correlation ID as the idempotency key.
- A successful new request conditionally creates a `Queued` record, enqueues once, and returns accepted.
- If the key already exists with the same fingerprint, return the existing record/result without re-enqueueing.
- If the key exists with another fingerprint, return conflict.
- A worker may claim only `Queued` records, or `Processing` records whose lease is expired, and only through a conditional concurrency-token mutation. A terminal record cannot be claimed.
- Completion and terminal failure must only succeed for the matching worker attempt and expected concurrency token.
- Do not implement an automatic dead-letter replay loop.
- Use `CancellationToken` for all asynchronous public operations. Do not use external packages, Azure SDKs, static mutable state, or `async void`.

## Output Boundary

The generated source must compile under ordinary modern .NET tooling with no package restore. It may use BCL namespaces only. Do not include explanation, Markdown fences, tests, or placeholders marked TODO.
