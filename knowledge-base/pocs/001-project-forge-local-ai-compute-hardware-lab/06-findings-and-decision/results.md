# POC Results: Project FORGE — Local AI Compute & Hardware Lab

> **Status:** Completed — Machine A evaluation closed
> **Scope:** Machine A — personal desktop

## Evidence Summary

Machine A evidence establishes the RTX 3070 desktop as the current local-development baseline. Qwen3 8B at 4,096 context generated at 68.75 tok/s through Ollama with complete GPU placement; the operator rated it **usable–excellent**. The corresponding llama.cpp/CUDA run placed all 37 layers, output layer, and 4K KV cache on the RTX 3070 and generated at 69.7 tok/s.

Qwen3 14B is capacity-feasible but uses mixed CPU/GPU placement (observed 37% CPU / 63% GPU) and generates at roughly 7–9 tok/s. It is usable for deliberate drafting with the `no-thinking` profile, but slow for an interactive development loop. A controlled 3,200-token fixture measured 1,018.93 prompt-evaluation tok/s and 8.74 generation tok/s at 4,096 context; the 8,192 configuration was directionally slower and shifted additional placement to CPU.

Qwen3 32B completed at 4,096 context but required 55.333 seconds for cold load and generated at 2.01 tok/s with 71% CPU / 29% GPU placement. It is not practical for interactive use on the current desktop. A 64 GB RAM upgrade may improve offload capacity, but this evidence does not show it would make 32B interactive with the 8 GB RTX 3070.

The staged synthetic application-development workload found the desktop 8B model useful for planning and drafting, but implementation had material semantic defects, tests were non-executable and contract-inconsistent, and review was incomplete. Local models remain suitable for assisted drafting with human review and executable validation, not autonomous code/test/review sign-off.

## Decision

**Final Phase 1 decision — retain Machine A as the POC baseline; do not approve a GPU purchase.**

The evidence supports Qwen3 8B or Llama 3.1 8B on the RTX 3070 for interactive local assistance. A future 24/32 GB discrete GPU should be evaluated only if frequent private/local work with 20–32B models is a concrete requirement, and only after the Windows runtime support and practical generation rate of an Intel, NVIDIA, or AMD candidate are measured using this benchmark contract. The present evidence does not establish a purchase case for any GPU candidate versus APIs.

See [`results-matrix.md`](../04-performance-evidence/results-matrix.md) for complete measurements and [`poc-final-decision.md`](poc-final-decision.md) for the evidence boundary.
