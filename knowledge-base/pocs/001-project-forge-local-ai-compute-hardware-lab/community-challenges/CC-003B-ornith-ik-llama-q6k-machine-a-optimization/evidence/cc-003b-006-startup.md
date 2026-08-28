# CC-003B-006 Startup Evidence — 131K Context-Boundary Probe

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-006`
> **Changed parameter:** `-c 131072` only from CC-003B-004
> **Outcome:** Failed before tensor load; `Unable to auto-fit model`

| Field | Observed value |
| --- | ---: |
| Requested context | 131,072 |
| Model tensors plus cache required | 31,424 MiB |
| Available device memory excluding compute | 5,628 MiB |
| Expert overrides attempted | 40 (all MoE expert tensors) |
| Required device memory after overrides | 6,223 MiB |
| Device memory available after overrides | 6,076 MiB |
| Shortfall | 147 MiB |

The runtime exhausted its automatic MoE expert-to-host override strategy and then rejected the configuration before tensor allocation. No model load, context allocation, or server readiness occurred.

## Boundary

With `-ctk q8_0`, `-ctv q4_0`, `--fit-margin 1024`, and all other CC-003B-004 flags retained, 131,072 tokens exceed the observed auto-fit boundary on Machine A v2. This establishes 98,304 as the highest successful configured context before cache-type tuning.
