# CC-003B-012 Large-Context Utilization Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-012`
> **Request contract:** `RC-004` — Synthetic 165K Context-Utilization Prompt
> **Outcome:** HTTP 200; admitted and retained 165,017 prompt tokens without truncation

| Measure | Observed value |
| --- | ---: |
| Prompt tokens | 165,017 |
| Completion tokens | 1 |
| Total tokens | 165,018 |
| Cached prompt tokens | 0 |
| Client elapsed time | 131.877 s |
| Prompt evaluation | 131,391.70 ms / 165,017 tokens / 1,255.92 tok/s |
| Generation evaluation | 0.00 ms / 1 token |
| Server total | 131,391.70 ms / 165,018 tokens |
| Server context / retained tokens | 196,608 / 165,017 |
| Truncated | false |

The server erased an old checkpoint, created its 32nd checkpoint at position 165,016, released the slot, and returned to idle. This validates substantial real prompt admission within the configured 196,608-token context. The synthetic repeated-token prefill throughput is intentionally not compared to RC-001 interactive Prompt v1 throughput.
