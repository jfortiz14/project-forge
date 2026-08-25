# CC-002 Qwen — Quality Evaluation Summary

> **Model profile:** `forge-qwen3-35B-A3B-ctx4096-nothink:latest`  
> **Scope:** Quality of Azure/C# planning, implementation, test-generation, and code-review outputs.  
> **Data:** Synthetic/public only.  
> **Source of truth:** [full quality register](quality-evaluation-register-v1.md)

## Executive Result

| Capability measured | Outcome | Practical interpretation |
| --- | --- | --- |
| Autonomous Azure/C# planning | **FAIL** | Both planning captures exceeded the fixed length and retained material gaps in idempotency, identity, and operational design. |
| Autonomous C# implementation | **FAIL** | Literal output plus two separate pipeline-conformant captures did not pass the required compile gate. |
| Minimal human repair | **N/R** | No eligible two-fence baseline existed; no human repair was invented or applied. |
| Autonomous C# test generation | **FAIL** | Generated tests did not compile against the frozen human reference; no execution or mutant-detection evidence exists. |
| Autonomous C# code review | **FAIL** | The reviewer recognized 4 known defects but violated the read-only boundary, output format, and traceability requirements. |

**Decision:** Qwen is not approved for autonomous architecture, C# implementation, contract-test generation, or code review in this workload. It may be used only as a drafting or defect-hypothesis aid with human ownership and the full compile/test/review pipeline.

## Evaluation Map

```text
Q-001  Planning -------------------------- FAIL
  `- Q-001C corrective pass ------------- FAIL (single allowed pass consumed)

Q-002  Independent implementation -------- FAIL (326 literal build errors)
  `- Q-002R pipeline capture re-run ------ FAIL (fence and incomplete source)
  `- Q-002R2 reproducibility capture ----- FAIL (fence and trailing text)
      `- Q-002R2F fence-only preflight --- NOT STARTED (no closing outer fence)

Q-003  Independent test generation ------- FAIL (BCL imports absent; duplicate local)

Q-004  Code and test review -------------- FAIL (4/4 true findings, but format/read-only failure)
```

## Evidence at a Glance

| Unit | Raw output / intervention | Build result | Tests | Final status |
| --- | --- | --- | --- | --- |
| Q-001 | Planning prompt | N/A | N/A | Fail |
| Q-001C | One corrective planning pass | N/A | N/A | Fail |
| Q-002 | Human-reviewed architecture + domain contract; model implementation | 326 errors in literal output | Not run | Fail |
| Q-002R | Same implementation prompt, raw `.cs` capture | 6 errors: opening fence and missing brace | Not run | Fail |
| Q-002R2 | Same implementation prompt, reproducibility capture | 6 errors: opening fence and trailing text | Not run | Fail |
| Q-002R2F | Authorized two-fence derivation preflight | Not run; closing fence absent | Not run | Not started |
| Q-003 | Model-generated BCL test harness | Missing BCL imports and duplicate local declaration | Not run | Fail |
| Q-004 | Frozen contract + fixture v2 implementation + frozen tests | N/A | Review score: 4/4 true findings; format/read-only fail | Fail |

## Material Findings

| Area | Evidence-backed finding |
| --- | --- |
| Output discipline | Qwen emitted Markdown fences or non-source/trailing text in every measured implementation capture, preventing literal compilation. |
| Planning | Both captures exceeded the fixed word range and did not provide an acceptable durable idempotency/admission and side-effect safety design. |
| Implementation | No raw implementation capture met the compile gate. The R2F preflight could not create a comparable fence-only derivative because R2 ended without a closing outer fence. |
| Test generation | The generated test harness omitted required BCL namespace imports and redeclared a local variable, so it could not execute against the human reference or mutants. |
| Code review | Although it recognized all four hidden material defects, it invented a partial-implementation premise, omitted the mandated structure/traceability, and generated replacement code against an explicit read-only instruction. |

## Methodology Improvements Established

```text
Model output
    ↓
Save immutable raw .cs evidence + SHA-256
    ↓
dotnet build
    ↓ (only if build passes)
Contractual tests
    ↓ (only after test evidence)
Human / contract review
```

- No semantic code review occurs before the compile gate.
- Failed autonomous units remain failed; re-runs are separate reproducibility measurements and do not overwrite earlier verdicts.
- Human repair is measured separately and requires an explicit, eligible baseline; no repair is inferred from a truncated artifact.
- Human references, test suites, seeded defects, fixtures, prompts, and evidence are frozen for reproducibility.

## What Was Frozen

| Asset | Location |
| --- | --- |
| Full register, prompts, decisions, hashes | [quality-evaluation-register-v1.md](quality-evaluation-register-v1.md) |
| Q-003 human reference and baseline | [q-003-reference-v1](../tests/q-003-reference-v1) |
| Q-003 seeded defects | [q-003-reference-v1/mutants](../tests/q-003-reference-v1/mutants) |
| Q-004 review fixture v2 | [q-004-review-fixture-v2](fixtures/q-004-review-fixture-v2) |
| Raw model outputs | [evidence](evidence) |

## Current Next Decision

Do not grant this profile autonomous approval for the evaluated Azure/C# tasks. Any future quality run should use a new, separately frozen model/configuration and this compile-first pipeline. Comparison with other models is now possible because the reference implementation, seeded defects, review fixture, prompts, and evidence rules are frozen.
