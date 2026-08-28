# CC-003B-005 Large-Context Utilization Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-005`
> **Request contract:** `RC-002` — Synthetic 85K Context-Utilization Prompt
> **Outcome:** HTTP 200; admitted and retained 85,017 prompt tokens without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt tokens | 85,017 |
| Completion tokens | 1 |
| Total tokens | 85,018 |
| Cached prompt tokens | 0 |
| Client elapsed time | 71.824 s |
| Prompt evaluation | 71,416.48 ms / 85,017 tokens / 1,190.44 tok/s |
| Generation evaluation | 0.00 ms / 1 token |
| Server total | 71,416.49 ms / 85,018 tokens |
| Server context / retained tokens | 98,304 / 85,017 |
| Truncated | false |

The server created a context checkpoint at position 85,016, released the slot, and returned to idle. This validates substantial real prompt admission within the configured 98,304-token context. The synthetic repeated-token prefill throughput is intentionally not compared to RC-001 interactive Prompt v1 throughput.
