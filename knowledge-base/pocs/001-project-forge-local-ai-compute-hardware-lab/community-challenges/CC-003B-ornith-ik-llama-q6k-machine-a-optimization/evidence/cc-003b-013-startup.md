# CC-003B-013 Clean-Restart Startup Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-013`
> **Server configuration:** Unchanged from CC-003B-011
> **Outcome:** Model loaded and HTTP server reached readiness after a clean restart

| Field | Observed value |
| --- | ---: |
| Requested / allocated context per slot | 196,608 / 196,608 |
| Expert overrides to host memory | 38 |
| Device memory used after overrides | 5,811 MiB |
| Device memory available after overrides | 6,076 MiB |
| Fit headroom at runtime estimate | 265 MiB |
| CUDA_Host / CUDA tensor buffer | 24,337.85 / 2,844.88 MiB |
| CUDA KV buffer | 1,142.81 MiB |
| CUDA compute buffer | 1,956.00 MiB |
| K/V cache type | `q4_0` / `q4_0` |

The clean restart reproduced CC-003B-011's runtime-reported placement and fit estimate. It initialized one 196,608-token slot and reached HTTP server readiness. The pasted log did not include an initial runtime timestamp, so cold-load duration is not recorded.
