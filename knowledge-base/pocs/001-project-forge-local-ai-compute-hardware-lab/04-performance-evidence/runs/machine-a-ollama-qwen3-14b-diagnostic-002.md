# Evidence: Machine A / Ollama / Qwen3 14B — Diagnostic 002

> **Status:** Successful runtime validation; not a throughput benchmark

## Command

`ollama run qwen3:14b "Reply with exactly: OK"`

## Observed Result

- The model produced visible reasoning text followed by the requested final output: `OK`.
- `ollama ps` reported `qwen3:14b`, size 10 GB, context 4096, and `37%/63% CPU/GPU` placement.

## Interpretation

Ollama and the Qwen3 model are operational on Machine A. The model's default reasoning behavior is enabled in this interactive path. This is a functional validation only: it has no valid load, prefill, or generation timing fields.

## Benchmark Implication

The next benchmark contract must explicitly select a reasoning-mode policy. A default-thinking benchmark and a no-thinking benchmark are different workloads and must not be compared as if they are identical.

