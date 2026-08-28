# CC-003B-011 Pre-Start Evidence — 196K Context-Scaling Probe

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-011`
> **Parent:** `CC-003B-009`
> **Changed parameter:** `-c 131072` to `-c 196608` only; retain batch/micro-batch 2048 and K/V cache `q4_0`

| Resource | Observed value |
| --- | --- |
| Free physical memory | 52,310.26 MiB |
| GPU VRAM total / used / free | 8,192 / 938 / 7,081 MiB |
| GPU temperature / power / utilization | 46 °C / 12.95 W / 5% |

The preceding batch-2048 server was stopped before this snapshot. This candidate is a context-scaling probe; success or explicit auto-fit/runtime failure is valid evidence.
