# CC-003B-008 Post-Request Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-008`
> **State:** RC-003 complete; server loaded and idle

| Resource | Observed value |
| --- | --- |
| Free physical memory | 21,312.43 MiB |
| GPU VRAM total / used / free | 8,192 / 7,778 / 241 MiB |
| GPU temperature / power / utilization | 48 °C / 13.57 W / 2% |

The server remained loaded and idle after retaining 110,017 prompt tokens. No OOM or runtime failure was reported. This confirms substantial real-context use under the 131,072-token / q4 K-cache configuration, with narrow VRAM headroom.
