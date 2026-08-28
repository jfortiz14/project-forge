# CC-003B-007 Pre-Start Evidence — 131K K-Cache Trade-Off

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-007`
> **Parent:** `CC-003B-006`
> **Changed parameter:** `-ctk q8_0` to `-ctk q4_0` only; retain `-c 131072`

| Resource | Observed value |
| --- | --- |
| Free physical memory | 52,029.72 MiB |
| GPU VRAM total / used / free | 8,192 / 1,013 / 7,006 MiB |
| GPU temperature / power / utilization | 45 °C / 13.02 W / 2% |

This candidate isolates a K-cache precision change intended to test the 131,072-token startup boundary. Any inference-quality implications of the lower K-cache precision are outside the current challenge scope.
