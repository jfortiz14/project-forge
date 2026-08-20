# Hardware Inventory Evidence: Machine A — Personal Desktop

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Captured:** 2026-08-15 10:51:51 local time
> **Evidence type:** Read-only inventory; not a performance benchmark

## Verified Inventory

| Category | Verified value |
| --- | --- |
| Operating system | Windows 11 Pro, version 10.0.26200, build 26200, 64-bit |
| CPU | 12th Gen Intel Core i7-12700KF; 12 physical cores; 20 logical processors; reported max clock 3600 MHz |
| System memory | 32 GiB total: 2 × Kingston KHX2666C16/16G, each 16 GiB, configured at 2666 MT/s |
| Compute GPU | NVIDIA GeForce RTX 3070 |
| NVIDIA driver | 576.88; CUDA compatibility reported by `nvidia-smi`: 12.9 |
| GPU VRAM | 8192 MiB total, per `nvidia-smi` |
| GPU board power cap | 220 W, per `nvidia-smi` |
| Ollama | Version 0.32.14 installed |
| Installed Ollama models | None listed at capture time |

## Capture Conditions and Caveats

- At capture, `nvidia-smi` reported 1,978 MiB / 8,192 MiB VRAM in use, 23% GPU utilization, 46 °C, and 25 W board power.
- Parsec, desktop-shell applications, browsers, and other GUI processes were active. This is acceptable for inventory but **not** for a controlled inference baseline.
- `Win32_VideoController.AdapterRAM` reported approximately 4 GiB for the RTX 3070. For this POC, `nvidia-smi` is the authoritative source for NVIDIA VRAM capacity: 8 GiB.
- Virtual/remote display adapters were present and are excluded from compute-GPU capacity analysis.

## Architecture Impact

Machine A is confirmed as the consumer NVIDIA 8 GiB CUDA baseline. The declared Qwen3 14B Q4_K_M test remains unexecuted. Its model availability, actual GPU-layer offload, RAM offload, load time, prefill speed, generation speed, and usability remain pending measured evidence.
