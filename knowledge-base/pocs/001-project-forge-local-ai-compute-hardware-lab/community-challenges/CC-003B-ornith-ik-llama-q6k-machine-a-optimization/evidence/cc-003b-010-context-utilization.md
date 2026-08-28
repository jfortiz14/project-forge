# CC-003B-010 Large-Context Utilization Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-010`
> **Request contract:** `RC-003` — Synthetic 110K Context-Utilization Prompt
> **Outcome:** HTTP 200; admitted and retained 110,017 prompt tokens without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt tokens | 110,017 |
| Completion tokens | 1 |
| Total tokens | 110,018 |
| Prompt evaluation | 75,626.16 ms / 110,017 tokens / 1,454.75 tok/s |
| Generation evaluation | 0.00 ms / 1 token |
| Server total | 75,626.17 ms / 110,018 tokens |
| Server context / retained tokens | 131,072 / 110,017 |
| Truncated | false |

The server erased an old checkpoint, created its 32nd checkpoint at position 110,016, released the slot, and returned to idle. This validates substantial real prompt admission for the batch-2048 profile. The synthetic repeated-token prefill throughput is intentionally not compared to RC-001 interactive Prompt v1 throughput.
