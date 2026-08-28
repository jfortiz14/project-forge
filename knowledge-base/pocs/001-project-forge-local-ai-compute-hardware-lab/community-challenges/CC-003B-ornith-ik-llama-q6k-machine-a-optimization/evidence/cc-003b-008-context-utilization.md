# CC-003B-008 Large-Context Utilization Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-008`
> **Request contract:** `RC-003` — Synthetic 110K Context-Utilization Prompt
> **Outcome:** HTTP 200; admitted and retained 110,017 prompt tokens without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt tokens | 110,017 |
| Completion tokens | 1 |
| Total tokens | 110,018 |
| Prompt evaluation | 79,976.62 ms / 110,017 tokens / 1,375.61 tok/s |
| Generation evaluation | 0.00 ms / 1 token |
| Server total | 79,976.62 ms / 110,018 tokens |
| Server context / retained tokens | 131,072 / 110,017 |
| Truncated | false |

The server created a context checkpoint at position 110,016, released the slot, and returned to idle. This validates substantial real prompt admission within the configured 131,072-token context. The synthetic repeated-token prefill throughput is intentionally not compared to RC-001 interactive Prompt v1 throughput.
