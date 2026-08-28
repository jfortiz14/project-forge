# CC-003B Ornith / ik_llama.cpp — Quality Evaluation Summary

> **Model profile:** CC-003B selected 196K Machine A profile in explicit no-thinking mode  
> **Scope:** Azure/C# planning, implementation, contract-test generation, and read-only code review  
> **Status:** Q-001 through Q-004 recorded; Q-002F testable but contract fail  
> **Source of truth:** [full quality register](quality-evaluation-register-v1.md)

## Executive Result

| Capability measured | Outcome | Practical interpretation |
| --- | --- | --- |
| Autonomous Azure/C# planning | **FAIL** | Q-001 and the sole Q-001C corrective pass exceeded the fixed word range; material idempotency, handoff, and effect-safety gaps remained. |
| Autonomous C# implementation | **FAIL** | Literal output contained Markdown fences and did not compile. |
| Fence-only implementation derivation | **TESTABLE, CONTRACT FAIL** | The immutable derivative compiled through the short path. Corrected tests v2 passed 7/8; claim-token rotation failed. |
| Autonomous C# test generation | **FAIL** | Literal output contained Markdown fences; Q-003F reached a missing-closing-brace error. Q-003H then compiled after one human brace but failed 2 checks against the frozen reference. |
| Autonomous C# code review | **FAIL** | Q-004 followed the review format and found 3 of 4 material defects, but omitted the URI-query fingerprint defect and made a contrary technical conclusion about it. |

**Decision to date:** The evaluated no-thinking profile is not approved for autonomous planning, C# implementation, or contract-test generation. Q-004 remains an independent review-capability measurement.

## Evaluation Map

```text
Q-001  Planning -------------------------- FAIL (624 words)
  `- Q-001C corrective pass ------------- FAIL (632 words; pass consumed)

Q-002  Independent implementation -------- FAIL (7 literal fence errors)
  `- Q-002F fence-only derivation -------- TESTABLE → 7/8 contract tests pass

Q-003  Independent test generation ------- FAIL (literal Markdown fences)
  `- Q-003F fence-only derivation -------- FAIL (CS1513: } expected)
      `- Q-003H minimal human repair ----- TESTABLE → 6/8 reference checks pass

Q-004  Code/test review ------------------ FAIL (3/4 material defects; URI-query defect omitted)
```

## Evidence at a Glance

| Unit | Raw output / intervention | Build or review result | Final status |
| --- | --- | --- | --- |
| Q-001 | Planning output `F97D…AF3F6` | 624 words | Fail |
| Q-001C | Sole corrective planning output `35AB…EA65` | 632 words; material gaps | Fail |
| Q-002 | Raw C# `DA31…69E9` | 7 fence errors | Fail |
| Q-002F | Exact outer-fence removal `5530…C699` | Valid short-path build; v2 harness 7 pass / 1 fail | Testable, contract fail |
| Q-003 | Raw test C# `C517…8622` | Fence errors at lines 1/232 | Fail |
| Q-003F | Exact outer-fence removal `0EFF…4A48` | `CS1513` at line 230 | Fail |
| Q-003H | One closing-brace repair `ACA6…4BE9` | Build passed; reference execution 6 pass / 2 fail | Testable, reference-contract fail |
| Q-004 | Review output `1FAB…521A` | Required format/read-only boundary pass; 3/4 material defects found | Fail |

## Material Findings

| Area | Evidence-backed finding |
| --- | --- |
| Output discipline | Q-002 and Q-003 both violated raw-source-only instructions by emitting Markdown fences. |
| Planning | Both planning passes exceeded the frozen word maximum. Q-001C still omitted reliable same-key conflict/admission reconciliation and crash-safe external-effect behavior. |
| Implementation | The autonomous implementation was never eligible for tests. Q-002F's physical-path attempt was invalidated by infrastructure; the unchanged derivative then compiled through the short path and passed 7/8 contract tests. |
| Test generation | The format-only derivative reached a source-level missing-brace error. One-brace Q-003H repair made it executable, but it failed two checks against the frozen reference; no mutant score exists. |
| Infrastructure | Windows `MAX_PATH` blocked physical-path project invocation. A temporary short mapped drive allowed a known-valid harness and corrected reference project to build. |
| Code review | Q-004 correctly identified missing request idempotency-key validation, mutable fingerprint bytes, and claim-token reuse. It omitted query omission from fingerprint construction and incorrectly asserted that `LocalPath` includes the query. |

## Methodology

```text
Frozen prompt and configuration
    ↓
Immutable raw evidence + SHA-256
    ↓
Literal compile gate
    ↓ (only if compilation succeeds)
Correct-reference tests and seeded-defect measurement
    ↓ (only after test evidence)
Human / contract review
```

## What Was Frozen

| Asset | Record |
| --- | --- |
| Prompts and hashes | [prompts](prompts/README.md) |
| Runtime profile and evidence contract | [execution manifest](execution-manifest.md) |
| Source contracts and fixtures | [frozen inputs](frozen-inputs.md) |
| Chronological operational history | [execution events](execution-events.md) |
| Raw output, hashes, builds, reviews | [evidence](evidence/) |
| Prior working draft | [historical draft summary](quality-evaluation-register-summary.old.md) |

## Current Next Decision

Q-003H is closed **testable but reference-contract fail**. Do not repair the generated tests without a new explicitly authorized human-repair experiment; no mutant run is permitted. The autonomous Q-003 outcome remains closed fail.
