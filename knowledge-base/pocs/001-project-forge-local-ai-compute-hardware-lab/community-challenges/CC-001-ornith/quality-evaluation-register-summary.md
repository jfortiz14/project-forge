# CC-001 Ornith — Quality Evaluation Summary

> **Model profile:** `forge-ornith-35B-A3B-ctx4096-nothink`  
> **Scope:** Quality of Azure/C# planning, implementation, and test-generation outputs.  
> **Data:** Synthetic/public only.  
> **Source of truth:** [full quality register](quality-evaluation-register-v1.md)

## Executive Result

| Capability measured | Outcome | Practical interpretation |
| --- | --- | --- |
| Autonomous Azure/C# planning | **FAIL** | Useful ideas appeared, but planning did not satisfy the fixed length and idempotency/atomicity requirements. |
| Autonomous C# implementation | **FAIL** | Raw output and the re-run did not pass the required compile gate. |
| Minimal human repair | **Testable, contract FAIL** | One code-line repair after fence removal made the candidate compile, but contractual tests found 4 material defects. |
| Autonomous C# test generation | **FAIL** | Generated test output did not compile; no test or mutant-detection evidence exists. |
| Autonomous C# code review | **FAIL** | The reviewer found none of the four known material defects and made one material false-positive claim. |

**Decision:** Ornith is not approved for autonomous architecture, C# implementation, or contract-test generation in this workload. It can still be used as a drafting aid only with human ownership and the full compile/test/review pipeline.

## Evaluation Map

```text
Q-001  Planning ─────────────── FAIL
  └─ Q-001C corrective pass ─── FAIL (single allowed pass consumed)

Q-002  Independent implementation ───────── FAIL
  └─ Q-002R pipeline capture re-run ─────── FAIL (Markdown fences)
      └─ Q-002H minimal human repair ────── COMPILES → 4/8 contract tests fail

Q-003  Independent test generation ──────── FAIL (Markdown fences)
  └─ Q-003R fence-only derivation ───────── FAIL (23 unresolved references)

Q-004  Code and test review ─────────────── FAIL (0/4 material defects found; 1 false positive)
```

## Evidence at a Glance

| Unit | Raw output / intervention | Build result | Tests | Final status |
| --- | --- | --- | --- | --- |
| Q-001 | Planning prompt | N/A | N/A | Fail |
| Q-001C | One corrective planning pass | N/A | N/A | Fail |
| Q-002 | Human-reviewed architecture + domain contract; model implementation | 188 errors in literal output | Not run | Fail |
| Q-002R | Same implementation prompt, raw `.cs` capture | 7 fence errors | Not run | Fail |
| Q-002H | Remove two outer fences; one human code-line repair | 0 errors, 3 warnings | 4 pass / 4 fail | Testable, contract fail |
| Q-003 | Model-generated BCL test harness | 7 fence errors | Not run | Fail |
| Q-003R | Remove two outer fences only | 23 unresolved-reference errors | Not run | Fail |
| Q-004 | Frozen contract + Q-002H implementation + frozen tests | N/A | Review score: 0/4 true findings; 1 false material finding | Fail |

## Material Findings

| Area | Evidence-backed finding |
| --- | --- |
| Output discipline | Ornith emitted Markdown fences despite raw-C#-only instructions in both implementation and test-generation units. |
| Planning | It did not meet the fixed word range and did not define a sufficient durable idempotency/admission or side-effect safety design. |
| Implementation after minimal repair | Missing client idempotency-key validation; fingerprint omitted URI query information; fingerprint was mutable; claim did not rotate the concurrency token. |
| Test generation | After fence removal, the generated suite still could not resolve the supplied `Forge.DocumentIntake` API types. |
| Code review | It invented retry/lease semantics that contradict the contract, proposed non-compilable replacement code, and omitted all four evidenced material defects. |

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
- Failed autonomous units stay failed; Q-002R/Q-003R are separate capture/format measurements.
- Q-002H measures human-plus-model repair effort, never autonomous model capability.
- Contract tests, human reference, and four seeded defects are frozen for reproducibility.

## What Was Frozen

| Asset | Location |
| --- | --- |
| Full register, prompts, decisions, hashes | [quality-evaluation-register-v1.md](quality-evaluation-register-v1.md) |
| Q-002H contract suite | [tests/q-002h-contract-tests-v1](tests/q-002h-contract-tests-v1) |
| Q-003 human reference and baseline | [tests/q-003-reference-v1](tests/q-003-reference-v1) |
| Q-003 seeded defects | [tests/q-003-reference-v1/mutants](tests/q-003-reference-v1/mutants) |
| Raw model outputs | [evidence](evidence) |

## Current Next Decision

Do not grant this profile autonomous approval for the evaluated Azure/C# tasks. Any future quality run should use a new, separately frozen model/configuration and the established compile-first pipeline. A direct comparison with another model is now possible because the reference implementation, test suite, seeded defects, prompts, and evidence rules are in place.
