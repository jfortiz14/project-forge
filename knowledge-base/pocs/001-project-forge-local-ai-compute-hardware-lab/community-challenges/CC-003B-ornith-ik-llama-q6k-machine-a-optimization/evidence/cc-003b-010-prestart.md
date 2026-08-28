# CC-003B-010 Pre-Request Evidence — 110K Batch-2048 Context Utilization

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-010`
> **Request contract:** `RC-003` — Synthetic 110K Context-Utilization Prompt
> **Server configuration:** Unchanged from CC-003B-009 (`-c 131072`, K/V cache `q4_0`, batch/micro-batch 2048)

| Resource | Observed value |
| --- | --- |
| Free physical memory | 25,680.52 MiB |
| GPU VRAM total / used / free | 8,192 / 7,522 / 497 MiB |
| GPU temperature / power / utilization | 52 °C / 13.97 W / 0% |

This snapshot is taken with the batch-2048, 131,072-token server configuration loaded and idle. The following request tests large-context prompt admission and prefill capacity, not response quality or generation throughput.
