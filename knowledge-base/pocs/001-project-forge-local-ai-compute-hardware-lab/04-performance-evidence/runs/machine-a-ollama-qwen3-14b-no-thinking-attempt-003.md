# Evidence: Machine A / Ollama / Qwen3 14B — No-Thinking Attempt 003

> **Status:** Control ineffective; excluded from no-thinking benchmark

## Command

`ollama run qwen3:14b "Reply with exactly: OK /no_think"`

## Result

The model emitted visible `Thinking...` content before returning `OK`. Therefore the trailing `/no_think` text in a one-shot CLI command did not establish a no-thinking execution profile in this environment.

## Next Action

Attempt the control as a separate turn in an interactive Ollama session before submitting the benchmark prompt. Do not label a result as `no-thinking` unless visible reasoning is absent and the control method is captured.

