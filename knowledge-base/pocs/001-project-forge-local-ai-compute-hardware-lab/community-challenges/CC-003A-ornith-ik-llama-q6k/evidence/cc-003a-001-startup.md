# CC-003A-001 Startup Evidence — Frozen Community Configuration

> **Experiment:** `FORGE-CC-003A`  
> **Run:** `CC-003A-001`  
> **Outcome:** Failed before model load and server readiness  
> **Privacy:** Local absolute paths are redacted as `<LOCAL_LLM_ROOT>`.

## Configuration Identity

- Runtime build: `4848`, commit `0ed847d3`; full source `HEAD` `0ed847d3140baead542abe3e5e6fe841013e7340`.
- Model: Ornith 1.5 35B-A3B Q6_K; SHA-256 `15d4658bbfc9c6034621729c15bbb50662c82b32a7ddd9624a1e545a74bdbb4b`.
- Context requested: `262144`.
- Frozen community flags were used, including `--device CUDA0`, `--fit`, `--fit-margin 3030`, `-ctv q4_0`, `-ctk q8_0`, and the literal `-p 1`.

## Pre-Start State

| Resource | Observed value |
| --- | --- |
| Free physical memory | 50,860 MiB |
| GPU VRAM total / used / free | 8,192 / 1,337 / 6,682 MiB |
| GPU temperature / power / utilization | 40 °C / 12.09 W / 0% |

## Observed Startup Sequence

1. CUDA initialized successfully and detected one RTX 3070 (`compute capability 8.6`); the runtime selected `CUDA0` and reported 7,100 MiB free at that moment.
2. The model file was parsed successfully as GGUF V3, architecture `qwen35moe`, 35B.A3B, Q6_K, with a metadata context length of 262144.
3. The automatic-fit estimator reported 32,568 MiB required for model tensors plus cache, with 3,622 MiB initially available for compute across devices.
4. The runtime attempted CPU expert overrides for every listed MoE layer (layers 39 through 0).
5. After those overrides, it still reported 7,367 MiB required on device 0 versus 4,070 MiB available, then terminated with `Unable to auto-fit model`.

## Terminal Failure Excerpt

```text
CUDA0: using device CUDA0 - 7100 MiB free
Memory required for model tensors + cache: 32568 MiB
Memory available on all devices - compute: 3622 MiB
Adding experts CPU overrides for layer 39 in device 0
...
Adding experts CPU overrides for layer 0 in device 0
Required memory 7367 MiB in device 0 is still greater than available memory 4070 MiB after overriding all MoE tensors to CPU
llama_model_load: error loading model: Unable to auto-fit model
llama_model_load_from_file: failed to load model
llama_init_from_gpt_params: error: failed to load model '<LOCAL_LLM_ROOT>\\models\\ornith-q6k\\Ornith-1.5-35B-Q6_K.gguf'
```

## Evidence-Bound Interpretation

- The exact runtime commit, model identity, CUDA backend, and `CUDA0` target were reached.
- This run does **not** establish a successful Q6_K model load, server start, 262K allocation, placement, prefill, or generation measurement.
- The failure is an observed fit/resource outcome under this Machine A v2 state and frozen community configuration. It is not evidence that the community configuration is invalid on Machine B, nor a justification to tune CC-003A parameters.
