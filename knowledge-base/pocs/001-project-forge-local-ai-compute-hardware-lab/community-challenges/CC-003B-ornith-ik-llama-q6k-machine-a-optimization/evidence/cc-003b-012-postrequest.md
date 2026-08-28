# CC-003B-012 Post-Request Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-012`
> **State:** RC-004 complete; server loaded and idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 23,033.13 MiB |
| GPU VRAM total / used / free | 8,192 / 7,754 / 265 MiB |
| GPU temperature / power / utilization | 55 °C / 14.77 W / 0% |

The server remained loaded and idle after retaining 165,017 prompt tokens. No OOM or runtime failure was reported. This demonstrates substantial real-context use under the 196,608-token profile, with narrower post-large-context VRAM headroom than its bounded interactive request.
