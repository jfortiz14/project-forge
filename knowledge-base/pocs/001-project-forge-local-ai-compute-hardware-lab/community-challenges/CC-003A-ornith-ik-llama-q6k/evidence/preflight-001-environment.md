# CC-003A Preflight Evidence 001 — Machine A v2 Environment

> **Experiment:** `FORGE-CC-003A`  
> **Evidence type:** Operator-captured environment preflight  
> **Capture timestamp:** 2026-08-26 23:25:36 local time, as reported by `nvidia-smi`

## Observed System Environment

| Field | Observed value |
| --- | --- |
| Operating system | Windows 10 Pro |
| Windows version | `2009` |
| OS build | `26200` |
| System type | x64-based PC |
| Total physical memory | 68,547,338,240 bytes (~63.84 GiB) |
| GPU | NVIDIA GeForce RTX 3070 |
| GPU driver package | `576.88` (`32.0.15.7688` reported by `Win32_VideoController`) |
| CUDA version reported by `nvidia-smi` | `12.9` |
| CUDA Toolkit compiler | `nvcc` release `12.6`, version `12.6.20` |
| Compute mode | Default |
| Driver model | WDDM |
| GPU memory at capture | 1,634 MiB / 8,192 MiB |
| GPU utilization at capture | 0% |
| GPU power at capture | 53 W / 220 W |

## Comparability Note

This confirms the expected Windows / RTX 3070 8 GB boundary and a ~64 GiB physical-memory configuration. It does not demonstrate that the Q6_K model, the community server flags, or a 262K context can start or fit. The background desktop workload means this is an idle-state observation, not a free-VRAM guarantee.

## Still Required Before Startup

- ik_llama.cpp executable path, version, and actual commit.
- Model and `mmproj` paths, file sizes, and SHA-256 hashes.
- Exact Windows invocation and any adaptation from the frozen community reference.
- Available-memory observation immediately before the startup attempt.

## Toolchain Probe

| Command | Observed result |
| --- | --- |
| `git --version` | Available: `git version 2.55.0.windows.3` |
| `cmake --version` | Available in the native build-command environment: `3.31.6-msvc6` |
| `nvcc --version` | Available in the native build-command environment: CUDA Toolkit release `12.6`, version `12.6.20` |
| `cl` | Available in the native build-command environment: Microsoft C/C++ Optimizing Compiler `19.44.35228` for x64 |

The NVIDIA driver reporting CUDA 12.9 is distinct from the installed CUDA Toolkit compiler (`nvcc` 12.6.20). The local CMake/CUDA/MSVC toolchain is now available for a source-built CUDA runtime.

## Local Runtime Preparation

`<LOCAL_LLM_ROOT>` denotes the private local base path. The absolute path and drive letter are intentionally not retained in FORGE evidence.

| Field | Observed value |
| --- | --- |
| Source location | `<LOCAL_LLM_ROOT>\runtimes\ik_llama.cpp` |
| CUDA build configuration requested | `cmake -S . -B build -DGGML_CUDA=ON` |
| Built server executable | `<LOCAL_LLM_ROOT>\runtimes\ik_llama.cpp\build\bin\Release\llama-server.exe` |
| Actual source `HEAD` | `0ed847d3140baead542abe3e5e6fe841013e7340` |
| Server version/build report | Version `4848` (`0ed847d3`), built with MSVC `19.44.35228.0` for x64 |

## Model Artifact Verification

| Artifact | Local path | Size | Observed SHA-256 | Verification result |
| --- | --- | ---: | --- | --- |
| Ornith 1.5 35B Q6_K | `<LOCAL_LLM_ROOT>\models\ornith-q6k\Ornith-1.5-35B-Q6_K.gguf` | 29,208,731,392 bytes | `15d4658bbfc9c6034621729c15bbb50662c82b32a7ddd9624a1e545a74bdbb4b` | Matches the official file's published size and SHA-256. |
| Ornith 1.5 BF16 `mmproj` | `<LOCAL_LLM_ROOT>\models\ornith-q6k\mmproj-Ornith-1.5-35B-BF16.gguf` | 902,822,240 bytes | `1921a36a85aee56cd2abd27f46701802c9d85a33474792e600df6c3b282a135d` | Matches the official file's published SHA-256 observed at retrieval time. |

## First Start Attempt — Pre-Start Memory

| Field | Observed value |
| --- | --- |
| Free physical memory immediately before start | 50,860 MiB |
| GPU | NVIDIA GeForce RTX 3070 |
| GPU driver | 576.88 |
| VRAM total / used / free | 8,192 / 1,337 / 6,682 MiB |
| GPU temperature / power / utilization | 40 °C / 12.09 W / 0% |

## Runtime Flag Compatibility Probe

The `llama-server --help` output from the pinned build confirms recognition of the community configuration's long-form flags for host, port, device, model, `mmproj`, `--no-mmproj-offload`, metrics, parallel sequences, reasoning budget/tokens, Jinja, KV-cache types, automatic fit, and fit margin. It also confirms the short aliases `-ctk`, `-ctv`, `-muge`, and `-mqkv`.

The remaining short aliases were captured: `-c` is context size, `-b` is logical batch size, `-ub` is physical micro-batch size, and `-p` is the initial prompt/system prompt in conversation mode. Therefore the frozen community `-p 1` is reproduced as the literal initial prompt `1`; it is not a device or parallelism setting.

`--list-devices` is not supported by this pinned build. The help text documents `CUDA0` as a valid `--device` example, so `CUDA0` remains the frozen target for the first runtime attempt; actual placement must be taken from startup output and telemetry.
