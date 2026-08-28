# CC-003B — Ornith 1.5 / ik_llama.cpp / Q6_K Machine A Optimization

> **Category:** Community Challenge
> **Parent POC:** `001-project-forge-local-ai-compute-hardware-lab`
> **Experiment ID:** `FORGE-CC-003B`
> **Status:** Closed — Machine A capacity/performance recommendation recorded; quality of lower-precision KV cache remains out of scope

## Purpose

Determine the best evidence-backed `Ornith 1.5 35B-A3B Q6_K` / `ik_llama.cpp` configuration for Machine A v2. Unlike CC-003A, this challenge explicitly permits parameter tuning to cross the observed Machine A fit boundary.

## Research Question

What configuration provides the best practical balance of successful load, usable context, GPU/CPU placement, RAM/VRAM pressure, and interactive performance for Ornith 1.5 35B-A3B Q6_K on Machine A v2 using `ik_llama.cpp`?

## Hypothesis

If context size, fit margin, cache types, batching, and placement are tuned in a controlled sequence while model identity and runtime remain fixed, Machine A v2 may load the Q6_K profile at a practical context length and provide measurable interactive behavior. The resulting configuration may require explicit trade-offs in context capacity, placement, or resource headroom.

## Inheritance and Boundary

CC-003B inherits FORGE methodology and references the frozen failure boundary in [CC-003A](../CC-003A-ornith-ik-llama-q6k/). It does not modify or reinterpret CC-003A.

CC-003B may tune documented Machine A parameters. It does not change the model quantization, runtime source commit, Machine A hardware, parent initiative, proposal, ADR, or canonical quality framework unless a separately approved challenge is created.

## Definition of Best

No single throughput maximum is treated as best. A candidate must be compared on the following ordered gates:

1. Starts reliably without OOM or runtime failure.
2. Preserves an explicitly recorded usable context target.
3. Reports stable RAM/VRAM pressure and actual placement.
4. Produces cold-load, prefill, and generation evidence.
5. Is practical for supervised interactive use; quality remains out of scope until separately authorized.

## Artifact Set

- [Execution manifest](execution-manifest.md)
- [Tuning protocol](tuning-protocol.md)
- [Request contracts](request-contracts.md)
- [Results matrix](results-matrix.md)
- [Findings](findings.md)

## Result

CC-003B closed with a repeatable 196,608-token configuration on Machine A v2. The profile loaded again after a clean restart, completed RC-001 without truncation, and had already admitted and retained 165,017 prompt tokens under RC-004 without OOM.

The recommendation is limited to capacity and performance. It does not establish answer-quality equivalence because the selected K/V cache types are `q4_0`/`q4_0` and no separate quality evaluation was authorized.

`N/R` means not recorded or not measured, not zero or a failed result.
