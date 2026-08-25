# FORGE Quality Run Manifest v1

> Copy this file to `model-evaluations/<run-id>/manifest.md` before execution.

| Field | Value |
| --- | --- |
| Run ID | N/R |
| Model alias / source / quantization | N/R |
| Runtime / backend / context / thinking | N/R |
| Protocol and fixture version | `quality-evaluation-protocol-v1` / `azure-csharp-domain-v1` |
| Prompt ID and SHA-256 | N/R |
| Raw output path and SHA-256 | N/R |
| Build command/result | N/R |
| Test command/result | N/R |
| Mutant detection | N/R |
| Human review | N/R |
| Autonomous verdict | N/R |

## Evidence Rules

- Preserve model standard output before parsing or editing it.
- Record `N/R` rather than inferred values.
- Keep derived/fence-only artifacts separate from raw evidence.
- Describe human edits by exact line/change count and separate them from autonomous results.
