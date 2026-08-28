# CC-003B Quality Evaluation Register v1 — Historical Draft Snapshot

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab
> **Challenge:** `FORGE-CC-003B`
> **Evaluation type:** Quality only; isolated from completed capacity/performance evidence
> **Status:** Prepared; Q-001 through Q-004 not started
> **Data class:** Synthetic/public only
> **Quick view:** [quality-evaluation-register-summary.md](quality-evaluation-register-summary.md)

## Purpose and Boundary

Evaluate the selected CC-003B profile as an assisted drafting model for the FORGE Azure/C# document-intake workload. Timings are diagnostic only and do not affect quality verdicts.

The frozen capacity/performance parameters are recorded in [README.md](README.md). The quality run uses explicit no-thinking controls, described in [operator-runbook.md](operator-runbook.md), so it can be compared with CC-002's quality procedure. This is a separate invocation profile and its quality verdict must not be generalized to reasoning-enabled mode.

## Evaluation Sequence and Gates

1. **Q-001 Planning:** frozen planning prompt; assess design quality and output constraints.
2. **Q-001C Corrective planning:** allowed once only if Q-001 fails and the operator authorizes it.
3. **Q-002 Implementation:** independent human-intervened unit using frozen human architecture and domain contract. It does not use Q-001 output.
4. **Q-003 Test generation:** frozen human reference, baseline tests, and mutants.
5. **Q-004 Code/test review:** frozen contract, implementation, and tests with a hidden human scoring baseline.

An autonomous unit passes only when it meets the quality contract with no critical or material findings and all required format/evidence gates. Failure does not prohibit later independent units whose entry gates are satisfied.

## Q-001 — Azure/C# Planning

**Prompt ID:** `azure-csharp-quality-planning-v1`

**Frozen file:** [`prompts/q-001-azure-csharp-quality-planning-v1.txt`](prompts/q-001-azure-csharp-quality-planning-v1.txt)  
**Frozen file SHA-256:** `7640A997DFB79A093C8FFC43B23C54B8E8BFE085EEC5CDAC6E04A48BDA22B4DA`

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

**Evidence target:** `evidence/q-001-ornith-raw.txt` plus SHA-256 and review record.

## Q-001C — Single Corrective Planning Pass

**Status:** Executed once; failed; planning capability closed for this configuration.  
**Frozen file:** [`prompts/q-001c-azure-csharp-quality-planning-corrective-v1.txt`](prompts/q-001c-azure-csharp-quality-planning-corrective-v1.txt)

This was the only corrective planning pass. It preserved the workload and six-section/450–600-word constraints, but failed its output-format and corrective-requirement gates; see [Q-001C contract review](evidence/q-001c-review.md). No further planning regeneration is permitted in this quality run.

## Q-002 — Independent C# Domain Implementation

**Prompt ID:** `azure-csharp-domain-implementation-human-baseline-v1`

**Frozen file:** [`prompts/q-002-azure-csharp-domain-implementation-human-baseline-v1.txt`](prompts/q-002-azure-csharp-domain-implementation-human-baseline-v1.txt)

Its human intervention is limited to the read-only sources in [frozen-inputs.md](frozen-inputs.md); do not provide Q-001 output to the model.

**Entry gate:** Confirm the source hashes and `.NET` SDK version. Preserve the raw response directly as `evidence/q-002-ornith-raw.cs`, hash it, and compile its literal content through [`build/q-002-ornith/OrnithQ002.csproj`](build/q-002-ornith/OrnithQ002.csproj). Do not run contractual tests or semantic review if it fails to build.

**Outcome:** **Fail — literal compile gate.** The immutable raw source contained opening and closing Markdown fences, producing 7 compiler errors. No source transformation, contractual test, or semantic review was performed. See [Q-002 literal compile gate](evidence/q-002-literal-build.md).

### Q-002F — Authorized Fence-Only Derivation

**Status:** Prepared; operator-authorized; not yet executed.  
**Purpose:** Measure whether removal of only exact outer Markdown fence lines changes the compilability of the otherwise immutable Q-002 output. This is a human-intervened derivative, not a retry or correction of Q-002.

The permitted transformation, preconditions, and distinct evidence identity are frozen in [Q-002F fence-only manifest](evidence/q-002f-fence-only-manifest.md). A failed derived build closes Q-002F; no additional transformation is allowed without a new explicit authorization.

## Q-003 — Independent Contract-Test Generation

**Prompt ID:** `azure-csharp-contract-test-generation-v1`

**Frozen file:** [`prompts/q-003-azure-csharp-contract-test-generation-v1.txt`](prompts/q-003-azure-csharp-contract-test-generation-v1.txt)

Use the canonical reference/mutants in [frozen-inputs.md](frozen-inputs.md). Preserve raw `Program.cs` output as `evidence/q-003-ornith-raw.cs`; hash, compile, and run it through [`build/q-003-ornith/OrnithQ003Tests.csproj`](build/q-003-ornith/OrnithQ003Tests.csproj) against the correct reference before mutant detection.

**Entry-gate observation:** Pass — the four baseline checks passed with zero failures, and MUT-001 through MUT-004 each compiled successfully.

**Raw capture:** `evidence/q-003-ornith-raw.cs`, SHA-256 `C5176E1C80F429F9EE6D397CB4C9E1C3EA526989FCA5F1D987F15F7DAB448622`; HTTP 200 with `truncated=false`. The artifact remains unreviewed and untransformed.

**Outcome:** **Fail — raw-source compile gate.** Opening and closing Markdown fences produced 8 compiler errors. The initial build also exposed an incorrect relative project-reference path in the Q-003 harness; it was corrected after closure without rerunning or changing the raw artifact. No reference execution, mutant measurement, transformation, or semantic review was performed. See [Q-003 literal build](evidence/q-003-literal-build.md).

### Q-003F — Authorized Fence-Only Derivation

**Status:** Prepared; operator-authorized; not yet executed.  
**Purpose:** Measure whether removal of only exact outer Markdown fences makes the immutable Q-003 test harness build and execute against the frozen human reference. It is a human-intervened format-only measurement, not a retry or correction of Q-003.

The scope, source identity, corrected-reference preflight, and gate sequence are frozen in the [Q-003F manifest](evidence/q-003f-fence-only-manifest.md).

## Q-004 — C# Implementation and Test Review

**Prompt ID:** `azure-csharp-contract-review-v1`

**Frozen template:** [`prompts/q-004-azure-csharp-contract-review-v1.txt`](prompts/q-004-azure-csharp-contract-review-v1.txt)

Before prompt construction, verify the read-only fixture hashes in [frozen-inputs.md](frozen-inputs.md), then append only the domain contract, implementation, and test harness. Do not send the hidden scoring baseline, prior model output, repair history, or known defects.

Preserve raw review output as `evidence/q-004-ornith-review-raw.txt`. Score format, read-only compliance, severity, contract/artifact traceability, true findings, omissions, and false material findings only after capture.

## Recording Rules

- Preserve full unedited model content before review or transformation.
- Hash raw output and every fixture input used for a unit.
- Keep raw model output, transport diagnostics, build/test logs, review, corrective pass, and human repair separate.
- Record `N/R` rather than an inference.
- Any changed prompt, source hash, server configuration, context, cache type, batch, or reasoning mode creates a new quality run version.
