# FORGE Quality Evaluation Protocol v1

## Purpose

Measure local-model capability separately for planning, implementation, test generation, and review. Fluent output, speed, or a successful repair does not establish autonomous capability.

## Mandatory Pipeline

`model output → immutable raw evidence + SHA-256 → dotnet build → contractual tests → human/contract review`

Never evaluate generated code semantically before its build result. If build fails, record it and do not run tests. Human review after a failed build may identify build blockers only; it cannot replace the build gate.

## Units

| Unit | Capability | Entry gate |
| --- | --- | --- |
| Q-001 | Architecture/planning | Frozen prompt and constraints |
| Q-002 | Implementation | Human-reviewed architecture and domain contract |
| Q-003 | Test generation | Frozen compilable human reference, baseline tests, and mutants |
| Q-004 | Code/test review | Frozen contract, implementation, tests, and hidden human scoring baseline |

`R` is a separately recorded format/capture re-run and never changes the original autonomous verdict. `H` is explicitly counted human repair; all outcomes belong to the combined human-plus-model artifact.

## Acceptance

A unit passes only with no critical/material findings and its required format/evidence. For Q-003, tests must compile, pass the correct reference, and detect applicable mutants. For Q-004, findings must be traceable, correctly severe, and avoid material false positives.

## Immutability and Versioning

Hash every raw output, fixture input, prompt, test suite, mutant, and reference source. Never overwrite an evaluated run. Any change creates a new fixture/protocol/run version with rationale and a fresh baseline.
