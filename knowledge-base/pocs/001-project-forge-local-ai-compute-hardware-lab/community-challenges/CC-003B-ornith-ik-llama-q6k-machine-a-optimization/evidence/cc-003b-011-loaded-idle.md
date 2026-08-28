# CC-003B-011 Loaded-Idle Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-011`
> **State:** Model loaded; HTTP server idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 25,485.59 MiB |
| GPU VRAM total / used / free | 8,192 / 7,103 / 916 MiB |
| GPU temperature / power / utilization | 46 °C / 12.96 W / 0% |

This is the largest observed loaded-idle VRAM headroom among the successful CC-003B candidates. It is consistent with the automatic shift of an additional expert layer to host memory, but must be validated under request workload.
