# CC-003B-010 Post-Request Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-010`
> **State:** RC-003 complete; server loaded and idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 23,323.69 MiB |
| GPU VRAM total / used / free | 8,192 / 7,805 / 214 MiB |
| GPU temperature / power / utilization | 47 °C / 13.40 W / 1% |

The server remained loaded and idle after retaining 110,017 prompt tokens. No OOM or runtime failure was reported. The 214 MiB free VRAM snapshot is lower than CC-003B-008's 241 MiB under the batch-4096 profile, reinforcing the narrower 31 MiB startup fit estimate.
