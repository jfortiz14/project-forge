# CC-003B-009 Warm Prompt v1 — Server Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-009`
> **Request contract:** `RC-001` — Benchmark Prompt v1, reasoning enabled
> **Outcome:** HTTP 200; completed without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt evaluation | 1,240.44 ms / 162 tokens / 130.60 tok/s |
| Generation evaluation | 228,678.33 ms / 4,569 tokens / 19.98 tok/s |
| Server total | 229,918.77 ms / 4,731 tokens |
| Server context / retained tokens | 131,072 / 4,730 |
| Truncated | false |

The server released the slot and returned to idle. Generation includes reasoning tokens, so it is not a no-thinking quality or latency measurement.
