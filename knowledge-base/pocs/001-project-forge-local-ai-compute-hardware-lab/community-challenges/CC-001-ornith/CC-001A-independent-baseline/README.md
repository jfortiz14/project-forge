# CC-001A-independent-baseline

> **Category:** Community Challenge baseline
> **Parent:** `CC-001-ornith`
> **Experiment ID:** `FORGE-CC-001A`
> **Status:** Baseline captured; results and quality evaluation recorded

## Purpose

Establish and record the FORGE-comparable baseline for Ornith 1.5 35B-A3B on Machine A.

This slice does not restart the FORGE initiative. It inherits the original Machine A methodology, benchmark contract, and quality contract where applicable, and it records the executed baseline, the associated results, and the completed quality evaluation.

## Model Source vs Operational Alias

- **Source model:** `hf.co/ornith-ai/Ornith-1.5-35B-A3B-GGUF:Q4_K_M`
- **FORGE operational alias:** `forge-ornith-35B-A3B-ctx4096-nothink`

The source model was pulled or imported first. The FORGE alias is the local experimental name used for the captured baseline run.

## Inherited FORGE Rules

- Use only synthetic, public, or personally authored non-sensitive inputs.
- The operator runs the commands and pastes back the observed output.
- Record only observed evidence, not inferred performance.
- Keep prefill and generation metrics separate.
- Do not compare results across runs unless the prompt, profile, model, quantization, runtime, and context rules are compatible.

## Frozen Baseline Question

Can Ornith 1.5 35B-A3B provide useful interactive performance and software-engineering quality on Machine A under the same style of controlled evaluation used in FORGE?

## Frozen Comparison Frame

The baseline will be compared against the original FORGE methodology, not against a newly invented prompt or scoring system.

The following elements remain unchanged unless the original FORGE documents already allowed a controlled variation:

- same machine family: Machine A desktop
- same public/synthetic non-sensitive policy
- same emphasis on load, prefill, generation, memory, placement, and usability
- same quality discipline for software-architecture and coding evaluation

## Experiment-Specific Variables

- Model under test: `Ornith 1.5 35B-A3B`
- Experiment family: `FORGE-CC-001`
- Baseline slice: `CC-001A-independent-baseline`

## Open Inputs That Must Be Fixed Before Execution

The baseline run has already been executed, so these inputs are retained only as historical setup context in the linked artifacts.

## Execution Guardrails

- The captured artifacts preserve the original prompt family and benchmarking contract.
- Any comparability caveats remain documented in the results and quality materials.

## Immediate Next Step

Review the recorded results and quality evaluation artifacts if a cross-challenge comparison is needed.
