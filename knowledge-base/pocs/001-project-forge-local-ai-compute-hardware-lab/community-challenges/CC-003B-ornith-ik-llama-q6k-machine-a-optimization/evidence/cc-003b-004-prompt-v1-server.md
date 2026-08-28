# CC-003B-004 Warm Prompt v1 — Server Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-004`
> **Request:** Benchmark contract Prompt v1, reasoning enabled
> **Outcome:** HTTP 200; completed without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt evaluation | 2,004.83 ms / 162 tokens / 80.80 tok/s |
| Generation evaluation | 243,746.25 ms / 4,584 tokens / 18.81 tok/s |
| Server total | 245,751.07 ms / 4,746 tokens |
| Server context / retained tokens | 98,304 / 4,745 |
| Truncated | false |

The server released the slot and returned to idle state. Generation includes reasoning tokens, so it is not a no-thinking quality or latency measurement.
