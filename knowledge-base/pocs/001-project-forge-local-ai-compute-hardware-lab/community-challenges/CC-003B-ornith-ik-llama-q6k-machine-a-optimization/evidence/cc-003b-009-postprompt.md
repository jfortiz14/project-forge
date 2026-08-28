# CC-003B-009 Post-Prompt Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-009`
> **State:** RC-001 complete; server loaded and idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 25,748.02 MiB |
| GPU VRAM total / used / free | 8,192 / 7,550 / 469 MiB |
| GPU temperature / power / utilization | 54 °C / 15.83 W / 24% |

The server remained loaded and idle after RC-001 with no OOM or runtime failure. This point-in-time VRAM reading is higher than CC-003B-007's post-request measurement, but the 31 MiB startup fit estimate remains the relevant resilience boundary.
