# CC-003B-007 Post-Prompt Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-007`
> **State:** RC-001 complete; server loaded and idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 22,990.73 MiB |
| GPU VRAM total / used / free | 8,192 / 7,779 / 240 MiB |
| GPU temperature / power / utilization | 53 °C / 13.77 W / 1% |

The server remained loaded and idle after the bounded RC-001 request. No OOM or runtime failure was reported. The point-in-time 240 MiB free VRAM reading reinforces that the 131,072-token configuration is feasible but narrow under the tested WDDM environment.
