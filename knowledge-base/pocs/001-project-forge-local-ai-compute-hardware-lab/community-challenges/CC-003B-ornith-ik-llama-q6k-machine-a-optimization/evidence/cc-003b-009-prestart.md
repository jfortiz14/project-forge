# CC-003B-009 Pre-Start Evidence — 131K Batch Trade-Off

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-009`
> **Parent:** `CC-003B-007`
> **Changed parameters:** `-b 4096 -ub 4096` to `-b 2048 -ub 2048` only

| Resource | Observed value |
| --- | --- |
| Free physical memory | 52,374.96 MiB |
| GPU VRAM total / used / free | 8,192 / 1,012 / 7,007 MiB |
| GPU temperature / power / utilization | 45 °C / 13.20 W / 0% |

The preceding 131K server was stopped before this snapshot. This candidate keeps 131,072 context and K/V cache `q4_0` while reducing batch and micro-batch size only.
