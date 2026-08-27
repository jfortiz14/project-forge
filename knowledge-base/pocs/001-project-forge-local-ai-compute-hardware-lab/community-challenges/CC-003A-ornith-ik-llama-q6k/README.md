# CC-003A — Ornith 1.5 / ik_llama.cpp / Q6_K Community Configuration Reproduction

> **Category:** Community Challenge  
> **Parent POC:** `001-project-forge-local-ai-compute-hardware-lab`  
> **Experiment ID:** `FORGE-CC-003A`  
> **Status:** Closed — reproduction failed; artifacts frozen

## Purpose

Reproduce, as faithfully as practical, the Ornith 1.5 community configuration contributed by Michael Eric Walter Wegener in his FORGE fork. This is a reproduction attempt first; it is not an optimization exercise for Machine A.

## Research Question

Can Machine A v2 reproduce the observed behavior of the community-provided Ornith 1.5 35B-A3B Q6_K configuration using ik_llama.cpp, despite the hardware and OS differences between Machine A and the contributor's Machine B?

## Objective

The first objective is not to beat the community result. Record observed evidence sufficient to answer:

1. Can the same Q6_K model be loaded on Machine A v2?
2. Can the community shipped configuration start without OOM or runtime failure?
3. What placement does ik_llama.cpp choose?
4. What RAM and VRAM pressure is observed?
5. What cold-load, prefill, and generation behavior is observed?
6. Can the configured 262K context be allocated?
7. If practical, can a large-context stress run complete without OOM?

## Inheritance and Boundary

This challenge inherits the FORGE Community Challenge structure, the reusable performance methodology, and the canonical quality framework where a later quality phase is authorized. It does not restart the parent initiative, proposal, ADR lifecycle, original POC baseline, or quality framework.

CC-003A is limited to configuration reproduction and resource/performance observation. It does not make a quality claim, optimize flags for Machine A, or alter the frozen community reference. Any later Machine A tuning must be a separately identified follow-up experiment.

## Artifact Set

- [Community reference and comparability boundaries](community-reference.md)
- [Execution manifest](execution-manifest.md)
- [Results matrix](results-matrix.md)
- [Closure decision](closure.md)

## Status

CC-003A is closed as a failed reproduction attempt. CC-003A-001 and CC-003A-002 reached CUDA initialization and valid model metadata parsing, but did not load model tensors or reach server readiness: `--fit` terminated with `Unable to auto-fit model` in both runs. The second run reduced background GPU load without changing a frozen parameter. The challenge artifacts are frozen; do not append retries, tuning, quality work, or altered configurations under `CC-003A`. See [closure](closure.md), [CC-003A-001](evidence/cc-003a-001-startup.md), [CC-003A-002](evidence/cc-003a-002-startup.md), and the [results matrix](results-matrix.md). `N/R` means not recorded, not zero or a failed result.
