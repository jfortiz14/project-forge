# CC-003B-002 Prompt v1 Server Timing Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-002`
> **Workload:** FORGE Benchmark Contract Prompt v1, warm request

| Measurement | Observed value |
| --- | --- |
| Prompt evaluation | 2,178.34 ms / 162 tokens / 74.37 tok/s |
| Generation evaluation | 223,353.09 ms / 4,653 tokens / 20.83 tok/s |
| Server total | 225,531.42 ms / 4,815 tokens |
| HTTP result | 200 |
| Slot state after request | Released and idle |
| Slot context | 32,768 |
| Tokens retained in slot | 4,814 |
| Context truncated | No |

The completion includes reasoning tokens. These server timings are valid for the documented warm, reasoning-enabled request and are not directly comparable to no-thinking profiles or runs with a different prompt/token count.
