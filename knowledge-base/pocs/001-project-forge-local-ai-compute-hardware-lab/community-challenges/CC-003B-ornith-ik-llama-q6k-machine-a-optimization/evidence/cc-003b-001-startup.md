# CC-003B-001 Startup Evidence — Context Feasibility

> **Experiment:** `FORGE-CC-003B`
> **Candidate:** `CC-003B-001`
> **Changed parameter:** `-c 32768` only
> **Outcome:** Failed before tensor load and server readiness

## Observed Runtime Result

| Field | Observed value |
| --- | ---: |
| CUDA device | `CUDA0` / RTX 3070 |
| Runtime-reported free VRAM at CUDA initialization | 7,100 MiB |
| Requested context | 32,768 |
| Model tensors plus cache estimate | 30,566 MiB |
| Available compute memory before expert overrides | 3,622 MiB |
| Required device memory after CPU expert overrides | 5,365 MiB |
| Available device memory after CPU expert overrides | 4,070 MiB |
| Final outcome | `Unable to auto-fit model` |

The runtime parsed the verified Q6_K model metadata and applied CPU expert overrides for layers 39 through 0, but did not load model tensors or allocate the requested context.

## Comparison to CC-003A

Lowering context from 262144 to 32768 lowered required device memory after CPU expert overrides from 7,367 MiB to 5,365 MiB, a reduction of 2,002 MiB. The candidate still missed the reported available memory by 1,295 MiB. This establishes context as a material resource control but does not establish a loadable configuration.
