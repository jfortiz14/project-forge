# CC-003A Execution Manifest

> **Experiment:** `FORGE-CC-003A`  
> **Challenge:** `CC-003A-ornith-ik-llama-q6k`  
> **Status:** Closed — no further execution authorized under CC-003A

## Fixed Intent

Attempt faithful practical reproduction of the frozen community configuration before any Machine A optimization. The external reference is authoritative in [community-reference.md](community-reference.md).

## Local Path Convention

`<LOCAL_LLM_ROOT>` is an operator-private base path. Only relative locations below it are recorded in this challenge; the drive letter and absolute local path are intentionally omitted.

## Required Pre-Execution Record

Record these values before the first start attempt:

| Field | Observed value |
| --- | --- |
| Date/time and operator | 2026-08-26 23:25:36 (local timestamp reported by `nvidia-smi`); operator N/R |
| Windows version | Windows 10 Pro; Windows version `2009`; OS build `26200`; x64-based PC |
| GPU driver version | NVIDIA `576.88` (Windows driver package); WDDM reported by `nvidia-smi` |
| CUDA environment/version | Driver reports CUDA `12.9` through `nvidia-smi`; locally installed CUDA Toolkit compiler is `12.6.20` (`nvcc`) |
| ik_llama.cpp executable/version and commit | `<LOCAL_LLM_ROOT>\runtimes\ik_llama.cpp\build\bin\Release\llama-server.exe`; version `4848` (`0ed847d3`); source `HEAD` `0ed847d3140baead542abe3e5e6fe841013e7340`; built with MSVC `19.44.35228.0` for x64 |
| Model file path, size, and SHA-256 | `<LOCAL_LLM_ROOT>\models\ornith-q6k\Ornith-1.5-35B-Q6_K.gguf`; 29,208,731,392 bytes; `15d4658bbfc9c6034621729c15bbb50662c82b32a7ddd9624a1e545a74bdbb4b` |
| `mmproj` file path, size, and SHA-256 | `<LOCAL_LLM_ROOT>\models\ornith-q6k\mmproj-Ornith-1.5-35B-BF16.gguf`; 902,822,240 bytes; `1921a36a85aee56cd2abd27f46701802c9d85a33474792e600df6c3b282a135d` |
| Available RAM before start | 50,860 MiB free immediately before the first start attempt; total physical memory reported as 68,547,338,240 bytes (~63.84 GiB) |
| Available VRAM before start | 6,682 MiB free; 1,337 MiB used / 8,192 MiB total immediately before the first start attempt |
| Exact Machine A invocation | N/R |
| Adaptation required for Windows, if any | N/R |

## Execution Sequence

1. Capture the pre-execution record and confirm that the Q6_K model and `mmproj` identities are recorded.
2. Attempt startup using the community configuration as faithfully as practical, recording any necessary Windows adaptation separately.
3. Capture whether startup completes, fails, or encounters OOM; preserve the relevant raw console/runtime evidence.
4. If startup completes, capture ik_llama.cpp placement plus RAM and VRAM pressure at idle/loaded state.
5. Execute a cold-load observation and a bounded performance prompt under the configured 262144 context; retain prompt and raw output/metrics.
6. Record prefill and generation separately whenever the runtime exposes them.
7. Record whether the 262K context was actually allocated, rather than inferring allocation from requested flags alone.
8. Only if practical after the above observations, execute one clearly labeled large-context stress run. Record its input/context size, completion state, and OOM/failure evidence.

## Stop / Interpretation Rules

- OOM, inability to start, or inability to allocate 262K is a valid observed reproduction outcome, not a reason to tune the configuration within CC-003A.
- Do not reduce context, change quantization, or modify flags to obtain a pass under this experiment ID.
- Do not mix a quality evaluation into the performance/resource reproduction phase.
- Do not compare throughput as a direct winner/loser result across Machine A and Machine B; hardware, OS, CUDA, build, prompt, and output conditions are material boundaries.
- `N/R` means not recorded or not measured; it does not mean zero, failure, or success.

## Controlled Reproduction Retry Rule

A follow-up run may reduce non-essential background GPU load without changing the runtime build, model files, `mmproj`, context, or any frozen community flag. Record it as a separate run with fresh RAM/VRAM telemetry. This is environmental control for reproduction, not Machine A parameter tuning.

## Evidence Requirements

- Exact Machine A invocation and all differences from the external reference.
- Raw startup/runtime output, including OOM or error output where applicable.
- Model and `mmproj` identities (path, size, SHA-256).
- Runtime build/commit, OS, GPU driver, and CUDA environment.
- Placement/offload evidence.
- RAM and VRAM evidence at the relevant run phases.
- Cold-load time, prefill rate/time, generation rate/time, token counts, and total duration where exposed.
- 262K allocation outcome and, if performed, large-context stress-run outcome.

## Post-Execution Documentation

Enter only observed values in [results-matrix.md](results-matrix.md). Preserve raw evidence in a clearly named `evidence/` subdirectory when it is captured; do not overwrite a prior run.

## Freeze Rule

CC-003A closed after two failed, same-configuration startup attempts. Its community reference, manifest, preflight evidence, startup evidence, and results matrix are historical artifacts and must not be altered by additional runs. A new experiment ID is required for any parameter change, alternative quantization, context adjustment, runtime change, or further diagnostic work.
