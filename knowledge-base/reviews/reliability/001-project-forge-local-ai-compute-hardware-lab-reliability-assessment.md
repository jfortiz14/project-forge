# Reliability Assessment: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **ADR Reference:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Reliability Scope

This is a bounded laboratory, not a production inference service. The reliability objective is repeatable measurements and safe recovery from failed runs.

| Concern | Risk | Control | Evidence |
| --- | --- | --- | --- |
| Thermal/power state | Desktop performance varies or throttles | Record power mode and observed thermals; bound run duration | Run notes |
| Memory exhaustion | Runtime fails or system becomes unresponsive | Start with one model/runtime; record RAM/VRAM; stop on instability | Runtime logs and notes |
| Driver/runtime variance | Results cannot be compared | Record OS, driver, runtime, backend, and model version | Inventory + result record |
| Partial failure | A failed run is misread as a performance result | Mark failed/aborted explicitly; do not calculate comparison | Evidence matrix |
| Storage pressure | Model files reduce available workstation capacity | Verify free disk before downloads; do not delete user/corporate data | Inventory |

## Findings

- 🟡 Results without power/thermal state are not suitable for comparison.
- 🟢 No uptime SLA is appropriate; run-completion and result completeness are the POC service objectives.
- 🟢 Resume is manual: retain configuration, mark the prior run aborted, and repeat only after cause is recorded.
