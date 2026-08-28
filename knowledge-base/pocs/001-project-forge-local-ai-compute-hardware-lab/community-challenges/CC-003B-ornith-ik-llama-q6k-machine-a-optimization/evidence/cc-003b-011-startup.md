# CC-003B-011 Startup Evidence — 196K Context Scaling

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-011`
> **Changed parameter:** `-c 131072` to `-c 196608` only from CC-003B-009
> **Outcome:** Model loaded and HTTP server reached readiness

## Fit Outcome

| Field | Observed value |
| --- | ---: |
| Requested / allocated context per slot | 196,608 / 196,608 |
| Model tensors plus cache required | 29,752 MiB |
| Available compute memory before expert overrides | 5,852 MiB |
| Device memory used after 38 expert overrides | 5,811 MiB |
| Device memory available after expert overrides | 6,076 MiB |
| Fit headroom at runtime estimate | 265 MiB |

## Placement Comparison to CC-003B-009

| Resource | CC-003B-009 (131K) | CC-003B-011 (196K) |
| --- | ---: | ---: |
| Expert overrides to host memory | 37 | 38 |
| CUDA_Host tensor buffer | 23,707.85 MiB | 24,337.85 MiB |
| CUDA tensor buffer | 3,474.88 MiB | 2,844.88 MiB |
| CUDA KV buffer | 782.81 MiB | 1,142.81 MiB |
| KV self size | 720.00 MiB | 1,080.00 MiB |
| CUDA compute buffer | 1,956.00 MiB | 1,956.00 MiB |
| CUDA_Host compute buffer | 528.05 MiB | 784.05 MiB |

The runtime initialized one 196,608-token slot at batch and micro-batch 2048, with K/V cache `q4_0`/`q4_0`. It automatically moved one additional expert layer to host memory relative to CC-003B-009, which offset the larger KV cache and yielded a 265 MiB fit estimate.
