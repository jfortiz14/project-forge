# CC-003B Quality Evaluation

> **Challenge:** `FORGE-CC-003B`
> **Phase:** Quality evaluation of the selected Machine A v2 capacity/performance profile
> **Status:** Closed and frozen; quality sequence complete
> **Data class:** Synthetic/public only

## Purpose

Measure whether the selected CC-003B configuration produces acceptable Azure/C# planning, implementation, contract-test generation, and read-only review outputs under the canonical FORGE quality method.

This phase does not revise the completed capacity/performance findings. It separately evaluates answer quality and output discipline, including the possible trade-off introduced by the selected lower-precision KV cache.

## Frozen Quality Profile

The quality profile retains the selected capacity/performance configuration and changes only reasoning behavior to an explicit no-thinking mode for comparison with CC-002:

| Field | Frozen value |
| --- | --- |
| Runtime / source commit | `ik_llama.cpp` / `0ed847d3140baead542abe3e5e6fe841013e7340` |
| Model | Ornith 1.5 35B-A3B Q6_K; model hash recorded in the parent [execution manifest](../execution-manifest.md) |
| Context | 196,608 |
| Batch / micro-batch | 2,048 / 2,048 |
| K/V cache | `q4_0` / `q4_0` |
| Fit controls | `--fit --fit-margin 1024` |
| Projector placement | CPU (`--no-mmproj-offload`) |
| Quality reasoning mode | `--reasoning-budget 0 --reasoning-tokens none` |
| Session behavior | Fresh server launch and one fresh request per quality unit; no conversation history or retry-as-correction |

The reasoning-mode change is explicitly scoped to quality comparability. It does not alter the capacity/performance recommendation's placement parameters.

## Required Order

`raw model content -> SHA-256 -> build -> contractual tests -> human/contract review`

Do not inspect, edit, format, or derive a generated C# artifact before its raw file hash and literal build result are recorded. A build failure closes that autonomous unit at the compile gate; tests and semantic review do not bypass it.

## Artifact Map

- [Quality register](quality-evaluation-register-v1.md) — frozen units, prompts, entry gates, and verdict rules.
- [Frozen prompts](prompts/README.md) — exact prompt files and their hashes.
- [Frozen inputs](frozen-inputs.md) — canonical source documents and fixture hashes.
- [Execution manifest](execution-manifest.md) — frozen runtime profile and per-run evidence contract.
- [Execution events](execution-events.md) — chronological tooling, capture, and quality-unit history.
- [Operator runbook](operator-runbook.md) — launch, raw-capture, and gate sequence for `ik_llama.cpp`.
- [Q-004 capture script](scripts/Invoke-ForgeCc003BQ004Capture.ps1) — hash-verifies the frozen review fixture, composes the request, and preserves raw evidence.
- [Historical draft register](quality-evaluation-register-v1.old.md) and [historical draft summary](quality-evaluation-register-summary.old.md) — preserved snapshots superseded by the normalized records above.
- [Results](results.md) — concise final outcomes after execution.
- [Evidence](evidence/) — immutable raw model outputs, hashes, build/test logs, and review evidence.
- [Build](build/) — disposable build workspaces; never overwrite raw evidence.

## Boundaries

- CC-002 and canonical fixtures remain read-only references; this package never changes them.
- Q-002 uses the human-reviewed reference architecture and domain contract even if Q-001 fails. That intervention is recorded and isolates implementation capability.
- Each autonomous unit receives at most one explicitly documented corrective pass. A re-run is a separate reproducibility measurement, never an overwrite of a failed verdict.
- No corporate code, secrets, customer data, PHI, or proprietary architecture may enter prompts or evidence.
