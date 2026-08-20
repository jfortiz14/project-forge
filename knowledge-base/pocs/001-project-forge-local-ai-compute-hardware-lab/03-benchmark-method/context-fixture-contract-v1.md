# Context Fixture Contract v1

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Status:** Ready for operator creation

## Purpose

Provide a deterministic, non-sensitive long input to compare the same Qwen3/Ollama profile with 4096 and 8192 configured context capacities.

## Content Requirements

- Original synthetic application-development design ledger only.
- No corporate facts, PHI, PII, credentials, private code, or copied external text.
- Repeated fixed sections create roughly 2,000–3,000 English words (approximately a few thousand model tokens; exact token count is measured by Ollama during each run).
- The model is instructed to reply exactly `CONTEXT-OK` so the test emphasizes prefill/context behavior rather than long generation.

## Comparison Rules

- Same fixture file, model, quantization, runtime, thinking profile, and background machine state for both runs.
- Record the actual `prompt eval count` from Ollama, not a word-count estimate.
- Run 4096 first and 8192 second, with one controlled model unload between them.

## Fixture Versions

| Version | Status | Notes |
| --- | --- | --- |
| `v1` | Superseded for comparison | 3,312 approximate words; likely too large to safely reserve output space within a 4096-token window. |
| `v2` | Created | 20 fixed sections; 2,368 approximate words; `C:\Users\pakoo\forge-context-fixture-v2.txt`. Ollama will provide the authoritative prompt-token count. |
