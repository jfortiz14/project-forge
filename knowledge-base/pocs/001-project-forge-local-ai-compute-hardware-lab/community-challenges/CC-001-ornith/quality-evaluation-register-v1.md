# CC-001 Ornith — Quality Evaluation Register v1

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Challenge:** `CC-001-ornith`  
> **Evaluation type:** Quality only; isolated re-evaluation  
> **Status:** Q-001 through Q-003R recorded; no autonomous acceptance  
> **Data class:** Synthetic/public only
> **Quick view:** [quality-evaluation-register-summary.md](quality-evaluation-register-summary.md)

## Purpose and Boundary

Evaluate `forge-ornith-35B-A3B-ctx4096-nothink` as an assisted drafting model for the FORGE Azure/C# document-intake workload. This register is independent from the earlier CC-001 records so that its evidence, review findings, and conclusion remain traceable.

This is **not** a performance comparison, hardware recommendation, or autonomous-approval test. Timings may be retained as run diagnostics only and must not determine a quality verdict. Do not include corporate code, credentials, customer data, PHI, secrets, or proprietary architecture details.

## Frozen Execution Configuration

| Field | Value |
| --- | --- |
| Model alias | `forge-ornith-35B-A3B-ctx4096-nothink` |
| Source model | Ornith 1.5 35B-A3B GGUF Q4_K_M |
| Runtime | Ollama |
| Context | 4,096 tokens (profile contract; verify before first run) |
| Thinking | Disabled explicitly with `--think=false` |
| Session behavior | One fresh CLI invocation per unit; no chat history; `--keepalive=0` |
| Quality contract | `../../05-quality-evaluation/forge-quality-contract-v1-software-architecture-coding.md` |
| Reference architecture | `../../05-quality-evaluation/azure-csharp-reference-architecture-v1.md` |
| Domain contract | `../../05-quality-evaluation/azure-csharp-domain-contract-v1.md` |

## Evaluation Sequence and Gates

1. **Q-001 Planning:** assess a design against the fixed Azure requirements. Its result measures autonomous architecture/planning capability only.
2. **Q-002 Implementation:** normally follows a reviewed Q-001 plan. It may also proceed after a Q-001 failure **only** as an explicitly independent, human-intervened unit: a human freezes and supplies the reference architecture and domain contract, excludes the failed planning output from both the prompt and the review, and records that intervention. This isolates implementation capability from architecture-planning capability. Compile the generated candidate before judging it.
3. **Q-002R Pipeline-conformant re-run:** used only when a prior Q-002 capture is confounded by terminal presentation artifacts or does not follow the raw-source pipeline. It preserves Q-002 as historical evidence, repeats the same frozen implementation prompt/configuration, saves standard output directly as raw `.cs`, and reruns the compile gate. It is a reproducibility/measurement unit, not a corrective prompt or an opportunity to improve the model answer.
4. **Q-002H Minimal Human Repair:** begins only after a generated candidate has failed compilation and an operator explicitly authorizes it. It creates a separate derivative, preserves the raw model output, and permits only the smallest counted human changes needed to compile. Its purpose is to measure repair effort needed to make the artifact testable; any post-repair test or review result must be attributed to the combined human-plus-model artifact, never to autonomous model capability.
5. **Q-003 Test generation:** run only against the frozen human-reviewed domain contract and a compilable target API.
6. **Q-004 Code/architecture review:** run only with the exact code and tests under review included in the prompt.

Each unit is accepted only when it has no critical or material finding under the quality contract. A failed unit is recorded as failed; it receives at most one explicitly documented corrective pass, never unlimited retries. A failure in one unit does not by itself close the overall quality evaluation or prohibit a later unit whose independent entry gate is documented and satisfied.

Q-002R and Q-002H do not change the verdict of Q-002. Q-002R answers whether a pipeline-conformant capture changes the compile outcome. Q-002H answers how much explicitly counted human intervention is needed to reach a testable artifact. Neither is a retry that may convert a failed autonomous implementation into a pass.

## Q-001 — Azure/C# Planning

**Prompt ID:** `azure-csharp-quality-planning-v1` (frozen; reproduced from the FORGE register)

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

### Q-001 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Model/profile verified | Pass — Modelfile shows `PARAMETER num_ctx 4096`, the named alias, and a template that suppresses the think block. |
| Exact prompt executed | Pass — `azure-csharp-quality-planning-v1`, with `--think=false --verbose --keepalive=0`. |
| Output captured | Pass — six labeled sections plus Ollama timing diagnostics; 1,004 generated tokens. |
| Format and constraint adherence | **Fail** — the six requested sections were present and no code, tables, citations, or tool use appeared; however, the answer contains **718 words**, exceeding the required 450–600-word range. |
| Requirement fidelity | Partial — covers HTTP intake, Service Bus, managed identity, Table Storage, logging/correlation, health/metrics, and unit/integration testing. |
| Technical correctness and Azure operability | Partial — the API/worker separation and at-least-once processing direction are reasonable, but the durable idempotency and least-privilege design are incomplete. |
| Critical findings | None observed. |
| Material findings | 1. The operation ID is returned/generated by the API but is not defined as a client-supplied stable idempotency key; a client retry can therefore create another operation and repeat side effects. 2. An ETag “conditional upsert” is named but no atomic create/claim, owner/lease, or valid transition/recovery protocol is specified; it does not by itself demonstrate duplicate-worker safety. 3. The API is granted `Storage Blob Data Contributor` despite not reading blob content, while the worker’s required Table/Blob roles and authorization of the supplied blob reference are not precisely designed. |
| Minor findings | The Table keys are described inconsistently (`PartitionId` as operation ID and a “fixed partition key”). Dead-letter handling stops at later inspection and lacks an owned, authorized replay/runbook decision. The proposed in-memory/mocked dependencies do not establish a true Azure integration test. |
| Quality verdict | **Fail — not eligible to drive implementation.** The word-limit violation and the two idempotency gaps are material under the FORGE quality contract. |

### Q-001 Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Total duration | 1m41.0465043s |
| Load duration | 1m4.3417753s |
| Prompt evaluation | 309 tokens; 1.818935s; 169.88 tokens/s |
| Generation | 1,004 tokens; 34.865926s; 28.80 tokens/s |

### Q-001 Review Rationale

The design correctly keeps document content out of the API, returns `202 Accepted` after queueing, and identifies reasonable component boundaries. It also distinguishes transient from permanent failures and names useful operational signals. These are useful planning ideas, not acceptance evidence.

The principal defect is the identifier lifecycle. A new API-generated operation ID cannot deduplicate a retried request unless a stable, client-supplied idempotency key is accepted, validated, and used in an atomic create-or-return decision before enqueueing. Worker-side ETag handling must then make an explicit conditional claim to `Processing`, with a lease/expiry and recovery rule before any external side effect. The response does not define those guarantees.

The deployment identity design also over-grants the API and does not establish authorization that the requested blob reference belongs to an allowed scope. Those issues must be corrected by a human-reviewed plan before implementation.

### Q-001 Disposition

One corrective planning pass is permitted by this register if explicitly requested. It must preserve the frozen workload while addressing every material finding and obeying the 450–600-word limit. No implementation unit will run until that pass is reviewed or a human reference design is selected.

## Q-001C — Corrective Azure/C# Planning (Single Allowed Pass)

> **Status:** Prompt frozen; awaiting user-operated execution  
> **Precondition:** Q-001 failed. This is the only corrective planning pass for this model/configuration in this register.

**Prompt ID:** `azure-csharp-quality-planning-corrective-v1`

```text
You are revising a planning draft for a small, maintainable Azure/C# document-intake service for a public/synthetic workload. Produce a corrected design, not implementation code.

The original fixed requirements remain:
- An ASP.NET Core HTTP API accepts document metadata and a blob-reference, but not document content.
- The API returns quickly after durable acceptance and enqueues background processing through Azure Service Bus.
- The worker is idempotent and handles transient failures, retry limits, and dead-letter processing.
- Use Microsoft Entra managed identity; no connection strings or secrets in application configuration.
- Store processing status and idempotency state in Azure Table Storage or another Azure service you justify.
- Include structured logging, correlation IDs, health checks, operational metrics/tracing, C#/.NET module boundaries, failure behavior, and focused unit/integration tests.
- Do not invent product-specific limits, pricing, or compliance claims.

Correct these mandatory defects explicitly:
- The client supplies a stable idempotency key that is reused for a retry; the API makes an atomic create-or-return decision before enqueueing.
- Define a concurrency-safe worker claim, legal state transitions, lease expiry/recovery, and the rule that prevents duplicate external side effects.
- Apply least privilege: state the distinct API and worker Azure roles needed for Service Bus, Table Storage, and blob access. Authorize the submitted blob reference against an allowed scope.
- Define dead-letter ownership, inspection criteria, and an authorized replay procedure; do not propose automatic replay of poison messages.

Deliver exactly these six labeled sections:
1. Architecture and Azure service choices
2. C#/.NET module and API design
3. Data, identity, and idempotency design
4. Failure handling and operations
5. Test strategy
6. Risks, assumptions, and open decisions

Use clear technical English. Keep the answer between 450 and 600 words. Do not write implementation code, tables, citations, or use external tools.
```

### Q-001C Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Exact frozen prompt executed | Pass — `azure-csharp-quality-planning-corrective-v1` under the frozen Ornith profile. |
| Output captured | Pass — six labeled sections plus Ollama diagnostics; 1,097 generated tokens. |
| 450–600-word constraint | **Fail** — 834 words observed. |
| Stable client idempotency and atomic admission | Partial — introduces a client idempotency key and an insert-or-return intent, but contradicts itself by returning `409 Conflict` for an accepted retry and does not resolve the persistence-versus-enqueue failure gap. |
| Worker claim, transition, lease, and recovery semantics | Partial — names Accepted/Processing/Succeeded/Failed and lease expiry, but does not state an atomic conditional claim with owner/lease token and version/ETag check. Persisting a marker *before* an external action risks a lost action after a crash; persisting it after risks duplication. |
| Least privilege and blob-reference authorization | Partial — correctly removes broad API blob-write access and requires scoped reference authorization, but names non-existent/generic roles (`Table Storage Data Writer`, `Service Bus Receive, Lease, and Dispose`) rather than the relevant built-in data roles. |
| Dead-letter ownership and controlled replay | Pass — assigns authorized human inspection and manual replay only after remediation. |
| Critical findings | None observed. |
| Material findings | 1. The response violates the frozen word range. 2. It does not define a reliable atomic handoff/outbox or reconciliation rule between durable acceptance and Service Bus enqueue; an accepted record can survive an enqueue failure and prevent a later retry from enqueuing. 3. The retry response semantics conflict: one section says return the original identifier, another says `409 Conflict`. 4. The proposed processed-marker ordering does not safely guarantee either delivery or at-most-once external effects across a crash. |
| Minor findings | “FIFO-eligible” is not a decision on whether sessions/order are actually used. One active consumer per instance does not solve multi-instance concurrency. The stated roles need replacement with `Storage Table Data Contributor`, `Azure Service Bus Data Sender`, and `Azure Service Bus Data Receiver` at the smallest applicable scope. |
| Quality verdict | **Fail — Q-001 closed.** This was the single allowed corrective pass; no further planning regeneration is permitted for this model/configuration in this register. |

### Q-001C Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Total duration | 1m44.0334034s |
| Load duration | 1m5.3327453s |
| Prompt evaluation | 441 tokens; 2.399242s; 183.81 tokens/s |
| Generation | 1,097 tokens; 36.277863s; 30.24 tokens/s |

### Q-001C Review Rationale and Closure

The pass is better than Q-001 on the client-supplied idempotency key, scoped blob references, state names, and manual dead-letter replay. It nevertheless cannot be accepted as an implementation specification.

The API cannot make its Table Storage admission record and its Service Bus send one atomic action with the design stated here. It needs a deliberately specified durable outbox/reconciliation pattern, or another justified atomic boundary, so an enqueue failure after acceptance is repaired rather than hidden by a later create-or-return response. Similarly, a durable marker alone cannot solve a crash on either side of a non-transactional external effect; the design must define an idempotent recipient/effect key or a suitable transactional boundary.

Role names were checked against Microsoft’s current built-in-role documentation. The valid relevant roles are `Azure Service Bus Data Sender`, `Azure Service Bus Data Receiver`, and `Storage Table Data Contributor`; receiving, lease management, and settlement are not separate Service Bus RBAC roles. See [Microsoft Learn: Azure built-in roles](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles).

**Decision:** Q-001 is closed as **fail** after its permitted corrective pass. Do not start Q-002 from this model output. A human-reviewed reference architecture may be used as the input for a separately scoped implementation-quality evaluation, but that would be a new unit rather than a repair of Q-001.

## Q-002 — Human-Intervened C# Domain Implementation

> **Status:** Prompt frozen; awaiting user-operated execution  
> **Scope:** Independent implementation-capability measurement. Q-001 and Q-001C remain closed as **FAIL** and are not input evidence for this unit.

### Human Intervention and Source Boundary

The permitted human intervention is the selection of the frozen, reviewed source material below. No generated Ornith planning output is used to design or judge Q-002.

- `../../05-quality-evaluation/azure-csharp-reference-architecture-v1.md`
- `../../05-quality-evaluation/azure-csharp-domain-contract-v1.md`

Q-002 deliberately evaluates the pure C# domain boundary that represents the reviewed architecture’s idempotency and state model. Azure SDK adapters, Service Bus, Table Storage I/O, HTTP endpoints, retries, deployment RBAC, and observability exporters are excluded from this generated artifact.

### Frozen Prompt

**Prompt ID:** `azure-csharp-domain-implementation-human-baseline-v1`

```text
You are implementing a pure, deterministic C# domain model for a synthetic document-intake service. This is an implementation-quality task, not an architecture-design task. Use only the fixed contract below; do not use or infer any earlier model planning output.

Write one complete raw C# source file in namespace Forge.DocumentIntake, using BCL namespaces only.

Required public domain types:
- IntakeState with exactly: Queued, Processing, Completed, Failed, DeadLettered.
- IntakeDecision with exactly: Accepted, Conflict, NotClaimable, Claimed, Completed, Failed.
- Immutable IntakeRequest and IntakeRecord types.

IntakeRequest.Create must:
- reject a blank idempotency key, a null metadata collection, and a blob reference that is not an absolute HTTP or HTTPS URI;
- normalize the URI; and
- calculate a SHA-256 fingerprint from that normalized URI plus metadata sorted by ordinal key and ordinal value. Metadata input order must not change the fingerprint.

IntakeRecord.CreateQueued must create a Queued record from a supplied request, identifier, correlation ID, and concurrency token. It must expose only pure methods. Matches(IntakeRequest) compares request fingerprints.

State-transition requirements:
- TryClaim(workerAttemptId, nowUtc, leaseDuration, expectedConcurrencyToken) permits only Queued, or Processing with an expired lease. Validate that nowUtc is UTC and leaseDuration is positive. On success, return a new Processing record with the supplied attempt ID, a future lease expiry, and a supplied new concurrency token. On an invalid state or expected-token mismatch, return NotClaimable without a transitioned record.
- TryComplete(workerAttemptId, expectedConcurrencyToken, newConcurrencyToken) permits only the matching Processing worker/token and returns a new Completed record with no lease.
- TryFail(workerAttemptId, expectedConcurrencyToken, newConcurrencyToken) permits only the matching Processing worker/token and returns a new Failed record with no lease.
- A transition result may use one additional public BCL-only immutable result type to carry the required IntakeDecision and the transitioned IntakeRecord. Do not rename or omit the required types or methods.

Reject invalid inputs with ArgumentException or ArgumentNullException. Do not use I/O, asynchronous code, Azure SDKs, external packages, mutable static state, code fences, explanation, tests, a Program class, or any text outside the C# source file.
```

### Raw-Evidence and Compile Procedure

1. The operator saves the entire unedited terminal output to `evidence/q-002-ornith-raw-output.txt` during execution.
2. The agent preserves that file unchanged. It is the source evidence, including any non-source text or timing diagnostics.
3. The agent copies the generated artifact exactly as returned into a build workspace and runs `dotnet build`. **No semantic code evaluation occurs before this compile gate.**
4. Only a successful build advances to contractual tests. The human/contract review occurs after the relevant test evidence is available. A build failure is recorded with its evidence and stops the test stage; a code fence or explanation is a format finding, but compilation is still attempted.

### Q-002 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Frozen prompt/model configuration | Pass — `azure-csharp-domain-implementation-human-baseline-v1`; `forge-ornith-35B-A3B-ctx4096-nothink`; explicit no-thinking. |
| Raw output preserved before evaluation | Pass — [q-002-ornith-raw-output.txt](evidence/q-002-ornith-raw-output.txt), SHA-256 `084278385BE61BB2060F8546743F72A04565D2F471F32D4417B87B96E6370A1F`, 23,764 bytes. It was not edited. |
| Output format | **Fail** — response begins/ends with a Markdown `csharp` fence despite the raw-source-only constraint. |
| Literal `dotnet build` | **Fail** — `dotnet build OrnithQ002.csproj --nologo` compiled the byte-identical raw output and reported 188 errors. The first errors are the opening backticks and terminal ANSI characters preserved in the raw evidence. |
| Transport-normalized diagnostic build | **Fail** — after removing only ANSI control sequences and the outer Markdown fences in a separate derived copy, with no source-code repair, `dotnet build OrnithQ002.csproj --nologo` still reported 63 errors. Examples include malformed/duplicated source around `Appenbuilder...` and `record record)`. |
| Required types and factories | Not accepted — the text attempts the enums, `IntakeRequest`, `IntakeRecord`, and `TransitionResult`, but the artifact does not compile. |
| Fingerprint normalization and metadata-order invariance | Not accepted — the text attempts SHA-256 and ordinal sorting, but its non-compilable state prevents executable verification. |
| Claim, token, lease, and terminal transition semantics | Not accepted — a `TryClaim`/complete/fail shape is attempted, but the artifact is not a compilable implementation. It also reuses `expectedConcurrencyToken` as the claimed record’s token rather than applying a distinct new token required by the frozen prompt. |
| Invalid-input handling and dependency boundary | Partial only — BCL namespaces and several argument checks are attempted; raw-source format and compilability fail. |
| Critical findings | None observed. |
| Material findings | 1. The generated deliverable is not raw C# as requested. 2. It fails the required `dotnet build` validation in both literal and transport-normalized forms. 3. The source contains syntactically malformed/duplicated tokens beyond presentation artifacts. 4. The claim transition does not apply a distinct new concurrency token. |
| Q-002 verdict | **Fail — closed.** This is an independent implementation result and does not change Q-001’s separate closed FAIL status. |

### Q-002 Compile Evidence

| Build | Input | Result |
| --- | --- | --- |
| Literal fidelity build | Byte-identical copy of the raw evidence, including fence and terminal output | Fail — 188 errors; expected because the required raw-source output format was violated and terminal transport characters were preserved. |
| Transport-normalized diagnostic build | Derived copy with ANSI control sequences and outer fence removed only; no code tokens repaired | Fail — 63 errors. This confirms the candidate remains non-compilable without human code changes. |

Build workspace retained for reproducibility: `build/q-002-ornith/`. The original evidence file remains the authoritative record; derived build inputs are diagnostic only.

### Q-002 Closure

Q-002 measured implementation capability independently from the failed Ornith architecture draft, using only the human-reviewed reference architecture and domain contract as its baseline. The candidate did not meet the required raw-source format or compile gate. No contract review can convert a non-compilable candidate into an accepted implementation.

This closes **Q-002 only**. It does not declare the overall CC-001 quality evaluation complete; any later unit must have its own frozen input, scope, and evidence gate.

## Q-002R — Pipeline-Conformant Implementation Re-run

> **Status:** Prompt frozen; awaiting user-operated execution  
> **Relationship to Q-002:** A separate reproducibility run, not a corrective pass and not a replacement for the closed Q-002 result.

### Reason and Scope

Q-002 remains **FAIL**. Its raw-evidence capture included terminal presentation characters and a Markdown fence, which the required literal build correctly rejected. After the quality-methodology clarification, Q-002R measures the same independent implementation capability with a pipeline-conformant raw-source capture:

`Model output → save raw .cs → dotnet build → contractual tests → human/contract review`

The model alias, context, thinking mode, human-reviewed source boundary, and frozen prompt text are unchanged from Q-002. Q-002R changes only the capture method: it omits `--verbose`, writes standard output directly to a `.cs` file, and uses `--nowordwrap`. Timings are intentionally out of scope for this quality-only re-run.

### Execution and Evidence Gate

1. Save the command output directly to `evidence/q-002r-ornith-raw.cs`; do not open, edit, format, or inspect it before the build.
2. Record its SHA-256 hash.
3. Compile that exact file with `dotnet build`.
4. Run contractual tests only if the build succeeds.
5. Perform human/contract review only after the applicable build/test evidence exists.

### Q-002R Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Frozen prompt/model configuration | Pass — same frozen Q-002 prompt and `forge-ornith-35B-A3B-ctx4096-nothink` configuration; no verbose/timing capture. |
| Raw `.cs` preserved and SHA-256 recorded | Pass — [q-002r-ornith-raw.cs](evidence/q-002r-ornith-raw.cs), SHA-256 `9FE5A7D05F8DA9B5CCF3746BB2171D1C042D2904FC61D0E069911299151AC614`, 23,870 bytes. The file was not opened or edited before compilation. |
| Literal `dotnet build` result | **Fail** — `dotnet build OrnithQ002R.csproj --nologo` reported 7 errors: opening Markdown fence at line 1 and closing fence at line 360. |
| Contractual test result | N/R — not run; the build gate failed. |
| Human/contract review | N/R — not performed; the pipeline forbids semantic review before a successful build and relevant contractual tests. |
| Q-002R verdict | **Fail — closed at compile gate.** The model violated the raw-source-only output constraint; no source transformation or corrective code edit was applied. |

### Q-002R Closure

Q-002R is the pipeline-conformant result for this re-run. It proves the failure at the compile gate without relying on visual code inspection. The source is retained unchanged for evidence only. Q-002 and Q-002R are separate failed implementation measurements; neither changes the closed Q-001 planning verdict.

### Q-002R — User-Approved Fence-Only Derivation

> **Scope:** Post-closure diagnostic requested by the operator. The authoritative raw evidence remains unchanged.

The operator authorized removal of the outer Markdown fences only. A derived file was created at `build/q-002r-ornith/OrnithQ002R.fence-removed.cs`; no model code token was corrected, added, deleted, or reordered.

| Criterion | Observed result |
| --- | --- |
| Raw evidence integrity | Pass — raw SHA-256 remains `9FE5A7D05F8DA9B5CCF3746BB2171D1C042D2904FC61D0E069911299151AC614`. |
| Derived file SHA-256 | `4C7A5800256EB3105844480C2A656CC6C8D26373F8BECD6BE34326D0D7C9E24D` |
| `dotnet build` after fence-only removal | **Fail** — 1 error and 3 warnings. Error `CS1061` at line 202: `DateTime` has no `IsUniversalTime` member. Warnings `CS8625` occur at lines 187, 253, and 283. |
| Contractual tests | N/R — not run; build remains unsuccessful. |
| Human/contract review | N/R — not performed. The required pipeline prohibits semantic review before successful compilation and contractual tests. |

**Outcome:** Fence removal did not make the implementation testable. Q-002R remains **FAIL** at the compile gate. A semantic review would violate the agreed methodology, so it was intentionally not performed.

## Q-002H — Minimal Human Repair

> **Status:** Repair scope frozen; awaiting repair and compile evidence  
> **Question:** *Given a generated implementation that fails compilation, how much human intervention is required to make it testable?*

### Purpose and Boundary

Q-002H measures the minimum human effort needed to make the fence-removed Q-002R candidate compile and therefore eligible for contractual tests. It does **not** measure autonomous model capability, does not reopen Q-002/Q-002R, and does not permit a redesign or semantic contract repair.

The source baseline is `build/q-002r-ornith/OrnithQ002R.fence-removed.cs`, whose only known compilation error is `CS1061` at line 202 (`DateTime.IsUniversalTime` is not a .NET member). Its original raw evidence and all prior derived files remain immutable.

### Frozen Repair Rules

1. Create a new derivative; never overwrite the raw or fence-removed source.
2. Permit only the smallest edit required to remove the current compiler error. Do not change public APIs, types, state-transition behavior, validation behavior, dependency choices, formatting, or warnings unless they become compiler errors.
3. Record the exact changed line(s), human repair count, and before/after SHA-256 hashes.
4. Run `dotnet build` on the repaired derivative.
5. Only if it builds, run contractual tests. Only after test evidence, conduct a human/contract review.

### Q-002H Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Baseline source and SHA-256 | `build/q-002r-ornith/OrnithQ002R.fence-removed.cs`; `4C7A5800256EB3105844480C2A656CC6C8D26373F8BECD6BE34326D0D7C9E24D`. |
| Minimal human change(s) | **One code-line substitution:** `!nowUtc.IsUniversalTime` → `nowUtc.Kind != DateTimeKind.Utc`. No public API, transition, dependency, or formatting behavior was intentionally changed. |
| Repaired-source SHA-256 | `77481C8B49924408A87EC28417A588A9E50589B1FFA86B17D59E2754E98B097A` for `OrnithQ002H.minimal-repair.cs`. |
| `dotnet build` result | **Pass with warnings** — 0 errors, 3 `CS8625` nullability warnings. |
| Contractual test result | **Fail** — 4 of 8 checks fail: client idempotency key is absent from `IntakeRequest.Create`; normalized URIs differing by query produce the same fingerprint; the public fingerprint byte array is mutable; claim does not apply a distinct new concurrency token. Four checks pass: enum values, metadata-order invariance, UTC validation, and completion lease clearing. |
| Human/contract review | **Fail** — findings follow the test evidence below. |
| Q-002H verdict | **Testable after minimal repair, but contract fail.** Q-002H answers the repair-effort question; it does not make the generated implementation acceptable. |

### Q-002H Contract Test Evidence

The BCL-only harness at `build/q-002r-ornith/contract-tests/` ran against the minimally repaired derivative. Observed result: `failures=4`.

| Test | Result |
| --- | --- |
| Required enum values | Pass |
| Request factory accepts a client idempotency key | Fail |
| Fingerprint is metadata-order invariant | Pass |
| Fingerprint includes the normalized URI | Fail |
| Request fingerprint is immutable to callers | Fail |
| UTC and positive-lease validation | Pass |
| Claim creates a distinct new concurrency token | Fail |
| Completion clears the lease | Pass |

### Frozen Contract Test Suite

The test suite is now frozen as `q-002h-contract-tests-v1`:

- [ContractTests.csproj](tests/q-002h-contract-tests-v1/ContractTests.csproj) — SHA-256 `A99D8D1751862DB8636EB1AD0FF4334B4BF0E59A8314AD82A56A901ED1374B2A`
- [Program.cs](tests/q-002h-contract-tests-v1/Program.cs) — SHA-256 `8D95F91EFEAD7C699126893CBCDC9F03E4C0010B5B7E891901C174AB748119C2`

The frozen suite runs on .NET SDK 10.0.400, references the Q-002H repaired derivative, and reproduces `4` failures out of `8` tests. It covers exact enum values, client-supplied idempotency key, metadata-order-invariant fingerprinting, full normalized-URI participation in the fingerprint, fingerprint immutability, UTC validation, distinct claim concurrency token, and terminal completion lease clearing.

Any new or changed test is a new test-suite version and must state its reason, affected contract clause, and SHA-256 hashes. It must not overwrite `q-002h-contract-tests-v1` or silently change the recorded Q-002H result.

### Q-002H Human/Contract Review

**Material findings**

1. `IntakeRequest.Create` has no client-supplied `idempotencyKey` parameter, so it cannot validate the mandatory input defined by the frozen domain contract.
2. The fingerprint computation uses scheme, host, and local path but omits other normalized URI components such as the query. Distinct normalized blob references can therefore compare as the same request.
3. `Fingerprint` returns its backing `byte[]` directly. A caller can mutate it, violating the required immutable request type and destabilizing `Matches`.
4. `TryClaim` has no new concurrency-token input and retains `expectedConcurrencyToken` in the transitioned record. It therefore does not produce the required new token on a successful conditional claim.

**Additional findings**

- The compiled candidate retains three nullability warnings because null is passed to non-nullable record fields for worker/lease state.
- `CreateQueued` accepts an empty correlation ID, and worker attempt IDs are checked for null but not blank; these are incomplete invalid-input guards.
- The lease-expiry check uses strict `<`; the contract should explicitly decide equality behavior. This is not a repair made by Q-002H.

### Q-002H Answer and Closure

**Answer:** Starting from the fence-removed candidate, **one human code-line change** was sufficient to make the artifact compile and testable. From the literal model output, the full minimal intervention was removal of two outer Markdown-fence lines plus that one code-line change. The subsequent contractual tests still found four material contract defects.

**Decision:** Q-002H is closed as **testable but contract fail**. Any attempt to correct the material semantics would be a separate, explicitly scoped human-repair experiment; it must not be attributed to autonomous Ornith capability.

## Q-003 — Independent Contract-Test Generation

> **Status:** Reference baseline preparation in progress  
> **Purpose:** Measure whether Ornith can generate useful C# contract tests independently of its failed autonomous implementation capability.

### Why This Is a Separate Unit

Q-002/Q-002R measure autonomous implementation and Q-002H measures the effort to repair that output. None is a suitable target for test generation: Q-002 is non-compilable and Q-002H remains contract-defective. Q-003 therefore uses only a human-reviewed, compilable reference implementation with a frozen public API.

### Frozen Q-003 Pipeline

`Human reference implementation → baseline contract tests pass → model test output → save raw .cs → dotnet build → execute generated tests → seeded-defect detection → human/contract review`

The model output is evaluated as a test artifact. It must compile against the frozen reference, pass against the correct reference, and fail when targeted seeded defects are introduced. A fluent test suite, a suite that only compiles, or a suite that passes without detecting a relevant defect is not sufficient.

### Entry Gates

1. A human reference API and implementation are frozen with a source hash.
2. Its baseline contract tests are frozen, pass, and have a source hash.
3. A small set of seeded-defect variants is frozen, each mapped to a specific contract clause.
4. The Q-003 prompt identifies the exact API and test framework/output boundary.
5. No Q-002/Q-002R/Q-002H generated source is supplied to the model as the implementation target.

### Q-003 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Human reference source/API frozen | Pass — `q-003-reference-v1`, hashes and API adjudication recorded below. |
| Baseline tests pass | Pass — 4 of 4 baseline checks pass on .NET SDK 10.0.400. |
| Seeded-defect variants frozen | Pass — four independently compilable variants; hashes and expected detection behavior below. |
| Model test prompt/output captured | Pass — raw evidence retained at [q-003-ornith-generated-tests-raw.cs](evidence/q-003-ornith-generated-tests-raw.cs), SHA-256 `38E63DFE41F13A5D8AD7CC7E62882C0C5123F8FFFE1A33F809C60BA748DD01A6`, 15,384 bytes. |
| Generated tests build and run | **Fail at build gate** — `dotnet run --project OrnithQ003Tests.csproj` reports 7 compilation errors caused by Markdown fences at lines 1 and 199. The test executable was not produced or run. |
| Seeded defects detected | N/R — not measured; generated tests did not compile. |
| Human/contract review | N/R — not performed; the pipeline requires successful build and execution before review. |
| Q-003 verdict | **Fail — closed at compile gate.** The raw-source-only output boundary was not met. |

### Frozen Human Reference — `q-003-reference-v1`

The reference is human-authored. It is not derived from Q-002, Q-002R, or Q-002H output. Its only role is to provide a valid, stable target for measuring generated tests.

| Artifact | SHA-256 |
| --- | --- |
| [Reference project](tests/q-003-reference-v1/src/Forge.DocumentIntake.Reference.csproj) | `FC05E8B0A79435FCA5D5A73D3CB43646FE1C2747E580EF1142F22A76D74076AA` |
| [Reference implementation](tests/q-003-reference-v1/src/IntakeDomain.cs) | `B568D4113F1C8939AAE8893DB024DAA96DA52745281B752E39C2AA4CDCAC8CEF` |
| [Baseline test project](tests/q-003-reference-v1/baseline-tests/Forge.DocumentIntake.BaselineTests.csproj) | `5F3E2C4C47170ACE5DC21CC2AA9C994B8618863BBD9A616B6712FF040DF1EC2F` |
| [Baseline tests](tests/q-003-reference-v1/baseline-tests/Program.cs) | `5B4A035C8D24D7097EE25FA13F09A1C4963A90B1D5A984A0EFA8CD43AB0583BB` |

#### API Adjudication

This reference preserves the domain-contract intent and resolves its claim-token ambiguity explicitly for Q-003:

- `IntakeRequest.Create(string idempotencyKey, Uri blobReference, IReadOnlyDictionary<string, string> metadata)` validates the client key, normalizes an HTTP/HTTPS URI, copies metadata immutably, and exposes a SHA-256 fingerprint as an immutable hexadecimal string.
- `IntakeRecord.CreateQueued(IntakeRequest request, string identifier, string correlationId, long concurrencyToken)` creates the initial state.
- `TryClaim(string workerAttemptId, DateTime nowUtc, TimeSpan leaseDuration, long expectedConcurrencyToken, long newConcurrencyToken)` requires a distinct caller-supplied new token, permits queued or expired-processing records only, and returns `TransitionResult`.
- `TryComplete` and `TryFail` require matching worker/token plus a distinct new token, clear the lease, and return a transitioned immutable record.

The reference and baseline tests are frozen. A modification requires a new reference version, new hashes, a stated contract reason, and a new baseline run; it must not silently change Q-003 comparability.

#### Baseline Test Result

Command: `dotnet run --project Forge.DocumentIntake.BaselineTests.csproj`  
Observed: 4 passes, 0 failures.

The frozen baseline proves request key validation, URI/metadata fingerprint behavior, claim-to-completion behavior, and expired-lease reclamation. It is a reference sanity suite, not the future model-generated test suite.

### Frozen Seeded Defects — `q-003-reference-v1`

Each mutant changes one contract-relevant behavior in the frozen human reference. All four compile on .NET SDK 10.0.400 with 0 errors and 0 warnings. Future generated tests must pass against the human reference and fail against each applicable mutant.

| ID | Frozen variant | Intended defect | Contract signal a generated test must detect | SHA-256 |
| --- | --- | --- | --- | --- |
| MUT-001 | [no idempotency validation](tests/q-003-reference-v1/mutants/MUT-001-no-idempotency-validation.cs) | Blank client idempotency keys are accepted. | `IntakeRequest.Create` rejects a blank key. | `9EACD0A01F0CFF0E59D490E37E820CB8D3F5676DBCA7AE0EAF5BC11E3379F651` |
| MUT-002 | [query omitted from fingerprint](tests/q-003-reference-v1/mutants/MUT-002-query-omitted-from-fingerprint.cs) | The URI query is absent from the fingerprint input. | Different normalized query values produce different fingerprints. | `937A1BFA9FC3ED078AE28090CD9C928D9CCB2BD9677A597AFA4ABF1DA237C383` |
| MUT-003 | [claim token not rotated](tests/q-003-reference-v1/mutants/MUT-003-claim-token-not-rotated.cs) | A claim may reuse the expected concurrency token. | Claim rejects a new token equal to the expected token. | `3C862CA2F967869D8B81B04FA5308C6E892DCD10616D6174E6553FD06FCBC83D` |
| MUT-004 | [active lease reclaim](tests/q-003-reference-v1/mutants/MUT-004-active-lease-reclaim.cs) | An active, unexpired processing lease is reclaimable. | A second worker cannot claim an active lease. | `F20A1B44FCDA719385CF9FA2F8FD0403DA52A663EBC76A2716F3FE06235F8369` |

The mutation set is frozen. A changed mutant, new mutant, changed assertion, or changed expected detection rule requires a new `q-003-reference-vN` version and does not alter Q-003 results already recorded.

### Frozen Q-003 Test-Generation Prompt

> **Prompt ID:** `azure-csharp-contract-test-generation-v1`  
> **Output target:** one raw `Program.cs` file; BCL-only executable test harness.

```text
You are writing contract tests for a human-reviewed C# domain library. Produce one complete raw Program.cs file only. Do not use Markdown fences, explanation, external packages, test frameworks, I/O, network access, asynchronous code, reflection, or a Program class.

The referenced library namespace is Forge.DocumentIntake. Its public API is:
- IntakeState: Queued, Processing, Completed, Failed, DeadLettered.
- IntakeDecision: Accepted, Conflict, NotClaimable, Claimed, Completed, Failed.
- IntakeRequest.Create(string idempotencyKey, Uri blobReference, IReadOnlyDictionary<string, string> metadata).
- IntakeRequest has IdempotencyKey, BlobReference, Metadata, and string Fingerprint.
- IntakeRecord.CreateQueued(IntakeRequest request, string identifier, string correlationId, long concurrencyToken).
- IntakeRecord has State, WorkerAttemptId, LeaseExpiryUtc, and ConcurrencyToken.
- TryClaim(string workerAttemptId, DateTime nowUtc, TimeSpan leaseDuration, long expectedConcurrencyToken, long newConcurrencyToken) returns TransitionResult.
- TryComplete(string workerAttemptId, long expectedConcurrencyToken, long newConcurrencyToken) returns TransitionResult.
- TransitionResult has IntakeDecision Decision and nullable IntakeRecord Record.

Write a small BCL-only console test harness that prints PASS or FAIL per check, returns process exit code 0 only when all checks pass, and otherwise returns 1. Use fixed UTC DateTime values; do not depend on wall-clock time.

Required checks:
1. Blank idempotency key is rejected with ArgumentException.
2. Equivalent normalized URI plus metadata in different insertion order produces the same fingerprint.
3. Different URI query values produce different fingerprints.
4. A queued record claims successfully with the expected token and a distinct new token.
5. Reusing the same token as the new claim token is rejected with ArgumentException.
6. An active unexpired Processing lease cannot be claimed by a second worker.
7. An expired Processing lease can be reclaimed by a second worker with a new token.
8. A matching worker/token completes a record and the completed record has no lease.

Use clear C# with BCL namespaces only. Output only the raw C# source.
```

### Q-003 Operator Capture Rule

Save standard output directly to `evidence/q-003-ornith-generated-tests-raw.cs` with `--think=false`, `--nowordwrap`, and no `--verbose`. Do not open or edit the raw file before the agent records its SHA-256 and runs `dotnet build`. The generated test project will reference only `q-003-reference-v1`.

### Q-003 Compile Evidence and Closure

The generated test project is retained at `build/q-003-ornith-generated-tests/OrnithQ003Tests.csproj`. It references the raw generated test file and the frozen human reference only. The literal build reported 7 errors: opening fence characters at line 1 and closing fence characters at line 199.

No test execution, mutation score, or semantic review was performed. This is intentional: the output did not pass the `dotnet build` gate. Q-003 is closed as a failed autonomous test-generation attempt. A future format-only derivation would require explicit operator authorization and a new, separately documented unit; it cannot alter this result.

## Q-003R — User-Approved Fence-Only Derivation

> **Status:** Authorized; derivation and execution in progress  
> **Relationship to Q-003:** Q-003 remains closed as **FAIL**. Q-003R is a separate, human-authorized format-only measurement.

### Scope and Invariants

- Preserve the frozen Q-003 raw output unchanged.
- Remove only the outer opening and closing Markdown fence lines.
- Make no C# code change, correction, formatting rewrite, or prompt/model rerun.
- Record raw and derived SHA-256 hashes.
- Compile the derived test artifact first.
- Only if compilation succeeds, execute it against the frozen correct reference and each of MUT-001 through MUT-004.
- Report reference pass rate and mutant detection separately. Do not reinterpret, replace, or overwrite the autonomous Q-003 verdict.

### Q-003R Evidence

| Criterion | Observed result |
| --- | --- |
| Raw artifact SHA-256 | `38E63DFE41F13A5D8AD7CC7E62882C0C5123F8FFFE1A33F809C60BA748DD01A6` |
| Derived artifact SHA-256 | `AF789B9B1871CA49E7E98B94357D5D36C01DCB0D1791F1A7536CB8EA38F977BE` |
| Derived artifact compilation | **Fail** — after fence-only removal, `dotnet run --project Q003R.Tests.csproj` reports 23 unresolved-name errors for `IntakeRequest`, `IntakeRecord`, `IntakeState`, and `IntakeDecision`. |
| Correct-reference execution | N/R — not run; derived test artifact did not compile. |
| MUT-001 detected | N/R — not measured. |
| MUT-002 detected | N/R — not measured. |
| MUT-003 detected | N/R — not measured. |
| MUT-004 detected | N/R — not measured. |
| Reference pass rate | N/R — no executable test suite. |
| Mutant detection rate | N/R — no executable test suite. |
| Q-003R interpretation | **Fail at compile gate.** Fence removal revealed an independent test-source defect; the generated artifact cannot reference the frozen library successfully. |

### Q-003R Closure

The raw Q-003 artifact remains preserved and Q-003 remains closed **FAIL**. Q-003R removed only the outer fences as authorized; it did not add a missing namespace import or otherwise change C# source. The derived suite failed to compile, so execution against the correct reference and all four mutants was correctly skipped. No reference pass rate or mutant-detection rate exists for Q-003R.

## Q-004 — C# Implementation and Test Review

> **Status:** Review fixture frozen; prompt preparation pending  
> **Question:** *Can Ornith identify material defects in a provided C# implementation and its tests without modifying them?*

### Independent Capability Boundary

Q-004 measures review accuracy, not architecture design, implementation generation, repair, or test generation. Q-001 through Q-003R remain closed with their existing verdicts. A useful Q-004 result would show that the model can assist a human reviewer even if it cannot autonomously produce an acceptable implementation or test suite.

The model must not write replacement code, modify the submitted artifacts, or receive the known defect list. It must inspect the supplied contract, implementation, and tests; identify only traceable issues; assign severity; and recommend validation or remediation at a high level.

### Frozen Review Fixture — `q-004-review-fixture-v1`

| Fixture input | Frozen source |
| --- | --- |
| Contract | `../../05-quality-evaluation/azure-csharp-domain-contract-v1.md` |
| Implementation | `build/q-002r-ornith/OrnithQ002H.minimal-repair.cs` — SHA-256 `77481C8B49924408A87EC28417A588A9E50589B1FFA86B17D59E2754E98B097A` |
| Tests | `tests/q-002h-contract-tests-v1/Program.cs` — SHA-256 `8D95F91EFEAD7C699126893CBCDC9F03E4C0010B5B7E891901C174AB748119C2` |
| Test project | `tests/q-002h-contract-tests-v1/ContractTests.csproj` — SHA-256 `A99D8D1751862DB8636EB1AD0FF4334B4BF0E59A8314AD82A56A901ED1374B2A` |
| Observed fixture behavior | Compiles after Q-002H minimal repair; frozen tests report 4 failures of 8. |

This fixture is selected because it is compilable, its tests are frozen, and its contract defects are already independently evidenced. It is not a new implementation sample and it does not change the Q-002H verdict.

### Hidden Scoring Baseline

The human reviewer will score whether the model identifies the four known material defects: missing client idempotency-key validation, query omission from fingerprint input, exposed mutable fingerprint bytes, and no distinct token on claim. A reported issue is credited only when traceable to the fixture and contract. False material findings count against review reliability.

### Q-004 Entry Gates

1. Submit the exact frozen contract, implementation, and test source together.
2. Use a review-only prompt with an explicit no-modification boundary.
3. Preserve raw reviewer output before assessment.
4. Score correct findings, omissions, severity, traceability, and false positives against the hidden baseline.
5. Do not execute any generated code or accept any proposed repair as part of Q-004.

### Q-004 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Frozen fixture submitted unchanged | Pass — contract, Q-002H implementation, and frozen test source were hash-verified before prompt composition. |
| Raw reviewer output | Pass — [q-004-ornith-review-raw.txt](evidence/q-004-ornith-review-raw.txt), SHA-256 `D5927F733B9FD5E712ECF7A77576760B2C343BF55D274F17A7175EAE5688AB27`, 9,660 bytes. |
| Review-only constraint obeyed | **Fail** — response did not use the four required labeled sections and included replacement C# code despite an explicit no-modification rule. |
| Known material defects identified | **0 of 4.** It did not identify missing client idempotency-key validation, query omission from fingerprint input, mutable fingerprint bytes, or lack of distinct claim-token rotation. |
| Severity and traceability | **Fail** — the sole asserted material defect is not traceable to the supplied contract. The contract defines `Failed` as a terminal state and requires `TryFail` to return `Failed` with no lease. |
| False-positive material findings | **1 material false positive.** It claims `TryFail` should preserve/extend a lease for retry, contradicting the contract; its proposed code also invents unavailable `nowUtc` and `leaseDuration` variables. |
| Q-004 verdict | **Fail — unreliable reviewer for this fixture.** |

### Q-004 Review Scoring

| Review measure | Result |
| --- | --- |
| True material findings | 0 / 4 |
| Material defects omitted | 4 / 4 |
| Material false positives | 1 |
| Required response format | Fail |
| Review-only boundary | Fail |

### Q-004 Closure

The model focused on an invented failure/retry semantics instead of the submitted domain contract and test evidence. A `Failed` state is explicitly terminal in the frozen contract; preserving a processing lease after transition to `Failed` is therefore not a valid required repair. The proposed replacement also cannot compile in the submitted method because `nowUtc` and `leaseDuration` are not parameters or fields there.

Q-004 is closed **FAIL**. This result is independent of Q-001 through Q-003R and does not change their verdicts. It provides no evidence that the evaluated Ornith profile can be relied on for autonomous code-review sign-off.

### Frozen Q-004 Review Prompt

> **Prompt ID:** `azure-csharp-contract-review-v1`  
> **Mode:** Read-only review; the three fixture artifacts are appended verbatim only after their frozen hashes are verified.

```text
You are a senior C#/.NET code reviewer. Review the supplied domain contract, implementation, and test harness. You are evaluating existing artifacts, not designing or implementing a replacement.

Rules:
- Do not modify, rewrite, or output replacement C# code.
- Do not assume facts not present in the submitted artifacts.
- Do not use external tools, citations, Azure SDK knowledge, or external services.
- Judge the artifacts only against the supplied contract.
- Distinguish observed defects from suggested validation.

Return exactly these four labeled sections:
1. Material findings
2. Test-suite assessment
3. Additional validation needed
4. Review verdict

For every finding, state: severity (Critical, Material, or Minor), the precise contract requirement, the relevant artifact/symbol, and why it is a defect. Do not report style preferences as material defects. If the implementation or tests already provide evidence, cite that evidence in your explanation. Do not use tables or code fences.

The artifacts follow.

=== DOMAIN CONTRACT ===
{CONTRACT}

=== IMPLEMENTATION ===
{IMPLEMENTATION}

=== TEST HARNESS ===
{TESTS}
```

### Q-004 Operator Capture Rule

Before invocation, verify the frozen SHA-256 values in the fixture table. Build the prompt by appending the exact file contents to the frozen template, then save Ornith’s standard output directly to `evidence/q-004-ornith-review-raw.txt` using `--think=false`, `--nowordwrap`, and no `--verbose`. The reviewer must receive no hidden scoring baseline, repair history, or prior model output.

## Recording Rules

- Record only observed values and reviewer findings; use `N/R` where evidence is absent.
- Preserve the full unedited model output as a dated evidence file before reviewing it.
- Keep model output, human review, compilation/test output, and any corrective pass separate.
- A coherent response, token rate, or absence of visible thinking is not quality acceptance evidence.
- Do not alter a frozen prompt, the model alias, or the context setting mid-sequence. If one changes, open a new evaluation run.

## Related Records

- Earlier CC-001 history: `quality-evaluation/quality-evaluation-register.md` (not reused as evidence in this register)
- Challenge scope: `README.md`
- FORGE quality contract: `../../05-quality-evaluation/forge-quality-contract-v1-software-architecture-coding.md`
