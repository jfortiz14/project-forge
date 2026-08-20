# SLA Matrix: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **ADR Reference:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

| Service Objective | Target | Measurement | Action if Missed |
| --- | --- | --- | --- |
| Result completeness | 100% for accepted rows | BR-003 field check | Mark row incomplete; exclude comparison |
| Prompt safety | 100% approved/non-sensitive | Operator attestation | Stop and do not retain sensitive data |
| Run recoverability | Configuration retained for every failed run | Evidence record | Diagnose before retry |
| Benchmark availability | Best effort only | Not an SLA | Reschedule; no production impact |
| Performance threshold | No pre-set throughput target | Usability + observed data | Inform final trade-off, not pass/fail alone |
