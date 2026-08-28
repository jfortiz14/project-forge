# Community Challenges — Closure Record

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Scope:** CC-001, CC-002, CC-003A, and CC-003B  
> **Status:** Closed and frozen

## Closure Decision

The recorded Community Challenge set is closed. All raw outputs, derived artifacts, build/test evidence, performance telemetry, and quality registers remain preserved as historical evidence. No challenge earned autonomous approval for the evaluated Azure/C# workload.

This closure does not prohibit a future, separately identified Community Challenge. A new challenge must define its own frozen configuration, evidence plan, and acceptance boundaries; it must not overwrite these results.

## Final Challenge Outcomes

| Challenge | Capacity / inference outcome | Quality outcome | Closure interpretation |
| --- | --- | --- | --- |
| CC-001 Ornith Q4 | Runnable 4K baseline; observed generation 28.42–31.24 tokens/s. | No autonomous acceptance across planning, implementation, test generation, or review. | Suitable only for supervised experimentation. |
| CC-002 Qwen Q4 | Runnable 4K baseline; observed generation 27.60–30.23 tokens/s. | No autonomous acceptance. Review recognized all four seeded themes but violated review constraints. | Suitable only for supervised experimentation. |
| CC-003A Ornith Q6_K community reproduction | Frozen community configuration did not auto-fit on Machine A. | Not in scope. | Closed reproduction failure; retained as a comparability boundary. |
| CC-003B Ornith Q6_K / ik_llama.cpp | Repeatable 196,608-context profile; 165,017-token prefill completed without OOM at 1,255.92 tokens/s; observed generation 19.11–19.37 tokens/s. Contributor Machine B reported ~262K, ~567 prefill tokens/s at 242K, and ~28 generation tokens/s under a different environment. | No autonomous acceptance. Q-002F and Q-003H were separate human-plus-model diagnostics and still failed their contractual/reference gates. | Best recorded Machine A long-context capacity profile; external contributor figures are a comparability boundary, not a direct ranking. |

## Non-Claims

- The evidence does not establish a permanent model winner.
- The evidence does not measure electricity use, cost per request, reliability over extended service operation, security posture, or production availability.
- Derived or repaired artifacts do not convert their original autonomous model outputs into passes.

## Future Work Boundary

Any continuation requires a new challenge ID and explicit authorization. Candidate scopes may include a production-readiness study, power/cost telemetry, reliability soak testing, or a new quality configuration. None is authorized by this closure record.
