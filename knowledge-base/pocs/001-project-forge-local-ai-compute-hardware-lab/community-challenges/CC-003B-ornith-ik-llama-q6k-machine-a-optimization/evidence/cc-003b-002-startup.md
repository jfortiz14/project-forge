# CC-003B-002 Startup Evidence — Fit Headroom

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-002`
> **Changed parameter:** `--fit-margin 1024` only from CC-003B-001
> **Outcome:** Model loaded and HTTP server reached readiness

## Fit Outcome

| Field | Observed value |
| --- | ---: |
| Requested / allocated context per slot | 32,768 / 32,768 |
| Available compute memory before expert overrides | 5,628 MiB |
| Device memory used after 39 expert overrides | 5,995 MiB |
| Device memory available after 39 expert overrides | 6,076 MiB |
| Fit headroom at runtime estimate | 81 MiB |

## Observed Placement and Buffers

- The runtime reported 42/42 layers offloaded to GPU while expert tensors were overridden to `CUDA_Host`.
- CUDA_Host tensor buffer: 24,967.85 MiB.
- CUDA tensor buffer: 2,214.88 MiB.
- CUDA KV buffer: 322.82 MiB; K cache `q8_0`, V cache `q4_0`.
- CUDA compute buffer: 3,912.00 MiB.
- The BF16 multimodal projector used the CPU backend, consistent with `--no-mmproj-offload`.

## Readiness Evidence

The runtime reported `n_ctx = 32768`, initialized one slot at `n_ctx_slot = 32768`, logged `model loaded`, and started the HTTP server on port 8080. The first runtime timestamp was 1787858414 and the `model loaded` timestamp was 1787858613, an observed elapsed interval of approximately 199 seconds.

## Boundary

This candidate passed only the feasibility gate. Loaded-idle telemetry and a fixed-prompt prefill/generation measurement are still required before it can be considered practical or recommended.
