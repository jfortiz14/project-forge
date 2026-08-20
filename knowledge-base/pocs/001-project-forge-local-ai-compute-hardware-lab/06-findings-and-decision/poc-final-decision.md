# POC Final Phase 1 Decision: Project FORGE — Local AI Compute & Hardware Lab

> **Status:** Final Phase 1 decision
> **Date:** 2026-08-15
> **Scope:** Machine A — personal desktop

## Decision

Continue the POC with Machine A as the single local-inference baseline. Do **not** approve or reject a discrete 24/32 GB GPU purchase from this evidence alone.

## What the Evidence Supports

- The desktop RTX 3070 is a practical current host for Qwen3 8B: 68.75 tok/s through Ollama and 69.7 tok/s through llama.cpp/CUDA, with all 37 layers resident on the GPU in llama.cpp.
- Qwen3 14B fits only partially on the RTX 3070 (observed 37% CPU / 63% GPU) and is usable but slow at roughly 7 tok/s generation.
- Qwen3 32B is capacity-feasible with RAM offload but not interactive: 71% CPU / 29% GPU and 2.01 tok/s generation.
- The synthetic application-development workload confirms that local 8B is useful for planning and drafting, but human review and executable validation remain mandatory.

## What the Evidence Does Not Support

- It does not predict the performance, price, availability, drivers, runtime support, power requirements, or model-specific generation rate of any Intel, NVIDIA, or AMD candidate GPU.
- It does not show that a desktop upgrade to 64 GB RAM makes 32B interactive with the existing 8 GB RTX 3070.
- It does not determine the API-versus-local total-cost boundary, because workload frequency, privacy requirements, model-quality needs, and API usage costs have not been quantified.
- It does not establish a 70B path; that tier remains deferred.

## Next Decision Gate

Before evaluating a GPU purchase, quantify one representative development workload and its monthly use: local/privacy requirement, acceptable response latency, preferred model tier, and comparable API spend. Then compare candidate 24/32 GB discrete GPUs only with verified current Windows runtime support and repeat the 8B/32B benchmark contract on the selected hardware.

## Evidence References

- [`results-matrix.md`](../04-performance-evidence/results-matrix.md) — benchmark rows A-001 through A-012, X-001 through X-003, and desktop telemetry.
- [`application-development-workload-v1.md`](../05-quality-evaluation/application-development-workload-v1.md) — staged application-development quality evidence.
- [`benchmark-contract-v1.md`](../03-benchmark-method/benchmark-contract-v1.md) — reproducibility boundary and timing definitions.
