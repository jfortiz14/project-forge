# Machine A Hardware Inventory and Versioned Baseline

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab
> **Captured:** 2026-08-15 10:51:51 local time
> **Evidence type:** Read-only inventory; not a performance benchmark

## Purpose

This document freezes the currently evidenced Machine A hardware and runtime state as the canonical `Machine A v1` baseline before the planned RAM upgrade to 64 GB.

The file also defines `Machine A v2` as a planned, unmeasured future state so that later results are not accidentally attributed to the frozen 32 GB configuration.

## Versioned Machine States

| Machine state | Status | Description |
| --- | --- | --- |
| Machine A v1 | Frozen / canonical | Current evidenced configuration: RTX 3070 8 GB + 32 GB system RAM + recorded software/runtime environment. |
| Machine A v2 | Planned / unmeasured | Future configuration after the intended RAM upgrade to 64 GB system RAM. No independent measurement exists yet. |

## Machine A v1 Verified Inventory

| Category | Verified value |
| --- | --- |
| Operating system | Windows 11 Pro, version 10.0.26200, build 26200, 64-bit |
| CPU | 12th Gen Intel Core i7-12700KF; 12 physical cores; 20 logical processors; reported max clock 3600 MHz |
| System memory | 32 GiB total: 2 × Kingston KHX2666C16/16G, each 16 GiB, configured at 2666 MT/s |
| Compute GPU | NVIDIA GeForce RTX 3070 |
| GPU VRAM | 8192 MiB total, per `nvidia-smi` |
| NVIDIA driver | 576.88 |
| CUDA / runtime | CUDA compatibility reported by `nvidia-smi`: 12.9; Ollama version 0.32.14 installed |
| Ollama | Version 0.32.14 installed |
| Relevant storage / model location | Not recorded |
| Motherboard | Not recorded |

## Machine A v2 Planned State

| Category | Planned value |
| --- | --- |
| System memory | 64 GB total, intended RAM upgrade from the frozen `Machine A v1` 32 GB configuration |
| CPU | Same as `Machine A v1` unless later independently recorded |
| Compute GPU | Same as `Machine A v1` unless later independently recorded |
| GPU VRAM | Same as `Machine A v1` unless later independently recorded |
| Motherboard | Not recorded |
| Operating system | Not recorded |
| NVIDIA driver | Not recorded |
| CUDA / runtime | Not recorded |
| Ollama | Not recorded |
| Relevant storage / model location | Not recorded |

`Machine A v2` is intentionally left unmeasured until the RAM upgrade is physically completed and independently recorded.

## Relationship To FORGE Evidence

The following completed FORGE artifacts were executed on `Machine A v1` and must remain attributed to the frozen 32 GB configuration:

| Area | Artifact set | Machine version |
| --- | --- | --- |
| POC baseline performance and decision | `04-performance-evidence/` and `06-findings-and-decision/` | Machine A v1 |
| CC-001 Ornith performance baseline | `community-challenges/CC-001-ornith/CC-001A-independent-baseline/` | Machine A v1 |
| CC-001 Ornith quality evaluation | `community-challenges/CC-001-ornith/quality-evaluation/` and related CC-001 artifacts | Machine A v1 |
| CC-002 Qwen performance baseline | `community-challenges/CC-002-qwen/CC-002A-independent-baseline/` | Machine A v1 |
| CC-002 Qwen quality evaluation | `community-challenges/CC-002-qwen/quality-evaluation/` and related CC-002 artifacts | Machine A v1 |

These artifacts preserve the historical evidence for the frozen `Machine A v1` baseline. They are not reinterpreted here.

## Capture Conditions And Caveats

- At capture, `nvidia-smi` reported 1,978 MiB / 8,192 MiB VRAM in use, 23% GPU utilization, 46 °C, and 25 W board power.
- Parsec, desktop-shell applications, browsers, and other GUI processes were active. This is acceptable for inventory but **not** for a controlled inference baseline.
- `Win32_VideoController.AdapterRAM` reported approximately 4 GiB for the RTX 3070. For this POC, `nvidia-smi` is the authoritative source for NVIDIA VRAM capacity: 8 GiB.
- Virtual/remote display adapters were present and are excluded from compute-GPU capacity analysis.

## Historical Interpretation

Machine A v1 is the consumer NVIDIA 8 GiB CUDA baseline used for the completed FORGE POC and the CC-001 / CC-002 challenge artifacts. The RAM upgrade to `Machine A v2` is a future change and does not alter the attribution of any existing evidence.
