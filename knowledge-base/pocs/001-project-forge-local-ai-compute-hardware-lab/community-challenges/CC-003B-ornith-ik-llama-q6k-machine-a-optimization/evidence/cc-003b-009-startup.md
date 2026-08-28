# CC-003B-009 Startup Evidence — 131K Batch Trade-Off

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-009`
> **Changed parameters:** `-b 4096 -ub 4096` to `-b 2048 -ub 2048` only from CC-003B-007
> **Outcome:** Model loaded and HTTP server reached readiness

## Fit Outcome

| Field | Observed value |
| --- | ---: |
| Requested / allocated context per slot | 131,072 / 131,072 |
| Model tensors plus cache required | 29,356 MiB |
| Available compute memory before expert overrides | 5,852 MiB |
| Device memory used after 37 expert overrides | 6,045 MiB |
| Device memory available after expert overrides | 6,076 MiB |
| Fit headroom at runtime estimate | 31 MiB |

## Placement Comparison to CC-003B-007

| Resource | CC-003B-007 (batch 4096) | CC-003B-009 (batch 2048) |
| --- | ---: | ---: |
| Expert overrides to host memory | 40 | 37 |
| CUDA_Host tensor buffer | 25,597.85 MiB | 23,707.85 MiB |
| CUDA tensor buffer | 1,584.88 MiB | 3,474.88 MiB |
| CUDA KV buffer | 782.81 MiB | 782.81 MiB |
| CUDA compute buffer | 3,912.00 MiB | 1,956.00 MiB |
| CUDA_Host compute buffer | 1,056.11 MiB | 528.05 MiB |

The runtime initialized one 131,072-token slot at batch and micro-batch 2048. Reducing the batching parameters halves the compute buffers, but `--fit` spends most of that recovery by returning three expert layers to GPU. The result is a 31 MiB fit estimate, materially narrower than CC-003B-007's 205 MiB.
