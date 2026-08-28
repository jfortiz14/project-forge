# Community Challenges — Quality Summary

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Scope:** Cross-challenge view of recorded Azure/C# quality evidence.  
> **Data:** Synthetic/public only.  
> **Rule:** This is a comparison index, not a replacement for each challenge's full register or raw evidence.

> **Program state:** The recorded quality sequence is closed and frozen. No profile earned autonomous approval; see the [community challenge closure](community-challenges-closure.md).

## Current Decision

None of the three evaluated profiles has earned autonomous approval for the FORGE Azure/C# workload. They may be used only as human-supervised drafting or defect-hypothesis aids, subject to the frozen compile/test/review pipeline.

## Challenge Register

| Challenge | Model profile | Quality sequence | Current decision | Detailed record |
| --- | --- | --- | --- | --- |
| CC-001 Ornith | `forge-ornith-35B-A3B-ctx4096-nothink` | Q-001 through Q-004 recorded | No autonomous approval | [summary](CC-001-ornith/quality-evaluation-register-summary.md) · [register](CC-001-ornith/quality-evaluation-register-v1.md) |
| CC-002 Qwen | `forge-qwen3-35B-A3B-ctx4096-nothink:latest` | Q-001 through Q-004 recorded | No autonomous approval | [summary](CC-002-qwen/quality-evaluation/quality-evaluation-register-summary.md) · [register](CC-002-qwen/quality-evaluation/quality-evaluation-register-v1.md) |
| CC-003B Ornith / ik_llama.cpp Q6_K | Ornith 1.5 35B-A3B Q6_K; ik_llama.cpp `0ed847d`; 196,608 context | Q-001 through Q-004 recorded | No autonomous approval; Q-002F testable but contract fail (7/8) | [summary](CC-003B-ornith-ik-llama-q6k-machine-a-optimization/quality-evaluation/quality-evaluation-register-summary.md) · [register](CC-003B-ornith-ik-llama-q6k-machine-a-optimization/quality-evaluation/quality-evaluation-register-v1.md) |

## Capability Comparison

| Capability | CC-001 Ornith | CC-002 Qwen | CC-003B Ornith / ik_llama.cpp Q6_K | Comparison note |
| --- | --- | --- | --- | --- |
| Autonomous Azure/C# planning | **Fail** — fixed length and durable idempotency/atomicity requirements not met; corrective pass also failed. | **Fail** — both captures exceeded the word limit and retained material idempotency, identity, and operational gaps. | **Fail** — Q-001 and its sole corrective Q-001C exceeded the word limit and retained material corrective gaps. | No planning profile met the quality gate. |
| Autonomous C# implementation | **Fail** — literal and pipeline re-run did not compile. | **Fail** — literal output and two reproducibility captures did not compile. | **Fail** — Q-002 literal output contained Markdown fences. The Q-002F format-only derivative passed its valid short-path build and then 7/8 contractual checks. | All completed autonomous implementation units violated the raw-source compile gate. |
| Minimal human repair | **Testable, contract fail** — fence removal plus one code-line change compiled; 4 of 8 contract checks failed. | **N/R** — no eligible two-fence baseline; no human repair was invented. | **Testable, contract fail** — Q-002F is a format-only derivative; only claim-token rotation failed (7/8 pass). | This measurement is not directly comparable because Qwen had no authorized repair baseline. |
| Autonomous C# test generation | **Fail** — raw output and fence-only derivation did not compile. | **Fail** — generated harness lacked required BCL imports and redeclared a local. | **Fail** — Q-003 raw output contained fences; Q-003F reached `CS1513`. One-brace Q-003H repair compiled but failed 2 of 8 checks against the frozen reference. | No profile produced mutant-detection evidence. |
| Autonomous C# code review | **Fail** — 0 of 4 known material defects found; 1 material false positive. | **Fail** — 4 of 4 known defects recognized, but read-only boundary, required structure, and traceability were violated. | **Fail** — 3 of 4 known material defects recognized. The review met format, review-only, severity, and traceability rules, but omitted the URI-query fingerprint defect and asserted it passes. | Qwen showed stronger defect recognition on its fixture, but neither completed review met the acceptance contract. |

## Evidence Highlights

| Area | CC-001 Ornith | CC-002 Qwen | CC-003B Ornith / ik_llama.cpp Q6_K |
| --- | --- | --- | --- |
| Raw-source discipline | Markdown fences caused implementation and test-generation compile failures. | Markdown fences and non-source/trailing text prevented every implementation capture from compiling. | Markdown fences appeared in both Q-002 implementation and Q-003 test-generation raw outputs. |
| Compile-first method | Q-002H was evaluated only after a separately authorized minimal repair; tests then exposed four material defects. | No Q-002H was run because R2 lacked a closing outer fence and no comparable fence-only baseline could be created. | Q-002F preserves the source except for outer-fence removal. A physical long-path build hit `MSB3030`; the valid short-path build then passed and 7/8 contractual checks passed. |
| Human reference test generation | Human reference and mutants were frozen; generated test output was not executable. | Same human reference and mutants were frozen; generated test output was not executable. | The frozen human reference and a dedicated preflight compiled under the short path. Q-003H executed but failed expired-lease reclaim and completion checks; mutants were not run. |
| Review evidence | Reviewer invented retry/lease semantics and replacement code. | Reviewer generated replacement code and asserted an untrue partial-implementation premise. | Reviewer followed the read-only/format boundary and found 3 defects, but reached a false URI-query conclusion. |

## Recorded Compilation Diagnostics

These are literal-build observations from the detailed registers. They distinguish presentation-boundary failures from diagnostics that remained after the permitted fence-only derivation; they are not semantic acceptance reviews.

| Challenge / unit | Observed compiler result | Interpretation boundary |
| --- | --- | --- |
| CC-001 Q-002 literal | 188 errors; the first diagnostics were Markdown backticks and terminal ANSI control characters. | Raw-source boundary failed. |
| CC-001 Q-002 transport-normalized derivative | 63 errors after removing only ANSI sequences and outer fences; examples include malformed/duplicated `Appenbuilder...` and `record record)` source. | Not attributable only to terminal presentation. |
| CC-001 Q-002R literal | 7 errors; opening fence at line 1 and closing fence at line 360. | Pipeline-conformant raw capture still violated the output format. |
| CC-001 Q-002R fence-only derivative | 1 error: `CS1061` at line 202 (`DateTime` has no `IsUniversalTime` member); 3 `CS8625` warnings. | The harness reached a C# compiler diagnostic after fence removal. |
| CC-001 Q-003 literal / fence-only derivative | Literal output had 7 fence-related errors. The derivative then had 23 unresolved-name errors for `IntakeRequest`, `IntakeRecord`, `IntakeState`, and `IntakeDecision`. | The generated test artifact still could not compile against the frozen reference. |
| CC-002 Q-002 literal | 326 errors; opening fences were first, followed by non-source deliberation and an unfinished implementation. | Raw-source boundary and source completeness both failed. |
| CC-002 Q-002R | 6 errors; opening fence and missing closing brace at line 154, ending inside `TryClaim`. | Not a fence-only failure. |
| CC-002 Q-002R2 | 6 errors; opening fence plus unexpected trailing text at line 324. | No eligible outer-closing-fence baseline existed for comparable fence-only derivation. |
| CC-002 Q-003 | Generated harness did not compile: required BCL imports for `Uri`, `Dictionary`, `Console`, `DateTime`, and `TimeSpan` were absent, and a local `_` was redeclared. | The test harness was not executable; no mutant score was measured. |
| CC-003B Q-002 literal | 7 errors: opening/closing Markdown fences at lines 1/298 (`CS1056`) and `CS0116`. | Raw-source boundary failed before semantic compilation. |
| CC-003B Q-002F fence-only derivative | Physical long-path invocation failed with `MSB3030` because the expected DLL was not produced; the valid short-path build then succeeded in 2.0 s with no warnings/errors. | The derived source has a separately recorded SHA-256. It passed 7/8 contractual checks, failing only distinct claim-token rotation; it does not revise Q-002's literal raw-source failure. |
| CC-003B Q-003 literal | 8 errors: opening/closing Markdown fences at lines 1/232, including `CS1056` and `CS1513`; the initial invocation also exposed a corrected relative-reference warning. | Raw-source boundary failed. The later preflight verified the corrected harness/reference independently. |
| CC-003B Q-003F fence-only derivative | `CS1513` (`}` expected) at line 230 after fence-only removal; frozen reference and dedicated preflight compiled successfully via the short path. | Source-level generated-test failure remained after presentation-only normalization. |

No CC-001 or CC-002 quality record reports `MSB3030` as the terminal diagnostic for a generated artifact. CC-003B recorded it only for the Q-002F physical long-path invocation; the same immutable derivative subsequently built successfully through the short path. The CC-003B frozen reference preflight compiled successfully from the short path before Q-003F was evaluated.

## Comparability Notes

- The planning, implementation, and Q-003 prompt families follow the same frozen FORGE evaluation contracts.
- A failed unit remains failed. Re-runs and derivations are separately named and never overwrite prior evidence.
- CC-002 includes Q-002R2 because the operator reported that Q-002R may have been cancelled; it is an additional reproducibility capture, not a retry that can convert Q-002 to pass.
- CC-001 Q-002H has no CC-002 equivalent because the required fence-only baseline did not exist for Qwen.
- CC-002 Q-004 uses `q-004-review-fixture-v2`, created after the current CC-001 fixture files no longer matched historic v1 hashes. Its four hidden material defects reproduce the same review themes, but direct reviewer-score comparison should be interpreted cautiously.
- CC-003B retains the same no-thinking quality methodology but runs the community-reproduction runtime/model configuration after Machine A capacity tuning. Its substantially larger context is an execution boundary, not a change to the frozen quality acceptance criteria.
- The CC-003B Q-002F long-path `MSB3030` condition must not be interpreted as a model-generated source failure. Its successful short-path build established format-only compilability; its corrected frozen contractual suite then failed only claim-token rotation. Q-002 remains the completed autonomous quality result.
- CC-003B Q-004 uses the CC-002 frozen v2 review fixture. It is comparable to CC-002 on defect-recognition themes, but a passing structure/read-only boundary does not offset an omitted material defect.
- This document compares quality evidence only. It does not compare inference speed, hardware efficiency, cost, or production suitability.

## Shared Method

```text
Frozen prompt and configuration
    ↓
Immutable raw evidence + SHA-256
    ↓
Literal compile gate
    ↓ (only if compilation succeeds)
Tests and seeded-defect measurement
    ↓ (only after test evidence)
Human / contract review
```

## Adding a Challenge

For each new community challenge:

1. Add one row to **Challenge Register** with the exact model profile and links to its summary/register.
2. Add observed outcomes to **Capability Comparison**; use `N/R` for units not measured.
3. Add only evidence-backed differences to **Evidence Highlights** and **Comparability Notes**.
4. Do not convert a passing unit into model-wide approval; record the workload and acceptance boundary.

## Current Next Decision

Use this index to select future model candidates for the same frozen workload. Any new model must receive its own challenge ID, frozen configuration, raw evidence, and full quality sequence before it is compared here.
