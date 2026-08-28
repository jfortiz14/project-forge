# CC-003B-004 Startup Evidence — 98K Context Scaling

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-004`
> **Changed parameter:** `-c 98304` only from CC-003B-003
> **Outcome:** Model loaded and HTTP server reached readiness

## Fit Outcome

| Field | Observed value |
| --- | ---: |
| Requested / allocated context per slot | 98,304 / 98,304 |
| Available compute memory before expert overrides | 5,628 MiB |
| Device memory used after 40 expert overrides | 5,937 MiB |
| Device memory available after expert overrides | 6,076 MiB |
| Fit headroom at runtime estimate | 139 MiB |

## Placement

| Resource | Observed value |
| --- | ---: |
| Repeating / total layers reported offloaded | 41 / 42 |
| Expert overrides to host memory | 40 |
| CUDA_Host tensor buffer | 25,597.85 MiB |
| CUDA tensor buffer | 1,584.88 MiB |
| CUDA KV buffer | 842.82 MiB |
| CUDA compute buffer | 3,912.00 MiB |
| CUDA_Host compute buffer | 800.11 MiB |
| Multimodal projector placement | CPU (`--no-mmproj-offload`) |

The runtime loaded the model, initialized one 98,304-token slot, and started the HTTP server on port 8080. Relative to CC-003B-003, automatic expert placement did not change; the 260.00 MiB KV-buffer increase reduced fit-estimate headroom from 425 MiB to 139 MiB.

## Boundary

This candidate passes the startup and context-allocation gate, but its very narrow estimated fit headroom requires loaded-idle telemetry and a bounded warm request before practical usability can be assessed.
