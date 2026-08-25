# Results Matrix: CC-002A Independent Baseline — `qwen3.5:35B-A3B`

Only complete, evidence-backed metrics are entered. `N/R` means not recorded for that run, not zero.

> **Boundary:** This matrix is a FORGE-style follow-up under `001-project-forge-local-ai-compute-hardware-lab`. It does not replace the original POC 001 matrix and does not alter the prior hardware decision.

| Run | Model | Quantization | Machine | Backend | Model Size | RAM | VRAM / Shared | Load Time | Prompt Tokens/s | Generation Tokens/s | Context | GPU Offload | Experience / Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| CC-002A-001 | `qwen3.5:35B-A3B` | Q4_K_M | Machine A - Desktop | Ollama / NVIDIA CUDA mixed offload | 23 GB | 32 GiB | 7,785 MiB / 8,192 MiB | 1m10.8099811s | 75.50 | 27.60 | 4096 | 76%/24% CPU/GPU | `no-thinking`; source model `qwen3.5:35B-A3B`; operational alias observed as `forge-qwen3-35B-A3B-ctx4096-nothink:latest`; 131 prompt tokens; 654 generated tokens; 1m36.2592308s total; `ollama ps` showed `4096`; `nvidia-smi` captured 53C, 48W / 220W, and 1% GPU utilization; response covered all requested areas and stayed in plain prose, making it more format-consistent than the prior Ornith baseline rerun. |
| CC-002A-002 | `qwen3.5:35B-A3B` | Q4_K_M | Machine A - Desktop | Ollama / NVIDIA CUDA mixed offload | 23 GB | 32 GiB | 7,724 MiB / 8,192 MiB | 1m11.9134137s | 63.09 | 30.23 | 4096 | 76%/24% CPU/GPU | `no-thinking`; source model `qwen3.5:35B-A3B`; operational alias observed as `forge-qwen3-35B-A3B-ctx4096-nothink:latest`; 131 prompt tokens; 629 generated tokens; 1m34.8129141s total; `ollama ps` showed `4096`; `nvidia-smi` captured 56C, 22W / 220W, and 5% GPU utilization; response again covered all requested areas and remained plain prose. |
| CC-002A-003 | `qwen3.5:35B-A3B` | Q4_K_M | Machine A - Desktop | Ollama / NVIDIA CUDA mixed offload | 23 GB | 32 GiB | 7,694 MiB / 8,192 MiB | 1m11.5753753s | 104.48 | 29.62 | 4096 | 76%/24% CPU/GPU | `no-thinking`; source model `qwen3.5:35B-A3B`; operational alias observed as `forge-qwen3-35B-A3B-ctx4096-nothink:latest`; 170 prompt tokens; 218 generated tokens; 1m20.5724832s total; `ollama ps` showed `4096`; `nvidia-smi` captured 58C, 37W / 220W, and 2% GPU utilization; response stayed in plain prose and remained close to the requested structure. |

## Interpretation Rules

- An interactive `ollama run` session may load a model before the user prompt is sent; its prompt-level `load duration` is not a cold model-load measurement.
- GPU placement, VRAM, CPU utilization, and GPU utilization must be captured separately for each accepted performance result when possible.
- Thinking and no-thinking profiles are separate rows and are not merged into one throughput result.
- Configuring `num_ctx` to 4096 is required for this baseline slice.

## Context

This CC follows the original FORGE measurement discipline:

- same machine family
- same emphasis on load, prefill, generation, memory, placement, and usability
- same requirement to preserve comparability caveats
- same separation of performance and quality phases

## Directional Observation

`qwen3.5:35B-A3B` completed successfully under the frozen `no-thinking` profile and produced a plain-text response that stayed close to the requested structure. The timing evidence is valid as an observed baseline, while any stronger quality judgment should wait for the separate quality phase.
