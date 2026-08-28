# CC-003B-007 Startup Evidence — 131K K-Cache Trade-Off

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-007`
> **Changed parameter:** `-ctk q8_0` to `-ctk q4_0` only from CC-003B-006
> **Outcome:** Model loaded and HTTP server reached readiness

## Fit Outcome

| Field | Observed value |
| --- | ---: |
| Requested / allocated context per slot | 131,072 / 131,072 |
| Model tensors plus cache required | 31,072 MiB |
| Available compute memory before expert overrides | 5,628 MiB |
| Device memory used after 40 expert overrides | 5,871 MiB |
| Device memory available after expert overrides | 6,076 MiB |
| Fit headroom at runtime estimate | 205 MiB |

## Placement and Cache

| Resource | Observed value |
| --- | ---: |
| Repeating / total layers reported offloaded | 41 / 42 |
| Expert overrides to host memory | 40 |
| CUDA_Host tensor buffer | 25,597.85 MiB |
| CUDA tensor buffer | 1,584.88 MiB |
| CUDA KV buffer | 782.81 MiB |
| KV self size / K / V | 720.00 / 360.00 / 360.00 MiB |
| K cache type / V cache type | `q4_0` / `q4_0` |
| CUDA compute / CUDA_Host compute | 3,912.00 / 1,056.11 MiB |
| Multimodal projector placement | CPU (`--no-mmproj-offload`) |

The runtime initialized one 131,072-token slot and started the HTTP server. Relative to CC-003B-006, reducing only the K cache lowered the model-plus-cache requirement by 352 MiB and changed the failed 147 MiB shortfall into a 205 MiB fit estimate. Quality implications of lower K-cache precision were not evaluated in this challenge.
