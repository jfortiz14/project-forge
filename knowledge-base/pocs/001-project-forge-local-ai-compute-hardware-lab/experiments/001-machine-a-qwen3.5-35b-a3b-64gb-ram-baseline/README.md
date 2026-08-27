# Experiment 001: Machine A / Qwen3.5 35B-A3B / 64 GiB RAM Baseline

> **Status:** Completed -- three-run performance baseline captured
> **Parent POC:** `001-project-forge-local-ai-compute-hardware-lab`
> **Historical comparison:** `CC-002A-independent-baseline`

## Research Question

With Machine A upgraded from 32 GiB to 64 GiB of system RAM, how does
`qwen3.5:35B-A3B` Q4_K_M perform under the same no-thinking, 4,096-context
FORGE benchmark configuration used by CC-002A?

## Comparison Boundary

The intended independent variable is installed system RAM. The experiment
reuses the historical model, quantization, backend, context target, thinking
profile, and the 131-token CC-002A performance prompt. It records observed
differences in load time, prefill, generation, RAM/VRAM use, CPU/GPU placement,
and operator experience. It does not change the historical CC-002A evidence or
make a hardware-procurement decision.

## Artifacts

- [Execution manifest](execution-manifest.md) -- frozen controls and run protocol.
- [Results matrix](results-matrix.md) -- 32 GiB reference and empty 64 GiB rows.

## Historical Reference

CC-002A ran `qwen3.5:35B-A3B` Q4_K_M with 32 GiB RAM, 4,096 context, and the
no-thinking profile. Its three observed generation rates were 27.60, 30.23,
and 29.62 tokens/s. The source evidence remains authoritative at
`../../community-challenges/CC-002-qwen/CC-002A-independent-baseline/`.
