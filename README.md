# Project FORGE — Local AI Compute & Hardware Lab

> **Project FORGE is a personal, independent research and experimentation project focused on local AI inference, hardware performance, and model quality.**

> **Researcher:** Francisco Ortiz  
> **Title:** Software Architect

## Purpose

Project FORGE evaluates when a local language model is practical for architecture and software-development assistance, and when an API or future hardware upgrade is the better choice. The current research baseline is **Machine A**, a personal Windows desktop with an NVIDIA RTX 3070 8 GB.

The project measures more than token throughput: model-load time, prefill, generation, RAM/VRAM use, GPU offload, context effects, developer experience, and the quality of generated Azure/C# work.

## Current Findings

| Model on Machine A | Practical result |
| --- | --- |
| Qwen3 8B | Usable–excellent; about 69 tok/s with complete GPU placement. |
| Llama 3.1 8B | Usable–excellent; about 73 tok/s. |
| Ministral 3 8B | Usable; about 51 tok/s with greater VRAM use. |
| Qwen3 14B | Usable but slow; roughly 7 tok/s because of CPU/GPU offload. |
| Qwen3 32B | Capacity-feasible but not interactive; about 2 tok/s. |

Local models are treated as drafting and planning assistants. They are not accepted as autonomous authorities for architecture, implementation, testing, code review, or deployment; human review, compilation, and executable tests remain required.

## Documentation Map

- [POC narrative and reading order](knowledge-base/pocs/001-project-forge-local-ai-compute-hardware-lab/README.md)
- [Executive proposal](knowledge-base/proposals/001-project-forge-local-ai-compute-hardware-lab.md)
- [Architecture decision record](knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md)
- [Current POC findings](knowledge-base/pocs/001-project-forge-local-ai-compute-hardware-lab/06-findings-and-decision/results.md)
- [Final Phase 1 decision](knowledge-base/pocs/001-project-forge-local-ai-compute-hardware-lab/06-findings-and-decision/poc-final-decision.md)
- [Benchmark method](knowledge-base/pocs/001-project-forge-local-ai-compute-hardware-lab/03-benchmark-method/benchmark-contract-v1.md)
- [Quality evaluation](knowledge-base/pocs/001-project-forge-local-ai-compute-hardware-lab/05-quality-evaluation/quality-evaluation-register-v1.md)

## Operating Principles

- Use only synthetic, public, or personally authored non-sensitive inputs.
- Record observed evidence; do not infer performance from specifications alone.
- Keep prefill and generation metrics separate.
- Add runtimes or model tiers one at a time, with a recorded reason and comparable evidence.
- Defer GPU procurement decisions until workload needs, runtime support, measured performance, and API alternatives are evaluated.

## Project Status

Phase 1 is closed: retain Machine A, use local 8B models only with human validation, and do not approve a GPU purchase. A future GPU-versus-API economic decision requires a representative workload, monthly usage profile, current candidate compatibility/pricing, and measured candidate performance.
