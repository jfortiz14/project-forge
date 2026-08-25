# CC-001A-independent-baseline

> **Category:** Community Challenge baseline
> **Parent:** `CC-001-ornith`
> **Experiment ID:** `FORGE-CC-001A`
> **Status:** Protocol frozen; no measurements yet

## Purpose

Establish a frozen, FORGE-comparable baseline for Ornith 1.5 35B-A3B before any community-recommended configuration is tested.

This slice does not restart the FORGE initiative. It inherits the original Machine A methodology, benchmark contract, and quality contract where applicable, and it records only the minimum experiment-specific additions needed to keep the run reproducible.

## Model Source vs Operational Alias

- **Source model:** `hf.co/ornith-ai/Ornith-1.5-35B-A3B-GGUF:Q4_K_M`
- **FORGE operational alias:** `forge-ornith-35B-A3B-ctx4096-nothink`

The source model must be pulled or imported first. The FORGE alias is what the baseline run uses locally after the source model is available.

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

- exact source-model acquisition method
- exact runtime/backend to use
- exact thinking profile
- exact prompt variant to run
- exact context target
- exact output boundary for the quality unit, if applicable
- exact command form for the operator run

## Execution Guardrails

- Do not introduce a new prompt family unless it is explicitly derived from FORGE and documented as such.
- Do not change the benchmark contract just to make Ornith look better or easier to run.
- Do not record any result until the command, model, profile, and context are frozen.
- If a direct comparison is not possible, record the incompatibility instead of changing the method.

## Immediate Next Step

Define the exact frozen command set for the first baseline run and verify it against the original FORGE documents before any measurement is taken.
