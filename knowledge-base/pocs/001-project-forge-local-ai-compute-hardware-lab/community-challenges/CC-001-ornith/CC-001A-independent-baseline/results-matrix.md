# Results Matrix: CC-001A Independent Baseline — Ornith 1.5 35B-A3B

Only complete, evidence-backed metrics are entered. `N/R` means not recorded for that run, not zero.

> **Boundary:** This matrix is a FORGE-style follow-up under `001-project-forge-local-ai-compute-hardware-lab`. It does not replace the original POC 001 matrix and does not alter the prior hardware decision.

| Run | Model | Quantization | Machine | Backend | Model Size | RAM | VRAM / Shared | Load Time | Prompt Tokens/s | Generation Tokens/s | Context | GPU Offload | Experience / Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| CC-001A-001 | Ornith 1.5 35B-A3B | Q4_K_M | Machine A — Desktop | Ollama / NVIDIA CUDA mixed offload | N/R | 32 GiB | N/R | 1m6.1930145s | 92.49 | 28.42 | 4096 | N/R | `no-thinking`; source model `hf.co/ornith-ai/Ornith-1.5-35B-A3B-GGUF:Q4_K_M`; operational alias `forge-ornith-35B-A3B-ctx4096-nothink`; 138 prompt tokens; 655 generated tokens; 1m30.751606s total; response used Markdown headings and bold formatting, so it is only partially comparable to the original FORGE `Benchmark Contract v1`. |
| CC-001A-002 | Ornith 1.5 35B-A3B | Q4_K_M | Machine A — Desktop | Ollama / NVIDIA CUDA mixed offload | 22 GB | 32 GiB | 7,795 MiB / 8,192 MiB | 1m7.0623469s | 89.04 | 30.13 | 4096 | 75%/25% CPU/GPU | `no-thinking`; source model `hf.co/ornith-ai/Ornith-1.5-35B-A3B-GGUF:Q4_K_M`; operational alias `forge-ornith-35B-A3B-ctx4096-nothink`; 138 prompt tokens; 677 generated tokens; 1m31.0929904s total; Ollama reported the loaded alias at 21 GB and `ollama ps` showed `4096`; `nvidia-smi` captured 7,795 MiB / 8,192 MiB VRAM, 3% GPU utilization, 51C, and 21W board power; output again used Markdown headings and bold formatting, so it remains only partially comparable to the original FORGE `Benchmark Contract v1`. |
| CC-001A-003 | Ornith 1.5 35B-A3B | Q4_K_M | Machine A — Desktop | Ollama / NVIDIA CUDA mixed offload | 21 GB | 32 GiB | 7,750 MiB / 8,192 MiB | 1m7.266259s | 106.13 | 31.24 | 4096 | 75%/25% CPU/GPU | `no-thinking`; source model `hf.co/ornith-ai/Ornith-1.5-35B-A3B-GGUF:Q4_K_M`; operational alias `forge-ornith-35B-A3B-ctx4096-nothink`; 171 prompt tokens; 765 generated tokens; 1m33.383157s total; `ollama ps` while loaded showed 21 GB and `4096`; `nvidia-smi` while loaded captured 7,750 MiB / 8,192 MiB VRAM, 1% GPU utilization, 55C, and 20W board power; output again used Markdown formatting and remains only partially comparable to the original FORGE `Benchmark Contract v1`. |

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

The Ornith baseline completed successfully under the frozen `no-thinking` profile, but its output format drifted from the exact FORGE prompt contract. The timing evidence is still valid as an observed baseline, while direct qualitative equivalence to the original FORGE benchmark remains partial until a strict-format rerun is captured.
