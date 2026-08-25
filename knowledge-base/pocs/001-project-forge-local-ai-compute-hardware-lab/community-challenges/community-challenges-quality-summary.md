# Community Challenges — Quality Summary

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Scope:** Cross-challenge view of recorded Azure/C# quality evidence.  
> **Data:** Synthetic/public only.  
> **Rule:** This is a comparison index, not a replacement for each challenge's full register or raw evidence.

## Current Decision

Neither evaluated profile has earned autonomous approval for the FORGE Azure/C# workload. Both may be used only as human-supervised drafting or defect-hypothesis aids, subject to the frozen compile/test/review pipeline.

## Challenge Register

| Challenge | Model profile | Quality sequence | Current decision | Detailed record |
| --- | --- | --- | --- | --- |
| CC-001 Ornith | `forge-ornith-35B-A3B-ctx4096-nothink` | Q-001 through Q-004 recorded | No autonomous approval | [summary](CC-001-ornith/quality-evaluation-register-summary.md) · [register](CC-001-ornith/quality-evaluation-register-v1.md) |
| CC-002 Qwen | `forge-qwen3-35B-A3B-ctx4096-nothink:latest` | Q-001 through Q-004 recorded | No autonomous approval | [summary](CC-002-qwen/quality-evaluation/quality-evaluation-register-summary.md) · [register](CC-002-qwen/quality-evaluation/quality-evaluation-register-v1.md) |

## Capability Comparison

| Capability | CC-001 Ornith | CC-002 Qwen | Comparison note |
| --- | --- | --- | --- |
| Autonomous Azure/C# planning | **Fail** — fixed length and durable idempotency/atomicity requirements not met; corrective pass also failed. | **Fail** — both captures exceeded the word limit and retained material idempotency, identity, and operational gaps. | Neither planning profile met the quality gate. |
| Autonomous C# implementation | **Fail** — literal and pipeline re-run did not compile. | **Fail** — literal output and two reproducibility captures did not compile. | Both repeatedly violated the raw-source compile gate. |
| Minimal human repair | **Testable, contract fail** — fence removal plus one code-line change compiled; 4 of 8 contract checks failed. | **N/R** — no eligible two-fence baseline; no human repair was invented. | This measurement is not directly comparable because Qwen had no authorized repair baseline. |
| Autonomous C# test generation | **Fail** — raw output and fence-only derivation did not compile. | **Fail** — generated harness lacked required BCL imports and redeclared a local. | Neither produced executable generated tests; no mutant-detection score exists. |
| Autonomous C# code review | **Fail** — 0 of 4 known material defects found; 1 material false positive. | **Fail** — 4 of 4 known defects recognized, but read-only boundary, required structure, and traceability were violated. | Qwen showed stronger defect recognition on its fixture, but neither met the review acceptance contract. |

## Evidence Highlights

| Area | CC-001 Ornith | CC-002 Qwen |
| --- | --- | --- |
| Raw-source discipline | Markdown fences caused implementation and test-generation compile failures. | Markdown fences and non-source/trailing text prevented every implementation capture from compiling. |
| Compile-first method | Q-002H was evaluated only after a separately authorized minimal repair; tests then exposed four material defects. | No Q-002H was run because R2 lacked a closing outer fence and no comparable fence-only baseline could be created. |
| Human reference test generation | Human reference and mutants were frozen; generated test output was not executable. | Same human reference and mutants were frozen; generated test output was not executable. |
| Review evidence | Reviewer invented retry/lease semantics and replacement code. | Reviewer generated replacement code and asserted an untrue partial-implementation premise. |

## Comparability Notes

- The planning, implementation, and Q-003 prompt families follow the same frozen FORGE evaluation contracts.
- A failed unit remains failed. Re-runs and derivations are separately named and never overwrite prior evidence.
- CC-002 includes Q-002R2 because the operator reported that Q-002R may have been cancelled; it is an additional reproducibility capture, not a retry that can convert Q-002 to pass.
- CC-001 Q-002H has no CC-002 equivalent because the required fence-only baseline did not exist for Qwen.
- CC-002 Q-004 uses `q-004-review-fixture-v2`, created after the current CC-001 fixture files no longer matched historic v1 hashes. Its four hidden material defects reproduce the same review themes, but direct reviewer-score comparison should be interpreted cautiously.
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
