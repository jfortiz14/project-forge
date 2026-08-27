# CC-003A Community Reference and Comparability Boundaries

> **Experiment:** `FORGE-CC-003A`  
> **Reference contributor:** Michael Eric Walter Wegener  
> **Rule:** The external reference below is frozen verbatim in substance. It is not a Machine A command prescription.

## Frozen External Reference

| Field | Community-provided value |
| --- | --- |
| Model | `Ornith-1.5-35B-Q6_K.gguf` |
| Runtime | `ik_llama.cpp` |
| Runtime commit | `0ed847d` |
| Context | `262144` |

### Community Shipped Server Flags

```text
--host 0.0.0.0 --port 8080 --device CUDA0
--model Ornith-1.5-35B-Q6_K.gguf
--mmproj mmproj-Ornith-1.5-35B-BF16.gguf --no-mmproj-offload
--metrics -c 262144 -b 4096 -ub 4096 --parallel 1
--reasoning-budget 4096 --reasoning-tokens auto
--jinja -ctv q4_0 -ctk q8_0 -muge -mqkv -p 1
--fit --fit-margin 3030
```

## Explicit Comparability Boundaries

These are environmental differences, not defects. The same flags are not assumed to fit, start, place layers, or perform identically on Machine A.

| Dimension | Community Machine B | Machine A v2 |
| --- | --- | --- |
| Operating system | Linux / CachyOS | Windows |
| GPU | RTX 3060, 12 GB | RTX 3070, 8 GB |
| System memory | ~62 GiB RAM | 64 GB DDR4-2666 |
| CUDA | 13.3 | Current local NVIDIA/CUDA environment — observe and record |
| ik_llama.cpp build | `0ed847d` | Build/commit actually used — observe and record |

## Reproduction Discipline

- The contributor's FORGE fork is reference-only. Do not clone from it, create branches in it, open pull requests, or make any external write as part of CC-003A. All FORGE artifacts and local work remain in the Project FORGE repository and the operator's local environment.
- Preserve the model identity, runtime target, context target, and shipped flags above as the external reference.
- Record the exact executable, build/commit, invocation, driver/CUDA environment, and model-file identity actually used on Machine A.
- A Windows-compatible invocation may be necessary, but it must be recorded as an adaptation; it must not overwrite or silently rewrite the community reference.
- Record startup, OOM, runtime failure, placement, RAM, VRAM, cold-load, prefill, generation, and context-allocation outcomes as observed.
- Do not tune flags, reduce context, replace quantization, or substitute a different model under the name of this reproduction attempt.
