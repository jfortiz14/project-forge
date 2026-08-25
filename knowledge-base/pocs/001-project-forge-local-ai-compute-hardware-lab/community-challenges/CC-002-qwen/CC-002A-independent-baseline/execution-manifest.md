# CC-002A Independent Baseline Execution Manifest

> **Experiment:** `FORGE-CC-002A`
> **Challenge:** `CC-002-qwen`
> **Parent POC:** `001-project-forge-local-ai-compute-hardware-lab`
> **Status:** Frozen, not yet executed

## Objective

Capture a FORGE-comparable performance baseline for `qwen3.5:35B-A3B` on Machine A using the original FORGE benchmarking style, without changing the prompt or format for model convenience.

## Model Source vs Operational Alias

- **Source model:** `qwen3.5:35B-A3B`
- **FORGE operational alias:** `forge-qwen35-35B-A3B-ctx4096-nothink`

The source model is downloaded or imported first. The FORGE alias is the local name used for the performance run after the source model exists.

## Fixed Rules

- Use the same FORGE-style prompt family as the original POC.
- Do not modify the prompt to suit Qwen.
- Do not mix performance baseline execution with quality evaluation.
- Run performance first, quality later.
- Keep prefill and generation metrics separate.
- Record only observed evidence.
- Use only synthetic, public, or personally authored non-sensitive inputs.

## Frozen Runtime Command

```powershell
ollama run qwen3.5:35B-A3B
/set verbose
/set parameter num_ctx 4096
/save forge-qwen3-35B-A3B-ctx4096-nothink
/bye
ollama stop qwen3.5:35B-A3B
ollama run forge-qwen3-35B-A3B-ctx4096-nothink --think=false --verbose --keepalive=0
```

## Frozen Baseline Parameters

- Runtime/backend: Ollama
- Source model: `qwen3.5:35B-A3B`
- Operational alias: `forge-qwen35-35B-A3B-ctx4096-nothink`
- Thinking profile: `no-thinking`
- Context target: `4096`
- Measurement style: FORGE original benchmark format
- Prompt family: same style as `Benchmark Contract v1`

## Execution Order

1. Confirm the model is not already loaded with `ollama ps`.
2. Stop the target model if needed.
3. Run the frozen benchmark command.
4. Capture prompt-processing rate, generation rate, load time, context, and backend details.
5. Record the result in the `CC-002A` results artifact.
6. Do not proceed to quality evaluation until this performance baseline is documented.

## Stop Conditions

- The prompt format changes.
- The command changes for model convenience.
- The model tag changes.
- Reasoning mode is not explicitly `no-thinking`.
- The run mixes quality evaluation into the performance baseline.
- Any observed evidence is missing or ambiguous.

## Evidence Requirements

- Exact command used
- Prompt family reference
- Model tag
- Runtime/backend
- Context
- Load timing
- Prompt-processing rate
- Generation rate
- Operator notes
- Any caveats about comparability

## Next Artifact

Create the results record only after the first execution is completed and observed evidence is available.
