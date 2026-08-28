# CC-003B-005 Post-Request Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-005`
> **State:** RC-002 complete; server loaded and idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 21,294.44 MiB |
| GPU VRAM total / used / free | 8,192 / 7,766 / 253 MiB |
| GPU temperature / power / utilization | 49 °C / 13.62 W / 1% |

The server remained loaded and idle after retaining 85,017 prompt tokens. No OOM or runtime failure was reported. The 253 MiB free VRAM snapshot confirms that the 98,304-token configuration operates with narrow resource headroom during substantial-context use.
