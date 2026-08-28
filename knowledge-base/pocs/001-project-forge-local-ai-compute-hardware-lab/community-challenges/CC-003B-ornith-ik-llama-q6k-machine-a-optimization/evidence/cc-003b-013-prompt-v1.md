# CC-003B-013 Clean-Restart RC-001 Evidence

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-013`
> **Request contract:** `RC-001` — Benchmark Prompt v1, reasoning enabled
> **Outcome:** HTTP 200; completed without truncation after clean restart

| Measure | Observed value |
| --- | ---: |
| Prompt tokens | 162 |
| Completion tokens | 4,615 |
| Total tokens | 4,777 |
| Cached prompt tokens | 0 |
| Client elapsed time | 241.282 s |
| Prompt evaluation | 1,940.94 ms / 162 tokens / 83.46 tok/s |
| Generation evaluation | 238,235.40 ms / 4,615 tokens / 19.37 tok/s |
| Server total | 240,176.34 ms / 4,777 tokens |
| Server context / retained tokens | 196,608 / 4,776 |
| Truncated | false |
| Post-request free physical memory | 25,328.49 MiB |
| Post-request VRAM total / used / free | 8,192 / 7,208 / 811 MiB |

The server released the slot and returned to idle. The clean restart reproduced server readiness, placement, and bounded RC-001 behavior. Generation includes reasoning tokens, so this is not a no-thinking quality or latency measurement.
