# FORGE-CC-003B Quality Evaluation Execution Manifest v1

**Status:** Closed and frozen — observed values are recorded in the register, results, and preserved evidence.  
**Scope:** Quality evaluation only; this manifest does not alter the closed CC-003B capacity/performance conclusion.

## Identity and Provenance

| Field | Frozen value |
|---|---|
| Evaluation ID | `FORGE-CC-003B-QE-v1` |
| Challenge | `FORGE-CC-003B` |
| Runtime | `ik_llama.cpp` |
| Runtime commit | `0ed847d3140baead542abe3e5e6fe841013e7340` |
| Model | `Ornith-1.5-35B-Q6_K.gguf` |
| Model SHA-256 | `15d4658bbfc9c6034621729c15bbb50662c82b32a7ddd9624a1e545a74bdbb4b` |
| Multimodal projector | `mmproj-Ornith-1.5-35B-BF16.gguf` |
| Projector SHA-256 | `1921a36a85aee56cd2abd27f46701802c9d85a33474792e600df6c3b282a135d` |
| Target machine | Machine A v2 — Windows, RTX 3070 8 GB, 64 GB DDR4-2666 |

The model and projector paths are intentionally operator-local and are not recorded in this repository.

## Frozen Server Profile

| Parameter | Value |
|---|---:|
| Context | 196,608 |
| Batch / microbatch | 2,048 / 2,048 |
| Parallel sequences | 1 |
| Cache K / V | `q4_0` / `q4_0` |
| Placement | `--device CUDA0 --fit --fit-margin 1024` |
| Multimodal projector placement | CPU (`--no-mmproj-offload`) |
| Reasoning | disabled (`--reasoning-budget 0 --reasoning-tokens none`) |
| Merge flags | `--merge-up-gate-experts --merge-qkv` |
| Chat template | `--jinja` |

The quality run changes the reasoning behavior relative to the capacity performance run, but does not tune context, cache, batch, or placement. Its conclusions apply only to this no-thinking quality profile.

## Evidence Contract

For every autonomous unit, retain under `evidence/`:

1. The exact request body and raw model response, encoded as UTF-8 without BOM.
2. SHA-256 for the response file.
3. Server log excerpt with startup configuration and request timing.
4. Build and contractual-test output where the quality contract requires them.
5. A short observation record in `results.md` and the register.

Each unit starts with a fresh server process and has no inherited conversation state. A terminal unit remains terminal; no silent retries or prompt revisions are permitted.

## Environment Observations

| Field | Value |
|---|---|
| Windows version/build | N/R |
| NVIDIA driver / CUDA reported by driver | NVIDIA 576.88 / driver reports CUDA 12.9 |
| CUDA toolkit / compiler | N/R in this quality-run preflight |
| Free physical memory before server start | 49,444.94 MiB |
| GPU memory before server start | 1,345 MiB used / 6,674 MiB free of 8,192 MiB |
| .NET SDK version | 10.0.400 |
| `llama-server --version` | version 4848 (`0ed847d3`), MSVC 19.44.35228.0, x64 |

## Q-001 Server-Startup Observation

| Observation | Recorded value |
|---|---|
| Server state | HTTP server listening on loopback port 8080; one idle slot |
| Effective context / batch / microbatch | 196,608 / 2,048 / 2,048 |
| Effective KV cache | K `q4_0`, V `q4_0`; 1,080.00 MiB self KV reported |
| Model placement | 42/42 layers offloaded; CUDA buffer 2,844.88 MiB |
| Compute placement | CUDA compute buffer 1,956.00 MiB; host compute buffer 784.05 MiB |
| Host model buffer | CUDA host buffer 24,337.85 MiB; pinned-host allocation 23.77 GiB |
| Projector | CPU backend, as required by `--no-mmproj-offload` |

The log confirms that the quality server uses the frozen placement profile. It does not constitute a quality verdict; Q-001 has not yet been requested.

## Canonical-Method References

- [Quality evaluation README](README.md)
- [Frozen inputs](frozen-inputs.md)
- [Operator runbook](operator-runbook.md)
- [Canonical quality protocol](../../../05-quality-evaluation/protocol/quality-evaluation-protocol-v1.md)
