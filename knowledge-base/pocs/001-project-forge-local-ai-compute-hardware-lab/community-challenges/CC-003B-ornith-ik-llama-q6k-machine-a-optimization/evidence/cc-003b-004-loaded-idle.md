# CC-003B-004 Loaded-Idle Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-004`
> **State:** Model loaded; HTTP server idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 24,361.14 MiB |
| GPU VRAM total / used / free | 8,192 / 7,756 / 263 MiB |
| GPU temperature / power / utilization | 46 °C / 13.18 W / 4% |

This is the lowest observed idle VRAM headroom among the successful CC-003B context candidates. It remains loaded, but the configuration must complete the bounded warm request without OOM before it can be considered usable.
