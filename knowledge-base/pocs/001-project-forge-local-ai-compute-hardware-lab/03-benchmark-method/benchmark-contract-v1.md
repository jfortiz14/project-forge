# Benchmark Contract v1: Project FORGE — Local AI Compute & Hardware Lab

> **Status:** Approved by operator, 2026-08-15
> **Scope:** Common non-sensitive prompt for architecture and application-development evaluation

## Prompt v1

```text
You are helping evaluate a local language model for software architecture and application development work.

Task:
1. Explain the difference between prompt processing (prefill) and token generation in local LLM inference.
2. Explain exactly three factors that affect prefill performance.
3. Explain exactly three factors that affect token-generation performance.
4. Describe a reproducible benchmarking procedure for evaluating a Windows desktop with a local GPU.
5. Describe a local-LLM software-development workflow covering planning, implementation assistance, testing, and code review.

Constraints:
- Use clear technical English.
- Produce exactly 350 to 450 words.
- Do not use tables, code blocks, citations, or external tools.
```

## Measurement Rules

- Record prompt version `v1`, exact runtime command/configuration, context size, model, quantization, and backend.
- Measure and store prefill/prompt-processing rate separately from generation rate.
- Use this unchanged prompt for every directly comparable run.
- The prompt contains no corporate data, PHI, secrets, credentials, or private source code.

## Qwen3 Execution Profiles

The operator approved both profiles. They are separate workloads and must be labelled in every result row.

| Profile | Prompt control | Purpose |
| --- | --- | --- |
| `thinking` | `/set think` | Complex deliberative architecture and development work |
| `no-thinking` | `/set nothink` | Direct-response architecture and application-development work |

Set the profile in the Ollama interactive session before submitting the benchmark prompt. Do not average or directly rank throughput across profiles without reporting the profile, because reasoning token volume changes latency and generation work.

### Implementation Caveat

Attempt 003 showed that appending `/no_think` to a one-shot CLI prompt did not disable visible reasoning. Interactive CLI help confirmed the supported control is `/set nothink`; establish it before the benchmark prompt and verify the next response contains no visible reasoning before accepting a `no-thinking` result.

Validation 004 confirmed `/set nothink` is effective in the interactive CLI session.

## Preferred One-Shot CLI Form

For reproducible timing runs, use the installed Ollama CLI flags rather than an interactive session:

- `--think=false` for the `no-thinking` profile.
- `--think=true` for the `thinking` profile.
- `--verbose` to print load, prompt-evaluation, and evaluation timings.
- `--keepalive=0` when the model must unload after a run.

For a cold run, execute `ollama stop <model>` first, confirm `ollama ps` is empty, then run the one-shot command. This avoids loading the model before the benchmark prompt is sent.

### Prompt Variant Registry

| ID | Description | Directly comparable with |
| --- | --- | --- |
| `v1` | Approved original multiline prompt | Other exact `v1` runs only |
| `v1-cli-cold` | One-shot CLI wording used for A-004; same task intent but condensed to 136 prompt tokens | Cold-load behavior and qualitative development workload only; not exact prefill comparison with `v1` |
- Do not modify the prompt to accommodate a specific model.
