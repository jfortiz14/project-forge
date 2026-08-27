# Execution Manifest: Experiment 001

> **Experiment ID:** `FORGE-EXP-001`
> **Status:** Completed -- three-run performance baseline captured

## Objective

Capture a three-run cold baseline for `qwen3.5:35B-A3B` on Machine A after
the system-RAM upgrade to 64 GiB, then compare it descriptively with the
accepted 32 GiB CC-002A baseline.

## Frozen Controls

| Control | Required value |
| --- | --- |
| Machine | Machine A desktop; record CPU, GPU, driver, OS, and observed RAM before the first run |
| System RAM | 64 GiB installed; capture the exact observed total in MiB/GiB |
| Source model | `qwen3.5:35B-A3B` |
| Quantization | Q4_K_M, as reported by the installed source model |
| Operational alias | `forge-qwen3-35B-A3B-ctx4096-nothink:latest` |
| Runtime / backend | Ollama with NVIDIA CUDA mixed offload |
| Thinking profile | `no-thinking` (`--think=false`) |
| Context | `4096` |
| Prompt | Exact 131-token CC-002A historical performance prompt below; no edits |
| Run count | Three accepted cold runs |

## Preflight

1. Record date/time, installed Ollama version, Windows version, GPU driver,
   CPU, GPU, and total/available RAM.
2. Verify that the source model and operational alias resolve locally. If a
   replacement alias is required, record the exact Modelfile, model digest,
   and reason; do not silently substitute a model tag.
3. Confirm `ollama ps` does not show the target model. Run `ollama stop` for
   the target if necessary, then confirm it is no longer loaded.
4. Ensure the 131-token historical prompt below is used verbatim.

## Frozen Historical Prompt (131 Tokens)

```text
You are helping evaluate a local language model for software architecture and application development work. Explain the difference between prompt processing (prefill) and token generation in local LLM inference. Give three practical factors that affect each phase. Recommend a small, reproducible benchmarking approach for two Windows computers with different GPUs. Outline a simple, maintainable application-development workflow that uses a local LLM for planning, implementation assistance, tests, and code review. Use clear technical English. Produce 350 to 450 words. Do not use tables, code blocks, citations, or external tools.
```

## Per-Run Procedure

1. Start from the verified unloaded state.
2. In a separate PowerShell window, begin a one-second `nvidia-smi` sample
   loop before starting the model. Preserve the sample containing the highest
   model VRAM use during the run.
3. Execute the operational alias with `--think=false --verbose --keepalive=0`.
4. Submit the exact 131-token historical prompt once, without follow-up
   prompts or interactive configuration changes.
5. Capture the Ollama verbose timings: total duration, load duration, prompt
   evaluation count/duration/rate, and evaluation count/duration/rate.
6. While the model is still loaded, capture `ollama ps` once to record
   processor placement and context. The `--keepalive=0` command may unload
   before a post-run capture, so do not infer placement from an empty result.
7. Enter only observed values in the 64 GiB row for that run. Mark unavailable
   fields as `N/R` and explain why in the notes.
8. Verify the unloaded state before the next run.

## Acceptance and Stop Conditions

Accept a run only when the model tag, no-thinking profile, 4,096 context,
exact 131-token prompt, verbose timing, and post-run placement evidence are
captured.

Stop and record the issue instead of accepting the run if the prompt changes,
thinking is visible, the context differs from 4,096, the model or
quantization differs, or any required timing is ambiguous.

## Interpretation Rules

- Compare prefill and generation separately.
- Report all three values and their range; do not claim causation from a
  single run.
- Treat changes in CPU/GPU placement or VRAM saturation as explanatory
  evidence, not as proof of a quality improvement.
- This experiment measures performance only. It does not rerun or revise the
  CC-002 quality evaluation.
