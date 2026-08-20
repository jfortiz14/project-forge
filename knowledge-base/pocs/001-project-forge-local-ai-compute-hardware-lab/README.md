# Project FORGE POC — Machine A Local AI Compute Lab

This POC records the evaluation of local LLM inference on Machine A, a personal Windows desktop with an RTX 3070 8 GB. It establishes the practical baseline for local development assistance and informs a later hardware-versus-API decision.

## Reading Order

1. [Charter](01-charter/) — scope, entry conditions, and validation plan.
2. [Environment](02-environment/) — verified desktop, model registry, and preflight evidence.
3. [Benchmark Method](03-benchmark-method/) — workload contract, context fixture, and telemetry collection.
4. [Performance Evidence](04-performance-evidence/) — benchmark matrix, telemetry, and raw run notes.
5. [Quality Evaluation](05-quality-evaluation/) — Azure/C# contracts, model outputs, and quality register.
6. [Findings and Decision](06-findings-and-decision/) — interpreted results and the current decision boundary.

## Current Conclusion

Qwen3 8B and Llama 3.1 8B are practical interactive local assistants on Machine A. Qwen3 14B is usable but slow, and Qwen3 32B is not interactive on the RTX 3070 with 8 GB VRAM. Models remain drafting assistants; human review, compilation, and real tests are mandatory.

For initiative-level artifacts, see the executive [proposal](../../proposals/001-project-forge-local-ai-compute-hardware-lab.md), [ADR](../../adrs/001-project-forge-local-ai-compute-hardware-lab.md), and specialist [reviews](../../reviews/).
