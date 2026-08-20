# POC Execution Readiness Plan: Project FORGE — Local AI Compute & Hardware Lab

## Critical Entry Conditions

| ID | Condition | Owner | Status |
| --- | --- | --- | --- |
| EC-001 | Use only approved non-sensitive test content | Lab operator | Complete — enforced per run |
| EC-002 | Verify Machine A hardware/runtime inventory | Lab operator | Complete |
| EC-003 | Approve common prompt and result record format | Chief Architect + operator | Complete — Benchmark Contract v1 approved |

## Work Packages

| ID | Work Package | Depends On | Acceptance Evidence |
| --- | --- | --- | --- |
| WP-01 | Hardware validation | None | Machine A inventory |
| WP-02 | Ollama baseline | WP-01 | Complete baseline row |
| WP-03 | Optional additional backend/model | WP-02 | Recorded reason and comparable row |
| WP-04 | Consolidation | WP-02 / WP-03 | Results and decision |

**Readiness decision:** Machine A baseline is operational; future work follows the benchmark contract.
