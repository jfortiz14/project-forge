# Evidence: Machine A / Ollama / Qwen3 14B — Attempt 001

> **Status:** Incomplete — excluded from performance comparison  
> **Prompt:** Benchmark Contract v1  
> **Context target:** 4096

## Observed Output

The PowerShell calculation object returned no model, token counts, or timing fields. It displayed `LoadSeconds: 0` and `NaN` for prefill and generation throughput. The raw API response was not captured, so the cause cannot be determined from this attempt.

## Valid Partial Evidence

| Field | Observed value |
| --- | --- |
| Ollama placement | `37%/63% CPU/GPU` |
| Loaded model size reported by Ollama | 10 GB |
| Context | 4096 |
| GPU memory after attempt | 7,845 MiB / 8,192 MiB |
| GPU compute utilization after attempt | 3% |

## Interpretation

The 14B Q4_K_M baseline is not fully GPU-resident on the RTX 3070 8 GiB in this runtime configuration; Ollama reported mixed CPU/GPU placement. This is not a throughput result. The next attempt must retain and display the raw API response before calculating metrics.

