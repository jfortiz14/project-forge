# CC-003A-002 Startup Evidence — Controlled Background-Load Retry

> **Experiment:** `FORGE-CC-003A`  
> **Run:** `CC-003A-002`  
> **Outcome:** Failed before model load and server readiness  
> **Privacy:** Local absolute paths are redacted as `<LOCAL_LLM_ROOT>`.

## Reproduction Control

CC-003A-002 reused the identical pinned runtime, model, `mmproj`, requested 262144 context, and frozen community flags from CC-003A-001. The only intended difference was reduced non-essential background GPU load.

## Observed Result

| Observation | CC-003A-001 | CC-003A-002 |
| --- | ---: | ---: |
| Pre-start free physical memory | 50,860 MiB | 50,738 MiB |
| Pre-start free VRAM | 6,682 MiB | 6,739 MiB |
| Runtime free VRAM at CUDA initialization | 7,100 MiB | 7,100 MiB |
| Runtime required device memory after CPU expert overrides | 7,367 MiB | 7,367 MiB |
| Runtime available device memory after CPU expert overrides | 4,070 MiB | 4,070 MiB |
| Final outcome | `Unable to auto-fit model` | `Unable to auto-fit model` |

The runtime again selected `CUDA0`, parsed the valid Q6_K GGUF metadata, attempted CPU overrides for MoE experts in layers 39 through 0, and terminated before model tensors were loaded. No 262K context allocation, server readiness, prefill, or generation occurred.

## Interpretation

The modest reduction in background VRAM use did not alter the runtime's fit calculation or outcome. This establishes two same-configuration reproduction observations under slightly different transient machine states; it does not authorize parameter tuning within CC-003A.
