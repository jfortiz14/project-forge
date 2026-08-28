# CC-003B-008 Pre-Request Evidence — 110K Context Utilization

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-008`
> **Request contract:** `RC-003` — Synthetic 110K Context-Utilization Prompt
> **Server configuration:** Unchanged from CC-003B-007 (`-c 131072`, K/V cache `q4_0`)

| Resource | Observed value |
| --- | --- |
| Free physical memory | 22,902.36 MiB |
| GPU VRAM total / used / free | 8,192 / 7,654 / 365 MiB |
| GPU temperature / power / utilization | 51 °C / 13.85 W / 1% |

This snapshot is taken with the 131,072-token server configuration loaded and idle. The following request tests prompt admission and prefill capacity, not response quality or generation throughput.
