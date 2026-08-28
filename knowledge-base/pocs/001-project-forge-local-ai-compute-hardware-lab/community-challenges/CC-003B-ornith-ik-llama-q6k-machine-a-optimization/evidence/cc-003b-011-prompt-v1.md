# CC-003B-011 Warm Prompt v1 Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-011`
> **Request contract:** `RC-001` — Benchmark Prompt v1, reasoning enabled
> **Outcome:** HTTP 200; completed without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt tokens | 162 |
| Completion tokens | 4,660 |
| Total tokens | 4,822 |
| Cached prompt tokens | 0 |
| Client elapsed time | 245.961 s |
| Prompt evaluation | 2,062.76 ms / 162 tokens / 78.54 tok/s |
| Generation evaluation | 243,836.85 ms / 4,660 tokens / 19.11 tok/s |
| Server total | 245,899.61 ms / 4,822 tokens |
| Server context / retained tokens | 196,608 / 4,821 |
| Truncated | false |

The server released the slot and returned to idle. Generation includes reasoning tokens, so the timing is not a no-thinking quality or latency measurement.
