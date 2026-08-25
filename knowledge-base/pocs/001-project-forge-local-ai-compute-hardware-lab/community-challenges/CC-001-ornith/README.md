# CC-001-ornith

> **Category:** Community Challenge  
> **Parent POC:** `001-project-forge-local-ai-compute-hardware-lab`  
> **Experiment ID:** `FORGE-CC-001`

## Research Question

Can a Mixture-of-Experts model change the local AI performance and quality equation observed during Project FORGE?

More specifically, can Ornith 1.5 35B-A3B provide useful interactive performance and software-engineering quality on the same FORGE baseline, while remaining comparable to the existing Machine A methodology?

## Inheritance

This experiment inherits:

- the FORGE hardware baseline and Machine A evidence model
- the benchmark contract and telemetry expectations
- the quality contract for software-architecture and coding tasks
- the operating rules that prohibit synthetic claims from replacing observed evidence

## Model Source vs Operational Alias

- **Source model:** `hf.co/ornith-ai/Ornith-1.5-35B-A3B-GGUF:Q4_K_M`
- **FORGE operational alias:** `forge-ornith-35B-A3B-ctx4096-nothink`

The source model is the artifact that must be downloaded or imported. The FORGE alias is the local experimental name used after the source model is available.

## Experiment Boundary

- `CC-001A-independent-baseline` establishes the frozen independent comparison point.
- No CC-001B community configuration is currently defined.

## Execution Rule

Do not begin `CC-001A` until the frozen baseline protocol is documented and explicitly reviewed against the original FORGE benchmark and quality contracts.

## Recorded Artifact Set

- research question
- hypothesis
- frozen configuration
- execution manifest
- raw results
- findings

## Status

Completed. CC-001A baseline evidence, results, and quality artifacts are recorded in the linked challenge materials.
