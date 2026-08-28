# CC-003B Closure

> **Experiment ID:** `FORGE-CC-003B`  
> **Status:** Closed and frozen

## Final Answer

Machine A v2 can run the Ornith 1.5 35B-A3B Q6_K / ik_llama.cpp profile at a repeatable configured context of 196,608 tokens under the recorded tuned configuration. It also admitted 165,017 prompt tokens without OOM. This is a capacity/performance result, not autonomous quality approval.

The no-thinking quality sequence did not approve planning, autonomous implementation, generated tests, or code review. Q-002F and Q-003H were separately authorized format/minimal-repair diagnostics; neither alters the failed autonomous units, and each retained a contractual/reference failure.

## Recorded Recommendation

Use the profile only for supervised interactive experimentation where long context is more valuable than the observed 19.11–19.37 generation tokens/s. Do not use it as an autonomous implementation, test-generation, or review authority for the evaluated workload.

## Community Reference Comparison Boundary

Michael Eric Walter Wegener's contributor-reported Machine B observation is approximately 28 generation tokens/s, 567 prefill tokens/s at 242K tokens, 262K configured context, and 17.7% peak VRAM headroom. CC-003B instead observed 19.11–19.37 generation tokens/s and 1,255.92 prefill tokens/s for a 165,017-token request at 196K configured context. These values are not a winner/loser comparison: Machine B has 12 GB VRAM and Linux/CachyOS, while Machine A has 8 GB VRAM and Windows; batch, K-cache precision, fit margin, placement, request size, and timing method also differ.

## Preserved Evidence

- [Results matrix](results-matrix.md)
- [Findings](findings.md)
- [Quality evaluation](quality-evaluation/)
- [Global challenge closure](../community-challenges-closure.md)

No further tuning, quality generation, repair, or rerun is authorized inside CC-003B.
