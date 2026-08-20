# Preflight Evidence: Machine A / Ollama / Qwen3 14B

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Captured:** 2026-08-15 11:18:19 local time
> **Status:** Ready for first cold baseline run

| Check | Observed result |
| --- | --- |
| Loaded Ollama model | None; `ollama ps` returned no rows |
| Target model | `qwen3:14b`, installed; see Model Registry |
| NVIDIA compute utilization | 0% at capture |
| NVIDIA VRAM in use | 2,134 MiB / 8,192 MiB |
| NVIDIA temperature | 48 °C |
| NVIDIA board power | 47 W / 220 W |

## Caveat

Desktop/Parsec and GUI processes occupied background VRAM. The baseline is valid as a real desktop-development measurement, not an exclusive-GPU laboratory measurement. No process will be terminated for this POC without explicit operator direction.
