# CC-003B-012 Pre-Request Evidence — 165K Context Utilization

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-012`
> **Request contract:** `RC-004` — Synthetic 165K Context-Utilization Prompt
> **Server configuration:** Unchanged from CC-003B-011 (`-c 196608`, K/V cache `q4_0`, batch/micro-batch 2048)

| Resource | Observed value |
| --- | --- |
| Free physical memory | 24,847.91 MiB |
| GPU VRAM total / used / free | 8,192 / 7,259 / 760 MiB |
| GPU temperature / power / utilization | 52 °C / 12.92 W / 0% |

This snapshot is taken with the 196,608-token server configuration loaded and idle. The following request tests prompt admission and prefill capacity, not response quality or generation throughput.
