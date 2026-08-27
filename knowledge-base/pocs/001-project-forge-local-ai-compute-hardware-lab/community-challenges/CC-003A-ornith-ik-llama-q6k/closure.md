# CC-003A Closure — Failed Community Configuration Reproduction

> **Experiment:** `FORGE-CC-003A`  
> **Status:** Closed — failure retained; artifacts frozen

## Decision

Close CC-003A as a failed practical reproduction of the frozen community Ornith 1.5 35B-A3B Q6_K / ik_llama.cpp configuration on Machine A v2.

## Evidence Basis

- The local runtime matched the requested `ik_llama.cpp` source commit `0ed847d` (build 4848).
- The verified Ornith Q6_K model and BF16 `mmproj` artifacts were used.
- Both runs used the frozen community configuration, including 262144 context, `CUDA0`, cache types, reasoning options, `--fit`, and `--fit-margin 3030`.
- CC-003A-001 reached CUDA initialization and valid GGUF metadata parsing, then failed with `Unable to auto-fit model` before tensor loading or server readiness.
- CC-003A-002 retained all frozen parameters and reduced only transient background GPU load. It produced the same fit calculation and the same failure.

## Answer to the Research Question

**No, not under the two observed Machine A v2 states.** Machine A v2 could reproduce the runtime commit, model identity, CUDA discovery, and initial metadata parsing, but it could not reproduce a successful start of the community configuration. The runtime reported that even after CPU overrides for all MoE experts, device memory required remained greater than device memory available for auto-fit.

This answer applies only to the documented Machine A v2 environment and the frozen configuration. It is not a finding about the validity of the contributor's Machine B result.

## Unreached Objectives

Because server readiness was not achieved, the following remain `N/R`:

- complete model load;
- actual 262K context allocation;
- final tensor placement;
- cold-load time;
- prefill and generation behavior;
- large-context stress run;
- quality evaluation.

## Freeze and Follow-Up Boundary

CC-003A is immutable historical evidence from this point. Do not append retries, modify configuration, add quality work, or replace files under this ID.

If further work is authorized, create a new Community Challenge with a distinct research question and frozen configuration. It may reference CC-003A but must not revise its result.
