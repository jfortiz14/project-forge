# Model Registry: Project FORGE — Local AI Compute & Hardware Lab

> **Scope:** Machine A — personal desktop

## Baseline Models

| Model | Tier | Quantization | Installed evidence | Benchmark status |
| --- | --- | --- | --- | --- |
| `qwen3:8b-q4_K_M` | SMALL | Q4_K_M | Machine A; ID `500a1f067a9f`, 5.2 GB | Completed — A-009, A-011, A-012 |
| `qwen3:14b` | MEDIUM | Q4_K_M | Machine A; ID `bdbd181c33f2`, 9.3 GB | Completed — A-001 through A-008 |
| `qwen3:32b-q4_K_M` | LARGE | Q4_K_M | Machine A; ID `030ee887880f`, 20 GB | Completed — A-010 |

## Extra Comparison Tests

| ID | Model | Quantization | Status |
| --- | --- | --- | --- |
| X-001 / X-002 | Llama 3.1 8B Instruct | Q4_K_M | Completed on Machine A |
| X-003 | Ministral 3 8B Instruct 25.12 | Q4_K_M | Completed on Machine A |

## Reproducibility Rules

- Record runtime version, context, prompt variant, and all timing fields for every run.
- Do not substitute a tag, model revision, or quantization without a separate evidence record.
- Record Qwen3 reasoning-mode behavior for every run.
