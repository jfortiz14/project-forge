# CC-003B-003 Prompt v1 Server Timing Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-003`
> **Workload:** FORGE Benchmark Contract Prompt v1, warm request

| Measurement | Observed value |
| --- | --- |
| Prompt evaluation | 1,981.89 ms / 162 tokens / 81.74 tok/s |
| Generation evaluation | 245,030.16 ms / 4,668 tokens / 19.05 tok/s |
| Server total | 247,012.05 ms / 4,830 tokens |
| HTTP result | 200 |
| Slot state after request | Released and idle |
| Slot context | 65,536 |
| Tokens retained in slot | 4,829 |
| Context truncated | No |

The completion includes reasoning tokens. The same Prompt v1 produced 15 more completion tokens than CC-003B-002, so throughput comparison is directional rather than a fixed-output latency verdict.
