# CC-002 Qwen — Quality Evaluation Register v1

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Challenge:** `CC-002-qwen`  
> **Evaluation type:** Quality only; isolated re-evaluation  
> **Status:** Q-001 through Q-004 recorded; no autonomous acceptance  
> **Data class:** Synthetic/public only
> **Quick view:** [quality-evaluation-register-summary.md](quality-evaluation-register-summary.md)

## Purpose and Boundary

Evaluate `forge-qwen3-35B-A3B-ctx4096-nothink:latest` as an assisted drafting model for the FORGE Azure/C# document-intake workload. This register is independent from the earlier CC-002 performance records so that its evidence, review findings, and conclusion remain traceable.

This is **not** a performance comparison, hardware recommendation, or autonomous-approval test. Timings may be retained as run diagnostics only and must not determine a quality verdict. Do not include corporate code, credentials, customer data, PHI, secrets, or proprietary architecture details.

## Frozen Execution Configuration

| Field | Value |
| --- | --- |
| Model alias | `forge-qwen3-35B-A3B-ctx4096-nothink:latest` |
| Source model | `qwen3.5:35B-A3B` |
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

**Prompt ID:** `azure-csharp-quality-planning-v1` (frozen for this challenge)

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
| Model/profile verified | N/R — the submitted output does not include `ollama show`, `ollama ps`, or an equivalent profile capture. |
| Exact prompt executed | N/R — the output structure and 302 prompt-evaluation tokens are consistent with Q-001, but the invocation itself was not retained. |
| Output captured | Pass — official raw capture retained at [q-001-qwen-official-raw.txt](evidence/q-001-qwen-official-raw.txt). The source attachment SHA-256 was `CA27FC1B642A44B427DD4ECC761328618B7336CBD7CF44CBE2A3725B72341998`. |
| Format and constraint adherence | **Fail** — it has the six required labeled sections and no prohibited code, table, citation, or tool output, but contains 666 words, exceeding the 450–600-word limit. |
| Requirement fidelity | **Fail** — it does not define durable intake-time idempotency behavior for duplicate same-key requests versus conflicting same-key/different-request submissions; it instead describes worker-side create-or-update behavior. |
| Technical correctness and Azure operability | **Fail** — it claims exactly-once execution despite retry/redelivery, mixes "managed identity exclusively" with blob SAS references, probes an unselected Azure SQL dependency, and selects the deprecated Azure Storage Emulator. |
| Critical findings | None observed. |
| Material findings | 4 — output-size violation; incomplete idempotency contract; unsupported exactly-once/identity handling; incomplete and partly obsolete operational test/readiness design. |
| Minor findings | 2 — hosting and worker runtime are left as alternatives rather than a decision; the DLQ timer lacks authorization/replay criteria. |
| Quality verdict | **Fail — no autonomous acceptance. Q-001C was the permitted corrective pass; its separate result is recorded below.** |

### Q-001 Run Diagnostics

| Metric | Observed value |
| --- | --- |
| Total duration | `1m46.8450191s` |
| Load duration | `1m14.9346561s` |
| Prompt evaluation | 302 tokens in `1.716976s` (`175.89 tokens/s`) |
| Generation | 836 tokens in `30.176548s` (`27.70 tokens/s`) |

### Q-001 Review Rationale

The official Q-001 output meets the requested section count and addresses the principal components: asynchronous API, Service Bus, Table Storage, managed identity, observability, retries, dead-lettering, and tests. Its quality gate nevertheless fails.

The response exceeds the frozen word limit by 66 words. More importantly, it claims "exactly-once execution semantics" from a Table Storage create-or-update pattern while Service Bus delivery and retry behavior require an idempotent consumer and explicit settlement; duplicate detection is a send-side control, not a replacement for consumer idempotency. The proposed design does not state the client idempotency key, conditional insert before enqueue, same-key/same-fingerprint response, or same-key/different-fingerprint conflict needed to make the API outcome unambiguous.

The identity story is internally inconsistent: it says the system relies exclusively on managed identities, but the API accepts blob SAS references and later treats SAS refresh as a security control. It also uses one shared system-assigned identity while granting cross-component permissions, which weakens the stated least-privilege boundary. The health check names Azure SQL even though no Azure SQL component was selected; it should instead verify the dependencies actually used, including Service Bus and Table Storage, under an explicit timeout policy. The test plan omits explicit coverage for conflict, duplicate delivery, concurrent claim, expired lease, non-retryable failure, DLQ handoff, authorization failure, cancellation, and correlation propagation. Finally, Azure Storage Emulator is deprecated; current local storage testing should use Azurite or a controlled Azure integration environment.

The observed timing is retained solely as run diagnostics and is not part of the quality verdict.

### Q-001 Disposition

One corrective planning pass is permitted by this register if explicitly requested. It must preserve the frozen workload, address every material finding, and obey the 450–600-word limit. No implementation unit will run until that pass is reviewed or a human reference design is selected.

## Q-001C — Corrective Azure/C# Planning (Single Allowed Pass)

> **Status:** Recorded; failed quality gate; planning closed  
> **Precondition:** Q-001 failed. This is the only corrective planning pass for this model/configuration in this register.  
> **Reference basis:** Prompt reproduced verbatim from CC-001 Q-001C for cross-challenge comparability; only the evaluated model differs.

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
| Exact frozen prompt executed | N/R — the output follows the six corrective sections and reports 434 prompt-evaluation tokens, but the CLI invocation was not retained. |
| Model/profile verified | N/R — no profile/alias capture was submitted with this output. |
| Output captured | Pass — raw capture retained at [q-001c-qwen-corrective-raw.txt](evidence/q-001c-qwen-corrective-raw.txt). The source attachment SHA-256 was `F3968EA3C08B1C32929D3FF3839CED67BE7EA1A271F04150E5E5D910CEE44799`. |
| 450–600-word constraint | **Fail** — 724 words observed. |
| Corrective requirements addressed | **Fail** — it adds a client token, ETags/lease idea, roles, allowed-scope validation, and authorized DLQ replay, but leaves the admission, effects, and role rules materially incomplete or inconsistent. |
| Critical findings | None observed. |
| Material findings | 5 — word-range violation; incomplete create-or-return/conflict behavior; no crash-safe rule for duplicate external side effects; incorrect/incomplete least-privilege role design with SAS ambiguity; obsolete/incomplete integration test strategy. |
| Minor findings | 2 — runtime hosting remains undecided; `Pending` is introduced although the reference state model uses `Queued`. |
| Quality verdict | **Fail — Q-001 planning closed.** The single corrective pass is consumed. |

### Q-001C Run Diagnostics

| Metric | Observed value |
| --- | --- |
| Total duration | `1m47.3499397s` |
| Load duration | `1m12.5164228s` |
| Prompt evaluation | 434 tokens in `1.863555s` (`232.89 tokens/s`) |
| Generation | 912 tokens in `32.953555999s` (`27.68 tokens/s`) |

### Q-001C Disposition

Q-001C failed and the Q-001 planning capability is closed for this model/configuration. The output exceeds the fixed range by 124 words. Its labels are present, but it omits Markdown heading markers; this is recorded as a non-blocking format variance because the required labels remain unambiguous.

The response does not define an actual atomic create-or-return admission decision. It says "optimistic update" and returns an existing result only for `Completed` or `Processing`, omitting an already accepted/enqueued record and the required same-key/different-request conflict. It also does not close the durable-acceptance-to-enqueue failure gap. A conditional intake record can remain accepted if enqueue fails, preventing a retry from safely causing the required delivery; a transactional outbox or explicit reconciliation rule was needed.

Exclusive leases prevent simultaneous workers, but they do not alone prevent a duplicate external side effect after a worker acts, crashes before recording completion, and a later worker reclaims the expired lease. The response states the goal but does not define an external-effect idempotency key, transactional boundary, or other crash-safe effect rule. Its role plan is also incomplete: the API requires access to Table Storage for admission but receives no Table role, and `Data Owner` is not a precise Table Storage role. The simultaneous claims of managed-identity-only access and use of SAS tokens remain inconsistent.

The test plan again names the deprecated Azure Storage Emulator and does not explicitly test same-key/different-request conflict, enqueue recovery, duplicate queue delivery, concurrent claim, authorization failure, cancellation, or correlation propagation. The timing values remain diagnostic only and do not influence this verdict.

No further planning regeneration is permitted for this model/configuration. A later Q-002 may proceed only as an independent human-intervened unit using the frozen human reference architecture and domain contract; it must exclude Q-001/Q-001C output from both prompt and review.

## Q-002 — Human-Intervened C# Domain Implementation

> **Status:** Recorded; failed at literal compile gate  
> **Scope:** Independent implementation-capability measurement. Q-001 and Q-001C remain closed as **FAIL** and are not input evidence for this unit.  
> **Reference basis:** Q-002 prompt and procedure reproduced from CC-001 for cross-challenge comparability; only the evaluated model and evidence identifiers differ.

### Human Intervention and Source Boundary

The permitted human intervention is the selection of the frozen, reviewed source material below. No generated Qwen planning output is used to design or judge Q-002.

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

1. The operator saves the entire unedited terminal output to `evidence/q-002-qwen-raw-output.txt` during execution.
2. The agent preserves that file unchanged. It is the source evidence, including any non-source text or timing diagnostics.
3. The agent copies the generated artifact exactly as returned into a build workspace and runs `dotnet build`. **No semantic code evaluation occurs before this compile gate.**
4. Only a successful build advances to contractual tests. The human/contract review occurs after the relevant test evidence is available. A build failure is recorded with its evidence and stops the test stage; a code fence or explanation is a format finding, but compilation is still attempted.

### Q-002 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Frozen prompt/model configuration | N/R — the output reports 516 prompt-evaluation tokens, but the CLI invocation and profile capture were not retained. |
| Raw output preserved before evaluation | Pass — [q-002-qwen-raw-output.txt](evidence/q-002-qwen-raw-output.txt), source SHA-256 `41A4FD9695E8C25B5E17AA705C1AB5E28D66C8AF4C8E6AF4CFDF182FC7BD2094`, 18,417 bytes. It was copied unchanged before compilation. |
| Output format | **Fail** — it begins with a Markdown `csharp` fence and contains explanatory deliberation despite the raw-source-only constraint. |
| Literal `dotnet build` | **Fail** — `dotnet build QwenQ002.csproj --nologo` on the byte-identical raw output reports 326 errors. The first errors are the opening backticks; later errors show non-source explanatory text and an unfinished implementation. |
| Transport-normalized diagnostic build | N/R — not performed. Q-002R is the separately frozen pipeline-conformant re-run; no transformation is applied to Q-002. |
| Required types and factories | Not accepted — no successful compile gate. |
| Fingerprint normalization and metadata-order invariance | Not accepted — no successful compile gate. |
| Claim, token, lease, and terminal transition semantics | Not accepted — no successful compile gate. |
| Invalid-input handling and dependency boundary | Not accepted — no successful compile gate. |
| Critical findings | None observed. |
| Material findings | 3 — raw-source-only output boundary violated; candidate fails the required literal compilation; output contains explanatory text and ends before completing the source artifact. |
| Q-002 verdict | **Fail — closed at compile gate.** |

### Q-002 Closure

Q-002 is closed at the literal compile gate. No contractual tests or semantic code review were run. The raw evidence remains authoritative and no source transformation or repair was applied.

### Q-002 Run Diagnostics

| Metric | Observed value |
| --- | --- |
| Total duration | `3m21.2801959s` |
| Load duration | `1m6.3108776s` |
| Prompt evaluation | 516 tokens in `2.083668999s` (`247.64 tokens/s`) |
| Generation | 3,580 tokens in `2m12.874048s` (`26.94 tokens/s`) |

## Q-002R — Pipeline-Conformant Implementation Re-run

> **Status:** Recorded; failed at literal compile gate  
> **Relationship to Q-002:** A separate reproducibility run, not a corrective pass and not a replacement for the closed Q-002 result.

### Reason and Scope

Q-002 remains **FAIL**. Q-002R measures the same independent implementation capability with a pipeline-conformant raw-source capture:

`Model output -> save raw .cs -> dotnet build -> contractual tests -> human/contract review`

The model alias, context, thinking mode, human-reviewed source boundary, and frozen prompt text are unchanged from Q-002. Q-002R changes only the capture method: omit `--verbose`, write standard output directly to a `.cs` file, and use `--nowordwrap`. Timings are intentionally out of scope for this quality-only re-run.

### Execution and Evidence Gate

1. Save the command output directly to `evidence/q-002r-qwen-raw.cs`; do not open, edit, format, or inspect it before the build.
2. Record its SHA-256 hash.
3. Compile that exact file with `dotnet build`.
4. Run contractual tests only if the build succeeds.
5. Perform human/contract review only after the applicable build/test evidence exists.

### Q-002R Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Frozen prompt/model configuration | Partial — same Q-002 prompt/model was requested with `--think=false --nowordwrap --keepalive=0` and without `--verbose`; the raw file contains no verbose timing diagnostics, although the command invocation itself was not retained. |
| Raw `.cs` preserved and SHA-256 recorded | Pass — [q-002r-qwen-raw.cs](evidence/q-002r-qwen-raw.cs), SHA-256 `1D7D9DC768EF1DB5A655B2E410CBFCBB80AE4D0F01E0B668A1BB0F7ED904F860`, 12,454 bytes. No content was changed. |
| Capture-procedure caveat | The agent verified the hash before compilation but also counted lines accidentally. This was read-only and did not alter the file, but it means the capture cannot be described as entirely uninspected before build. |
| Literal `dotnet build` result | **Fail** — `dotnet build QwenQ002R.csproj --nologo` reported 6 errors: the opening Markdown fence at line 1 and a missing closing brace at line 154, where the generated output ends inside `TryClaim`. |
| Contractual test result | N/R — not run; the compile gate failed. |
| Human/contract review | N/R — not performed; the pipeline prohibits semantic review before successful compilation and relevant contractual tests. |
| Q-002R verdict | **Fail — closed at compile gate.** The raw-source-only boundary was not met; no source transformation or corrective edit was applied. |

### Q-002R Closure

Q-002R is closed at the literal compile gate. Q-002 and Q-002R remain separate failed implementation measurements; Q-002R does not revise the closed Q-002 result. The raw source is retained unchanged for evidence only.

## Q-002R2 — Pipeline-Conformant Reproducibility Capture

> **Status:** Recorded; failed at literal compile gate  
> **Relationship to Q-002R:** Separate reproducibility capture authorized after the operator reported that Q-002R may have been cancelled before completion. Q-002R remains preserved and closed as **FAIL**.

Q-002R2 preserves the Q-002 frozen prompt, model alias, context, thinking mode, human-reviewed source boundary, and output constraints. It changes only the evidence filename and must not be described as a correction or replacement for Q-002/Q-002R.

### Execution and Evidence Gate

1. Invoke the same frozen Q-002 prompt with `--think=false --nowordwrap --keepalive=0` and without `--verbose`.
2. Redirect standard output directly to `evidence/q-002r2-qwen-raw.cs`.
3. Do not open, edit, format, or inspect the raw file before its SHA-256 is recorded and the literal compile gate runs.
4. Compile that exact file with `dotnet build`.
5. Run contractual tests only if compilation succeeds; perform review only after applicable build/test evidence exists.

### Q-002R2 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Frozen prompt/model configuration | Partial — same Q-002 prompt/model was requested with `--think=false --nowordwrap --keepalive=0` and without `--verbose`; the command invocation itself was not retained. |
| Raw `.cs` preserved and SHA-256 recorded | Pass — [q-002r2-qwen-raw.cs](evidence/q-002r2-qwen-raw.cs), SHA-256 `A44D5CDFAAA62A3E72C77D4B7BD7FAE96BDFC7B4F0DB2831BEEF49E140A4BD41`, 32,742 bytes. The hash was verified before compilation without opening or changing the file. |
| Literal `dotnet build` result | **Fail** — `dotnet build QwenQ002R2.csproj --nologo` reported 6 errors: the opening Markdown fence at line 1 and unexpected trailing text at line 324. |
| Contractual test result | N/R — not run; the compile gate failed. |
| Human/contract review | N/R — not performed; the pipeline prohibits semantic review before successful compilation and relevant contractual tests. |
| Q-002R2 verdict | **Fail — closed at compile gate.** The raw-source-only boundary was not met; no source transformation or corrective edit was applied. |

### Q-002R2 Disposition

Q-002R2 is closed at the literal compile gate. It remains a separate reproducibility measurement and cannot alter the closed Q-002 or Q-002R verdicts. No source transformation, contractual tests, or semantic review was applied.

## Q-002R2F — Authorized Fence-Only Derivation Preflight

> **Status:** Not started — authorized transformation precondition not met  
> **Relationship to Q-002R2:** An operator authorized removal of the two outer Markdown fence lines only. Q-002R2 remains immutable and closed as **FAIL**.

### Preflight Result

The raw evidence [q-002r2-qwen-raw.cs](evidence/q-002r2-qwen-raw.cs) has SHA-256 `A44D5CDFAAA62A3E72C77D4B7BD7FAE96BDFC7B4F0DB2831BEEF49E140A4BD41`. Its first line is an opening ` ```csharp ` fence, but no matching closing fence exists at the end of the file; the file ends in incomplete explanatory comment text.

No derived file was created and no R2F compilation was run. Removing only the opening fence would not satisfy the authorized two-outer-fences transformation and would not be comparable to CC-001 Q-002R fence removal.

### Q-002R2F Disposition

Q-002H cannot begin from Q-002R2 under the CC-001 method because no fence-only baseline exists. Any further source transformation would require a new explicit authorization and would be a different human-intervention experiment, not Q-002R2F.

## Q-003 — Independent Contract-Test Generation

> **Status:** Recorded; failed at compile gate  
> **Purpose:** Measure whether Qwen can generate useful C# contract tests independently of its failed autonomous implementation capability.

### Why This Is a Separate Unit

Q-002, Q-002R, and Q-002R2 measure autonomous implementation and all closed at compilation gates. None is a suitable target for test generation. Q-003 therefore uses only a human-reviewed, compilable reference implementation with a frozen public API.

### Frozen Q-003 Pipeline

`Human reference implementation -> baseline contract tests pass -> model test output -> save raw .cs -> dotnet build -> execute generated tests -> seeded-defect detection -> human/contract review`

The model output is evaluated as a test artifact. It must compile against the frozen reference, pass against the correct reference, and fail when targeted seeded defects are introduced.

### Entry Gates

1. **Pass** — human reference `q-003-reference-v1` copied unchanged from CC-001 and hash-verified.
2. **Pass** — baseline command `dotnet run --project Forge.DocumentIntake.BaselineTests.csproj` observed 4 passes and 0 failures on .NET SDK 10.0.400.
3. **Pass** — four frozen seeded-defect variants are retained with the reference.
4. **Pass** — Q-003 prompt defines the exact API and raw `Program.cs` boundary below.
5. **Pass** — no Q-002/Q-002R/Q-002R2 generated source is supplied as the implementation target.

### Frozen Human Reference — `q-003-reference-v1`

The reference is human-authored and copied unchanged from CC-001 solely for cross-challenge comparability. It is not derived from any Qwen-generated implementation output.

| Artifact | SHA-256 |
| --- | --- |
| [Reference project](../tests/q-003-reference-v1/src/Forge.DocumentIntake.Reference.csproj) | `FC05E8B0A79435FCA5D5A73D3CB43646FE1C2747E580EF1142F22A76D74076AA` |
| [Reference implementation](../tests/q-003-reference-v1/src/IntakeDomain.cs) | `B568D4113F1C8939AAE8893DB024DAA96DA52745281B752E39C2AA4CDCAC8CEF` |
| [Baseline test project](../tests/q-003-reference-v1/baseline-tests/Forge.DocumentIntake.BaselineTests.csproj) | `5F3E2C4C47170ACE5DC21CC2AA9C994B8618863BBD9A616B6712FF040DF1EC2F` |
| [Baseline tests](../tests/q-003-reference-v1/baseline-tests/Program.cs) | `5B4A035C8D24D7097EE25FA13F09A1C4963A90B1D5A984A0EFA8CD43AB0583BB` |

The adjudicated API uses `IntakeRequest.Create(string idempotencyKey, Uri blobReference, IReadOnlyDictionary<string, string> metadata)`, `IntakeRecord.CreateQueued(IntakeRequest request, string identifier, string correlationId, long concurrencyToken)`, and a five-parameter `TryClaim` that includes a distinct caller-supplied new concurrency token.

### Frozen Seeded Defects — `q-003-reference-v1`

| ID | Intended defect | Required detection signal | SHA-256 |
| --- | --- | --- | -
| MUT-001 | Blank client idempotency keys accepted. | `IntakeRequest.Create` rejects a blank key. | `9EACD0A01F0CFF0E59D490E37E820CB8D3F5676DBCA7AE0EAF5BC11E3379F651` |
| MUT-002 | URI query omitted from fingerprint input. | Different normalized query values produce different fingerprints. | `937A1BFA9FC3ED078AE28090CD9C928D9CCB2BD9677A597AFA4ABF1DA237C383` |
| MUT-003 | Claim token not rotated. | Claim rejects a new token equal to the expected token. | `3C862CA2F967869D8B81B04FA5308C6E892DCD10616D6174E6553FD06FCBC83D` |
| MUT-004 | Active lease is reclaimable. | A second worker cannot claim an active lease. | `F20A1B44FCDA719385CF9FA2F8FD0403DA52A663EBC76A2716F3FE06235F8369` |

### Frozen Q-003 Test-Generation Prompt

**Prompt ID:** `azure-csharp-contract-test-generation-v1`  
**Output target:** one raw `Program.cs` file; BCL-only executable test harness.

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

Save standard output directly to `evidence/q-003-qwen-generated-tests-raw.cs` with `--think=false`, `--nowordwrap`, and no `--verbose`. Do not open or edit the raw file before the agent records its SHA-256 and runs `dotnet build`. The generated test project will reference only `q-003-reference-v1`.

### Q-003 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Human reference source/API frozen | Pass — `q-003-reference-v1`; hashes recorded above. |
| Baseline tests pass | Pass — 4 of 4 checks pass on .NET SDK 10.0.400. |
| Seeded-defect variants frozen | Pass — four variants retained with expected detection signals above. |
| Model test prompt/output captured | Pass — [q-003-qwen-generated-tests-raw.cs](evidence/q-003-qwen-generated-tests-raw.cs), SHA-256 `763EDED835F14CF3AB0D8889789DDE621119F3DAF4C90D1EB8D8BBE28F709900`, 20,538 bytes. The hash was verified before compilation without opening or changing the file. |
| Generated tests build and run | **Fail at build gate** — `dotnet run --project QwenQ003Tests.csproj` failed compilation against the frozen reference. The candidate lacks BCL namespace imports for types such as `Uri`, `Dictionary`, `Console`, `DateTime`, and `TimeSpan`, and declares a local `_` more than once. No executable test harness was produced. |
| Seeded defects detected | N/R — not measured; generated tests did not compile. |
| Human/contract review | N/R — not performed; the pipeline requires successful build and execution before review. |
| Q-003 verdict | **Fail — closed at compile gate.** |

### Q-003 Closure

The raw test artifact was compiled unchanged against `q-003-reference-v1`. Its failure is not a terminal-presentation artifact: the compiler reports missing required BCL namespace imports and a duplicate local declaration. No raw-source transformation, execution against the correct reference, mutation measurement, or semantic review was applied.

## Q-004 — C# Implementation and Test Review

> **Status:** Recorded; failed review-quality gate  
> **Question:** *Can Qwen identify material defects in a provided C# implementation and its tests without modifying them?*

### Independent Capability Boundary

Q-004 measures review accuracy, not architecture design, implementation generation, repair, or test generation. Q-001 through Q-003 remain closed with their existing verdicts. The model must not write replacement code, modify the submitted artifacts, or receive the known defect list.

### Frozen Review Fixture — `q-004-review-fixture-v2`

This is a new fixture version because the current CC-001 files did not match its historic v1 hashes. The v2 implementation/tests are frozen in CC-002 together with a local project binding needed to execute the existing tests.

| Fixture input | SHA-256 |
| --- | --- |
| [Domain contract](../../05-quality-evaluation/azure-csharp-domain-contract-v1.md) | `81DEC1163E9A16E7F3ACAF28AB2BE35A8B0F9624241B7FCEE82FFB4A0BA9F6DB` |
| [Implementation](fixtures/q-004-review-fixture-v2/implementation/OrnithQ002H.minimal-repair.cs) | `2380F6A745567B848F17DE90B6AFB548786811E79D9D6F5E8841BC9FCB70B3F9` |
| [Implementation project](fixtures/q-004-review-fixture-v2/implementation/Forge.DocumentIntake.Fixture.csproj) | `FCFDC71065451BA8940895F1D44451D9BB31D6E5ABF13F5F95E630198968C221` |
| [Tests](fixtures/q-004-review-fixture-v2/tests/Program.cs) | `81417ECFE0C62A8C8CC971E02F93BE8FD4CD79B0AA8D7A91C06AAAA4435F8384` |
| [Test project](fixtures/q-004-review-fixture-v2/tests/ContractTests.csproj) | `1E8DE50C44A22A558B18557E76D0773F3B9F504E806A12D9DCED167D740C1AF4` |
| Observed fixture behavior | Compiles with 3 `CS8625` warnings; frozen tests report 4 failures of 8. |

### Hidden Scoring Baseline

The human reviewer will score whether the model identifies these four known material defects: missing client idempotency-key validation, query omission from fingerprint input, exposed mutable fingerprint bytes, and no distinct token on claim. A reported issue is credited only when traceable to the fixture and contract. False material findings count against review reliability.

### Q-004 Entry Gates

1. Submit the exact frozen contract, implementation, and test source together.
2. Use the review-only prompt below with an explicit no-modification boundary.
3. Preserve raw reviewer output before assessment.
4. Score correct findings, omissions, severity, traceability, and false positives against the hidden baseline.
5. Do not execute any generated code or accept any proposed repair as part of Q-004.

### Frozen Q-004 Review Prompt

**Prompt ID:** `azure-csharp-contract-review-v1`  
**Mode:** Read-only review; append the exact fixture artifacts only after verifying the hashes above.

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

Before invocation, verify the frozen SHA-256 values in the fixture table. Build the prompt by appending the exact file contents to the frozen template, then save standard output directly to `evidence/q-004-qwen-review-raw.txt` using `--think=false`, `--nowordwrap`, and no `--verbose`. The reviewer must receive no hidden scoring baseline, repair history, or prior model output.

### Q-004 Evidence and Review

| Criterion | Observed result |
| --- | --- |
| Frozen fixture submitted unchanged | Pass — the contract, implementation, and test source hashes were verified before prompt composition. |
| Raw reviewer output | Pass — [q-004-qwen-review-raw.txt](evidence/q-004-qwen-review-raw.txt), SHA-256 `74387249B6CA7632A208FBE8C5787B039BA59EDFC9E0E4AA94E59E7CF7672DCB`, 18,770 bytes. |
| Review-only constraint obeyed | **Fail** — response omits all four required labeled sections and provides a "Complete Solution" with replacement C# code, directly violating the no-modification boundary. |
| Known material defects identified | **4 of 4.** It identifies the absent client idempotency-key parameter, query omission from fingerprint input, mutable fingerprint bytes, and lack of a distinct claim concurrency token. |
| Severity and traceability | **Fail** — it supplies no required severity labels or contract/artifact citations. It begins from the false premise that the submitted `IntakeRecord` is partial and then reasons toward a replacement rather than reviewing the supplied artifact. |
| False-positive material findings | **1 untraceable material premise.** It characterizes the submitted implementation as partial/missing despite the complete fixture source supplied to it. |
| Q-004 verdict | **Fail — non-compliant reviewer for this fixture.** |

### Q-004 Review Scoring

| Review measure | Result |
| --- | --- |
| True material findings | 4 / 4 |
| Material defects omitted | 0 / 4 |
| Material false-positive premises | 1 |
| Required response format | Fail |
| Review-only boundary | Fail |

### Q-004 Closure

The model recognized every hidden material defect, which is useful evidence of defect-recognition capability for this fixture. However, it did not perform the task it was asked to perform: it did not use the prescribed review structure, did not assign severity or trace findings to the submitted contract/artifacts, and generated replacement code after an explicit prohibition. The invented claim that the submitted `IntakeRecord` was partial further undermines the reliability of its reasoning boundary.

Q-004 is closed **FAIL** under the quality contract. The result does not change Q-001 through Q-003 verdicts and does not provide evidence for autonomous code-review sign-off.

## Recording Rules

- Record only observed values and reviewer findings; use `N/R` where evidence is absent.
- Preserve the full unedited model output as a dated evidence file before reviewing it.
- Keep model output, human review, compilation/test output, and any corrective pass separate.
- A coherent response, token rate, or absence of visible thinking is not quality acceptance evidence.
- Do not alter a frozen prompt, the model alias, or the context setting mid-sequence. If one changes, open a new evaluation run.

## Related Records

- Earlier CC-002 history: `quality-evaluation/results.md` and `quality-evaluation/README.md`
- Challenge scope: `../README.md`
- FORGE quality contract: `../../05-quality-evaluation/forge-quality-contract-v1-software-architecture-coding.md`
