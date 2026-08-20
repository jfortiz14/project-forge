# Evidence: Machine A / Ollama / Qwen3 14B — No-Thinking Validation 004

> **Status:** Successful profile validation; not a throughput benchmark

## Interactive Session Controls

1. `/set nothink`
2. `Reply with exactly: OK`

## Result

Ollama confirmed `Set 'nothink' mode.` The subsequent response was `OK` with no visible `Thinking...` block.

## Decision

The `no-thinking` profile is valid for this Ollama CLI session. Use `/set think` or `/set nothink` before each benchmark session and record the active profile in every result row.

