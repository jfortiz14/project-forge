# POC Plan: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Status:** Completed — Machine A evaluation closed
> **ADR:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Objective

Establish a reproducible, practical evaluation of local LLM inference on Machine A and determine whether current hardware plus APIs is sufficient or a future 24 GB/32 GB GPU warrants further procurement evaluation.

## Hypotheses

1. A staged, same-prompt baseline will reveal material differences between CPU/RAM offload and NVIDIA CUDA execution that throughput alone cannot capture.
2. The evidence may identify a justified high-VRAM upgrade path, but no purchase is presumed.

## Scope

- Machine A hardware inventory validation.
- One runtime baseline before any additional runtime/backend.
- Separate load, prefill, generation, memory, utilization, context, offload, and usability evidence.

## Non-Goals

- Production deployment, serving, fine-tuning, corporate workload testing, PHI/PII processing, or benchmarking to maximize a score.
- Hardware purchase approval.

## Success Criteria

| ID | Criterion |
| --- | --- |
| SC-001 | Each accepted result has all BR-003 fields. |
| SC-002 | The same approved non-sensitive prompt is used where technically possible. |
| SC-003 | Prefill and generation are separately captured. |
| SC-004 | Final result distinguishes practical comfortable capacity from merely loadable capacity. |

## Go / Pivot / Stop

- **Go:** Entry gates pass and baseline data is complete enough to progress one stage at a time.
- **Pivot:** A selected runtime cannot expose required evidence or is disallowed; choose one approved alternative and document why.
- **Stop:** Restricted data is at risk, or system health/safety requires stopping.

## Closure

All Machine A success criteria were met with evidence-backed measurements. The POC establishes Qwen3 8B and Llama 3.1 8B as practical supervised local assistants, identifies Qwen3 14B as usable but slow, and confirms Qwen3 32B is not interactive on the RTX 3070 8 GB. The Azure/C# quality comparison found no evaluated model eligible for autonomous architecture, code, test, or review acceptance.

**Final Phase 1 decision:** retain the current hardware; do not approve a GPU purchase. A GPU-versus-API economic decision is deferred until a representative workload, API usage profile, candidate-hardware compatibility, price, and measured candidate performance are available.

See the [POC findings](001-project-forge-local-ai-compute-hardware-lab/06-findings-and-decision/results.md) and [final decision](001-project-forge-local-ai-compute-hardware-lab/06-findings-and-decision/poc-final-decision.md).
