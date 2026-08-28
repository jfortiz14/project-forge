# CC-003B-002 Loaded-Idle Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-002`
> **State:** Model loaded; HTTP server idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 22,982 MiB |
| GPU VRAM total / used / free | 8,192 / 7,725 / 294 MiB |
| GPU temperature / power / utilization | 42 °C / 13.18 W / 4% |
| GPU display-active state | Off |

The running server process was observed by `nvidia-smi`. The candidate passed the loaded-idle stability observation, but 294 MiB free VRAM is narrow headroom and must be considered in later practicality decisions.
