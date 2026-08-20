# POC Validation & Test Plan: Project FORGE — Local AI Compute & Hardware Lab

| ID | Stage | Test | Evidence | Exit Condition |
| --- | --- | --- | --- | --- |
| T-001 | Entry | Capture read-only inventory for Machine A | PowerShell output | CPU/RAM/GPU/runtime facts recorded |
| T-002 | Baseline | Run approved Ollama workload | Complete result row | SC-001 through SC-003 met |
| T-003 | Comparison | Repeat only approved next backend/model tier | Complete comparable rows | Difference and caveats recorded |
| T-004 | Decision | Consolidate evidence | Results document | Go/Pivot/Stop recorded |

## Evidence Rules

Evidence must not include PHI, corporate data, secrets, credentials, or private source. A failed run remains evidence only when its configuration and failure reason are captured.
