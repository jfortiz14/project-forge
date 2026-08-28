# CC-003B-007 Warm Prompt v1 — Server Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-007`
> **Request contract:** `RC-001` — Benchmark Prompt v1, reasoning enabled
> **Outcome:** HTTP 200; completed without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt evaluation | 1,945.72 ms / 162 tokens / 83.26 tok/s |
| Generation evaluation | 247,715.97 ms / 4,595 tokens / 18.55 tok/s |
| Server total | 249,661.69 ms / 4,757 tokens |
| Server context / retained tokens | 131,072 / 4,756 |
| Truncated | false |

The server released the slot and returned to idle. Generation includes reasoning tokens, so it is not a no-thinking quality or latency measurement.
