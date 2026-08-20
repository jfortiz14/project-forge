# FORGE Quality Evaluation Register v1

> **Contract:** `forge-quality-contract-v1-software-architecture-coding.md`  
> **Data class:** Synthetic/public only  
> **Status:** Closed — evaluated-model comparison completed

## Q-001 — Azure Architecture and C#/.NET Planning Baseline

| Field | Value |
| --- | --- |
| Model | Qwen3 8B Q4_K_M |
| Runtime | Ollama / NVIDIA CUDA / Desktop RTX 3070 |
| Context | 4,096 |
| Thinking | Disabled explicitly with `--think=false` for the valid baseline attempt Q-001b |
| Prompt | `azure-csharp-quality-planning-v1` — synthetic document-intake service exercise |
| Required review | Requirement fidelity, technical correctness, constraint adherence, completeness, maintainability, verifiability |
| Status | Closed — planning output remained draft-only after corrective review; not eligible for implementation. |

## Evaluation Sequence

1. Q-001 — Qwen3 8B Azure/C# planning baseline.
2. Q-002 — Qwen3 8B C# domain implementation against the human-reviewed reference architecture.
3. Compile and review Q-002 before requesting generated tests.
4. Reuse reviewed prompts with peer models only after the Qwen baseline unit is closed.

## Q-002 — C# Domain Implementation

| Field | Value |
| --- | --- |
| Model | Qwen3 8B Q4_K_M / Ollama / Desktop RTX 3070 / 4,096 context / explicit no-thinking |
| Contract | `azure-csharp-reference-architecture-v1.md` |
| Deliverable | One self-contained C# source file, no external packages or Azure SDK dependency |
| Required review | Compile, API/contract fidelity, idempotency semantics, conditional-claim semantics, state transitions, cancellation, and error handling |
| Status | Closed — Qwen3 8B implementation candidates rejected. |

### Q14-002a — Generated Implementation Review

| Dimension | Result |
| --- | --- |
| Timing | 29.079 s cold load; 673.13 prompt tok/s for 450 tokens; 7.11 generation tok/s for 1,550 tokens; 247.836 s total. |
| Format | Pass — raw C# only, with BCL namespaces and no Markdown fence. |
| Improvements | Defines required state/decision types; uses static `Create` and `CreateQueued` factories; validates key/URI/tokens; normalizes URI; sorts metadata; uses SHA-256; returns new records for claim/completion/failure; correctly blocks terminal records and active leases; applies the new concurrency token on state transitions. |
| Material findings | The computed fingerprint is discarded: `IntakeRequest` has no fingerprint property and `Matches` compares raw fields rather than the required fingerprint. `nowUtc` is not validated as UTC. `TryComplete` and `TryFail` add unused `nowUtc` parameters, diverging from the required API. `CreateQueued` does not reject a null request. |
| Quality status | **Partial pass — substantial improvement, but not eligible for acceptance without a compile check and correction of the material fingerprint/UTC contract gaps.** |

### Q14-002a — Compile Check

| Field | Result |
| --- | --- |
| Artifact | `artifacts/azure-intake-q14-002a.cs` |
| Build command | `dotnet build azure-intake-q14-002a.csproj --nologo` |
| Build result | **Pass:** .NET SDK 10.0.400; 0 warnings, 0 errors. |
| Contract result | Still partial: fingerprint is computed then discarded; `Matches` does not compare it; UTC validation is absent; completion/failure signatures add an unused timestamp; null request is not rejected. |
| Next action | One corrective pass only; then compile and close Q14-002. |

### Q14-002b — Single Corrective Pass

| Dimension | Result |
| --- | --- |
| Timing | 5.404 s cold load; 796.99 prompt tok/s for 327 tokens; 6.97 generation tok/s for 2,544 tokens; 370.624 s total. |
| Contract fidelity | **Fail:** replaces the required API rather than correcting it. It removes `DeadLettered`, changes `IntakeRequest.Create` from `string blobReference`/`IReadOnlyDictionary` to `Uri`/`Dictionary`, and changes all required method signatures. |
| Material regressions | Does not preserve the `TransitionResult(IntakeDecision, nullable IntakeRecord)` record contract; fixes a 60-second lease in code despite the no-invented-limits constraint; `CreateQueued` now requires an unrelated future lease; `TryClaim` only claims queued records and no longer supports expired processing leases; use of `DateTime.UtcNow` makes transitions non-deterministic; nonabsolute URIs are not correctly rejected. |
| Closure | **Rejected — Q14-002 closed.** The original output compiled cleanly but had material contract defects. Its single permitted corrective pass regressed the contract, so no further generation attempts are authorized for this unit. |

## Q32-002 — Qwen3 32B Pure-Domain Quality Comparison

| Field | Value |
| --- | --- |
| Model | Qwen3 32B Q4_K_M / Ollama / Desktop RTX 3070 / 4,096 context / explicit no-thinking |
| Contract | `azure-csharp-domain-contract-v1.md` — same baseline domain contract as 8B and 14B |
| Purpose | Measure whether a substantially larger model improves C# contract fidelity on the existing 32 GB desktop, while recording the practical latency/offload trade-off. |
| Entry gate | No retained model; sufficient free RAM observed before load; operator accepts a potentially long run. |
| Required validation | Raw-source format, compile with .NET SDK 10.0.400, contract review; at most one corrective pass only if the initial output compiles and has isolated correctable contract defects. |
| Status | Closed — Qwen3 32B candidate rejected after compile-readiness failure. |

### Q32-002a — Generated Implementation Review

| Dimension | Result |
| --- | --- |
| Timing | 58.159 s cold load; 306.37 prompt tok/s for 450 tokens; 1.90 generation tok/s for 1,604 tokens; 901.696 s total. |
| Format | **Fail:** response includes a Markdown code fence. |
| Compile readiness | **Fail by inspection:** `IntakeRequest` exposes get-only properties without a constructor, then assigns them in an object initializer; C# rejects those assignments. |
| Improvements | Includes the required state set, SHA-256 fingerprint field, nullable worker/lease state, UTC validation, expiration-based claim behavior, expected/new concurrency-token transitions, and correct terminal-state handling. This is materially closer to the contract than the 8B/14B candidates. |
| Material findings | Calculates fingerprint from the original blob-reference rather than the normalized URI; `CreateQueued` does not validate its identifier/correlation/token/request inputs; `Matches` is implemented on `IntakeRequest`, not the required record state boundary. |
| Closure | **Rejected — Q32-002 closed.** The candidate did not pass initial compile readiness, so the predeclared one-correction allowance does not apply. |

### M-002a — Generated Implementation Review

| Dimension | Result |
| --- | --- |
| Timing | 17.929 s cold load; 1,750.08 prompt tok/s for 1,014 tokens; 50.31 generation tok/s for 1,559 tokens; 49.512 s total. |
| Format | **Fail:** response includes a Markdown code fence. |
| Compile readiness | **Fail by inspection:** calls `UriPattern.IsMatch` even though `UriPattern` is a string constant; duplicates the primary record constructor; and returns `IntakeRecord` from methods declared to return `TransitionResult`. |
| Improvements | Defines an in-type queue factory, nullable lease/worker fields, SHA-256 fingerprinting, and explicit terminal records. |
| Material findings | No required `IntakeRequest.Create` static factory/fingerprint property; URI normalization is incorrect; `nowUtc` is incorrectly required to be in the future relative to wall time; claim retains the old concurrency token and stores the new one in an unrelated field; no `Claimed` decision result is returned; terminal methods likewise do not return the required `TransitionResult`. |
| Quality status | **Fail — reject.** Ministral 3 8B does not pass this C# domain implementation unit. |

### L-002a — Generated Implementation Review

| Dimension | Result |
| --- | --- |
| Timing | 14.666 s cold load; 2,782.66 prompt tok/s for 442 tokens; 72.21 generation tok/s for 1,123 tokens; 30.381 s total. |
| Format | **Fail:** response includes a Markdown code fence despite the raw-source-only requirement. |
| Compile readiness | **Fail by inspection:** `IntakeRecord` has no `State` property although methods reference `record.State`; `Hash256(normalizedUri)` attempts to invoke a type as a method. |
| Contract fidelity | **Fail:** omits `IntakeRequest.Create` and `IntakeRecord.CreateQueued`; lacks the required fingerprint on the request; `Matches` compares unrelated fields; claim uses an `int` instead of `TimeSpan`, rejects all processing records with a lease rather than allowing expired ones, and returns `Accepted` rather than `Claimed`; completion/failure omit expected concurrency token and return no updated terminal record. |
| Quality status | **Fail — reject.** Llama 3.1 8B does not pass this C# domain implementation unit. |

## M-002 — Ministral 3 8B Pure-Domain Implementation Comparison

| Field | Value |
| --- | --- |
| Model | Ministral 3 8B Instruct 25.12 Q4_K_M / Ollama / Desktop RTX 3070 / 4,096 context |
| Contract | `azure-csharp-domain-contract-v1.md` — same contract as Q-002b/Q-002c and L-002 |
| Required validation | Raw-source format, compile with .NET SDK 10.0.400, contract review, then focused tests only if it builds |
| Status | Closed — Ministral 3 8B candidate rejected. |

## Q14-002 — Qwen3 14B Pure-Domain Implementation Comparison

| Field | Value |
| --- | --- |
| Model | Qwen3 14B Q4_K_M / Ollama / Desktop RTX 3070 / 4,096 context / explicit no-thinking |
| Contract | `azure-csharp-domain-contract-v1.md` — same implementation contract used by all 8B candidates |
| Purpose | Determine whether the larger Qwen model materially improves Azure/C# domain-code fidelity, format adherence, and compilability. This is a quality comparison, not a throughput winner. |
| Required validation | Raw-source format, compile with .NET SDK 10.0.400, contract review, then focused tests only if it builds |
| Status | Closed — Qwen3 14B candidate rejected after a regressive corrective pass. |

### Q-002a — Generated Implementation Review

| Dimension | Result |
| --- | --- |
| Timing | 5.794 s cold load; 1,893.53 prompt tok/s for 327 tokens; 68.12 generation tok/s for 1,300 tokens; 25.060 s total. |
| Format | **Fail:** response includes a Markdown code fence, contrary to the output boundary. |
| Compile readiness | **Fail by inspection:** source terminates with `return new Intake`, so it cannot compile. It also calls `ComputeFingerprint` without qualification, uses nonexistent result properties (`Fingerprint`, `State`, `ConcurrencyToken`), and uses record-constructor named arguments with wrong casing. |
| Dependency boundary | **Fail:** uses `JsonConvert.DeserializeObject`, an external package forbidden by the contract. |
| Contract fidelity | **Fail:** record lacks concurrency token and lease-expiry fields; claim is invoked with a null token; it does not support expired `Processing` claims; invalid blob references are only checked for emptiness; idempotency conflict/re-read behavior is incomplete; terminal failure is not implemented. |
| Quality status | **Fail — reject.** Do not save or compile this candidate as an implementation artifact. .NET SDK 10.0.400 is available for validation of the corrected candidate. |

### Q-002b — Reduced Pure-Domain Candidate

The correction is intentionally limited to a BCL-only pure domain model: deterministic request fingerprinting and valid record state transitions. Azure Table Storage and Service Bus adapters are deferred until this unit compiles and tests pass.

| Dimension | Result |
| --- | --- |
| Timing | 3.587 s cold load; 1,650.32 prompt tok/s for 300 tokens; 68.63 generation tok/s for 918 tokens; 17.152 s total. |
| Format | Pass — raw C# only, without Markdown fences or explanation. |
| Build validation | **Fail:** .NET SDK 10.0.400 reports four errors: invalid `Dictionary<string, object>(metadata)` construction from `object`; missing `StringBuilder`; missing `SHA256`; missing `Encoding`. It also reports a nullability warning for `WorkerAttemptId`. |
| Contract fidelity | **Fail:** required factory methods are extensions rather than `IntakeRequest.Create` / `IntakeRecord.CreateQueued`; `CreateQueued` drops its request; metadata is untyped `object`; `TryClaim` returns only a decision, does not return a new record, does not apply a new token/lease/attempt, and reverses the expired-lease condition; `TryComplete`/`TryFail` also return only decisions and do not make state transitions. |
| Quality status | **Fail — reject.** Candidate source retained only as evidence: `artifacts/azure-intake-q-002b.cs`. Correct via a narrowly specified compile-fix prompt, then rebuild. |

### Q-002c — Compile-Fix Candidate

| Dimension | Result |
| --- | --- |
| Timing | 3.346 s cold load; 2,005.36 prompt tok/s for 454 tokens; 67.68 generation tok/s for 1,618 tokens; 27.486 s total. |
| Format | Pass — raw C# with BCL using directives and no Markdown fence. |
| Build validation | **Fail:** .NET SDK 10.0.400 reports `CS0103`: `idemp` does not exist. Nine nullable-reference warnings also occur because null is assigned to non-nullable worker/lease/token fields. |
| Improvements | Uses SHA-256, validates the URI scheme, creates new records for claim/completion/failure, and correctly blocks terminal records plus active leases. |
| Material findings | `Matches` is not the required fingerprint comparison and is sensitive to metadata enumeration order. Required factories are extension methods rather than members on the advertised domain types. UTC input is not validated. Lease expiry and worker attempt are represented as non-nullable sentinel/null mixtures, weakening the contract. |
| Quality status | **Fail — reject.** Candidate source retained only as evidence: `artifacts/azure-intake-q-002c.cs`; build project: `artifacts/azure-intake-q-002c.csproj`. Stop Qwen corrective-loop attempts and compare another model against the same domain contract. |

## L-002 — Llama 3.1 8B Pure-Domain Implementation Comparison

| Field | Value |
| --- | --- |
| Model | Llama 3.1 8B Instruct Q4_K_M / Ollama / Desktop RTX 3070 / 4,096 context |
| Contract | `azure-csharp-domain-contract-v1.md` — same contract as Q-002b/Q-002c |
| Required validation | Raw-source format, compile with .NET SDK 10.0.400, contract review, then focused tests only if it builds |
| Status | Closed — Llama 3.1 8B candidate rejected. |

## Q-001a — Execution and Contract Review

| Dimension | Result |
| --- | --- |
| Timing | 14.228 s cold load; 1,756.53 prompt tok/s for 282 tokens; 68.38 generation tok/s for 1,406 tokens; 34.956 s total. |
| Operational configuration | **Fail for intended profile:** visible reasoning was emitted despite the saved no-thinking profile. This makes the observed total duration and output-token count unsuitable as the no-thinking quality baseline. |
| Constraint adherence | Partial pass — exactly six labeled sections, no implementation code/table/citation/tool use; final answer appears within the requested length, but the visible reasoning violates the intended interaction profile. |
| Requirement fidelity | Partial pass — covers API metadata/blob-reference intake, quick enqueue, Service Bus, managed identity, status store, observability, modules, and tests. |
| Material findings | Idempotency is described as a read/check before work without a concurrency-safe claim/lease or ETag/conditional-write strategy; duplicate workers could still process the same document. The worker's dead-letter handling/replay ownership is not designed. It introduces an example retry limit of five although the fixed prompt deliberately leaves limits as open decisions. |
| Additional findings | Least-privilege role assignments, Blob access authorization, message schema/versioning, poison-message diagnostics, and the chosen status-store partition/key design are not sufficiently defined. “Atomic” status updates are asserted without a concrete mechanism. |
| Quality status | **Partial pass — draft only.** Re-run with explicit no-thinking before comparing models or progressing to implementation. |

## Q-001b — Explicit No-Thinking Execution and Contract Review

| Dimension | Result |
| --- | --- |
| Timing | 3.295 s cold load; 1,829.12 prompt tok/s for 288 tokens; 68.14 generation tok/s for 526 tokens; 11.179 s total. |
| Operational configuration | Pass — no visible reasoning was emitted with explicit `--think=false`. |
| Constraint adherence | Pass — six labeled sections; no implementation code, table, citation, or tool use. |
| Requirement fidelity | Partial pass — includes ASP.NET Core intake, asynchronous Service Bus enqueue/202 response, managed identity, a status store, logs/correlation, health/metrics, and tests. |
| Material findings | **Idempotency design is insufficient.** A transaction/correlation ID generated afresh by the API cannot identify a client retry of the same request. The plan also lacks a concurrency-safe claim mechanism (for example, a conditional create/update with conflict handling) before worker side effects. |
| Additional findings | No clear dead-letter ownership/replay process; health checks are vague about dependency readiness; Azure authorization/least privilege and blob-reference authorization are not designed; partition-key strategy is unnamed; tests do not specifically cover duplicate delivery, concurrent workers, retry exhaustion, or dead-letter replay. |
| Quality status | **Partial pass — not eligible for implementation.** A corrective planning response must resolve material findings without adding unsupported Azure claims. |

## Q-001c — Corrective Planning Review

| Dimension | Result |
| --- | --- |
| Timing | 3.563 s cold load; 1,667.04 prompt tok/s for 295 tokens; 68.49 generation tok/s for 487 tokens; 10.861 s total. |
| Improvements | Introduces a client-supplied stable identifier, versioned state, managed identity/RBAC, correlation, and explicit concurrency/failure test categories. |
| Material findings | The Azure Cache for Redis lock is an unjustified new dependency and is not a sufficient source of truth for durable idempotency. “Versioned” Table Storage updates do not explain a conditional ETag operation or valid transition ownership. Periodic automated dead-letter replay can repeatedly replay poison messages; it needs an explicit authorized operator/runbook and replay criteria. |
| Additional findings | The key composition is underspecified; resource authorization omits Blob read and worker receive/settle rights; status transition model lacks lease/recovery behavior; it claims a dedicated dead-letter queue although Service Bus dead-letter semantics should be handled deliberately; output is shorter than requested 400–550-word range. |
| Quality status | **Partial pass — do not use as the implementation specification.** Human-reviewed reference architecture created in `azure-csharp-reference-architecture-v1.md`. |

## Prompt: azure-csharp-quality-planning-v1

```text
You are a senior Azure and C#/.NET application architect. Design a small, maintainable document-intake service for a public/synthetic workload.

Fixed requirements:
- An ASP.NET Core HTTP API accepts document metadata and a blob-reference, but not document content.
- The API must return quickly and enqueue background processing through Azure Service Bus.
- The worker must be idempotent and handle transient failures, retry limits, and dead-letter processing.
- Use Microsoft Entra managed identity; do not use connection strings or secrets in application configuration.
- Store processing status and idempotency state in Azure Table Storage or another Azure service you justify.
- Include structured logging, correlation IDs, health checks, and operational metrics/tracing.
- Define C#/.NET module boundaries, failure behavior, and a focused unit/integration-test strategy.
- Do not invent product-specific limits, pricing, or compliance claims.

Deliver exactly these six labeled sections:
1. Architecture and Azure service choices
2. C#/.NET module and API design
3. Data, identity, and idempotency design
4. Failure handling and operations
5. Test strategy
6. Risks, assumptions, and open decisions

Use clear technical English. Keep the answer between 450 and 600 words. Do not write implementation code, tables, citations, or use external tools.
```
