# Results Matrix: CC-003A Community Configuration Reproduction

> **Experiment:** `FORGE-CC-003A`  
> **Model:** `Ornith-1.5-35B-Q6_K.gguf`  
> **Runtime target:** `ik_llama.cpp` at community reference commit `0ed847d`  
> **Status:** Closed — failure retained; no additional rows may be appended under CC-003A  
> **Rule:** Enter only observed, evidence-backed values. `N/R` means not recorded or not measured.

## Startup and Configuration Reproduction

| Run | Exact Machine A invocation / adaptation | Runtime build/commit | Model SHA-256 | mmproj SHA-256 | Startup outcome | OOM / failure evidence | 262K requested | 262K allocated | Placement / offload | RAM pressure | VRAM pressure | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| CC-003A-001 | Frozen community flags; local paths redacted as `<LOCAL_LLM_ROOT>` | `0ed847d3140baead542abe3e5e6fe841013e7340` / build 4848 | `15d4658bbfc9c6034621729c15bbb50662c82b32a7ddd9624a1e545a74bdbb4b` | `1921a36a85aee56cd2abd27f46701802c9d85a33474792e600df6c3b282a135d` | **Failed before server readiness** | `Unable to auto-fit model`; see `evidence/cc-003a-001-startup.md` | 262144 | **No** — model load failed before context allocation completed | `CUDA0` selected; auto-fit attempted CPU overrides for MoE experts in layers 0–39 | Pre-start: 50,860 MiB free; post-load RAM N/R | Pre-start: 6,682 MiB free. Runtime reported 7,100 MiB free at CUDA init; fit estimation still failed. | The runtime read valid model metadata but did not load model tensors. No parameter was changed. |

## Cold-Load and Bounded Performance Observation

| Run | Context requested / allocated | Prompt tokens | Generated tokens | Cold-load time | Prefill rate / time | Generation rate / time | Total duration | RAM peak / observation | VRAM peak / observation | Completion / notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| CC-003A-002 | 262144 requested / no allocation | N/R | N/R | N/R | N/R | N/R | N/R | Pre-start: 50,738 MiB free; post-load RAM N/R | Pre-start: 6,739 MiB free; runtime again reported 7,100 MiB free at CUDA init | **Failed before server readiness** — identical `Unable to auto-fit model` outcome under reduced background GPU load; see `evidence/cc-003a-002-startup.md`. |

## Large-Context Stress Observation

This run is conditional. Do not execute it merely to force a result; run it only if the preceding configuration is practical enough to observe safely.

| Run | Actual context/input size | Allocation outcome | Completion outcome | OOM / runtime evidence | RAM peak / observation | VRAM peak / observation | Prefill | Generation | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| CC-003A-003 | N/R | N/R | N/R | N/R | N/R | N/R | N/R | N/R | Conditional large-context stress run. |

## Interpretation Boundary

This matrix reports whether Machine A v2 can reproduce the community configuration sufficiently to observe its behavior. It is not a Machine A optimization score, a direct performance ranking against Machine B, or a model-quality acceptance result.
