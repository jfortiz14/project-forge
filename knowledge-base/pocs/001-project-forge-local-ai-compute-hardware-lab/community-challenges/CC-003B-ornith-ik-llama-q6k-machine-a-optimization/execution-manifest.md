# CC-003B Execution Manifest

> **Experiment:** `FORGE-CC-003B`
> **Challenge:** `CC-003B-ornith-ik-llama-q6k-machine-a-optimization`
> **Status:** Prepared — execution pending

## Fixed Baseline

| Field | Fixed value |
| --- | --- |
| Machine | Machine A v2: Windows, RTX 3070 8 GiB, 64 GB DDR4-2666 |
| Runtime | `ik_llama.cpp`, source commit `0ed847d3140baead542abe3e5e6fe841013e7340`, build 4848 |
| CUDA toolkit | `nvcc` 12.6.20; driver-reported CUDA 12.9 |
| Model | Ornith 1.5 35B-A3B Q6_K; SHA-256 `15d4658bbfc9c6034621729c15bbb50662c82b32a7ddd9624a1e545a74bdbb4b` |
| `mmproj` | Ornith 1.5 BF16; SHA-256 `1921a36a85aee56cd2abd27f46701802c9d85a33474792e600df6c3b282a135d` |
| Local path convention | `<LOCAL_LLM_ROOT>` is private; only relative locations are recorded |

## CC-003A Starting Boundary

The frozen community configuration with context 262144, `--fit`, and `--fit-margin 3030` failed to auto-fit in two CC-003A runs. After CPU overrides for all MoE experts, the runtime reported 7,367 MiB required on device 0 and 4,070 MiB available. This is the baseline boundary to improve; it is not a configuration to overwrite.

## Tunable Parameters

The following may change only when the exact values and reason are recorded in the results matrix:

- context (`-c`);
- fit margin (`--fit-margin`) and fit behavior (`--fit`);
- cache types (`-ctk`, `-ctv`);
- batch and micro-batch sizes (`-b`, `-ub`);
- explicit placement/offload controls supported by the pinned runtime;
- other memory-related runtime controls, when a concrete hypothesis is recorded first.

## Invariants

- Do not change model file, quantization, `mmproj`, runtime commit, driver, or hardware within the primary CC-003B comparison set.
- Preserve exact command lines in private operator evidence; redact local drive paths in repository documentation.
- Change one parameter family per candidate whenever practical. If multiple parameters must change together, state why and treat it as a new candidate configuration.
- Capture pre-start RAM/VRAM, startup/placement output, and post-start telemetry for every attempted candidate.
- Do not claim a usable configuration from server readiness alone; cold-load and bounded prompt evidence remain required.

## Execution Gates

1. **Feasibility:** A candidate starts and loads model tensors without OOM.
2. **Context:** Record actual allocated context and its resource cost.
3. **Stability:** Observe loaded-idle RAM/VRAM and placement for a bounded period.
4. **Performance:** Record cold load, prefill, generation, token counts, and total duration with a fixed prompt.
5. **Practicality:** Compare accepted candidates using the decision rules in [tuning-protocol.md](tuning-protocol.md).

## Stop Conditions

- System instability, unbounded memory pressure, or operator safety concerns.
- Artifact/runtime identity cannot be verified.
- A result lacks the command, changed-parameter rationale, or required telemetry.

## Quality Boundary

CC-003B begins as a performance/resource optimization challenge. It does not confer autonomous quality approval or reopen the prior Azure/C# quality results. Any quality phase must use the canonical FORGE framework and be explicitly added after a practical candidate is accepted.
