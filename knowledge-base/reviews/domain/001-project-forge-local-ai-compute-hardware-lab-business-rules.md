# Business Rules: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **ADR Reference:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Rules

| ID | Rule | Type | Owner | Verification |
| --- | --- | --- | --- | --- |
| BR-001 | A run may use only synthetic, public, or personally authored non-sensitive input. | Constraint | Lab operator | Prompt review |
| BR-003 | A result is comparable only when it records model, quantization, machine, backend, context, prompt version, load time, prefill rate, generation rate, RAM, VRAM/shared memory, offload, utilization, and notes. | Invariant | Chief Architect | Result completeness check |
| BR-004 | Prefill and generation rates must be stored separately. | Invariant | Lab operator | Result completeness check |
| BR-005 | No hardware purchase recommendation may be made from unverified inventory or a single model/runtime result. | Decision gate | Chief Architect | ADR review |
| BR-006 | A runtime is introduced one at a time; baseline evidence is captured before another runtime is added. | Workflow | Lab operator | Evidence chronology |
| BR-007 | A usability rating must accompany quantitative measurements. | Invariant | Lab operator | Result completeness check |

## Decision Table

| Inventory verified | Comparable baseline exists | Permitted action |
| --- | --- | --- | --- |
| No | No | Perform read-only inventory only. |
| Yes | No | Run first approved baseline. |
| Yes | Yes | Evaluate next approved backend or model tier. |
