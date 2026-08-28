# CC-003B-003 Startup Evidence — 64K Context Scaling

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-003`
> **Changed parameter:** `-c 65536` only from CC-003B-002
> **Outcome:** Model loaded and HTTP server reached readiness

## Fit Outcome

| Field | Observed value |
| --- | ---: |
| Requested / allocated context per slot | 65,536 / 65,536 |
| Available compute memory before expert overrides | 5,628 MiB |
| Device memory used after 40 expert overrides | 5,651 MiB |
| Device memory available after expert overrides | 6,076 MiB |
| Fit headroom at runtime estimate | 425 MiB |

## Placement Comparison to CC-003B-002

| Resource | CC-003B-002 (32K) | CC-003B-003 (64K) |
| --- | ---: | ---: |
| Expert overrides | 39 | 40 |
| CUDA_Host tensor buffer | 24,967.85 MiB | 25,597.85 MiB |
| CUDA tensor buffer | 2,214.88 MiB | 1,584.88 MiB |
| CUDA KV buffer | 322.82 MiB | 582.82 MiB |
| CUDA compute buffer | 3,912.00 MiB | 3,912.00 MiB |

The runtime automatically shifted additional expert tensors to host memory, offsetting the larger KV cache. It reported 42/42 layers offloaded and initialized one 65,536-token slot. The `mmproj` remained on CPU.

## Boundary

This candidate passed the feasibility gate. Loaded-idle telemetry and the same warm Prompt v1 measurement remain required before it can be compared for practical use.
