# Cost Analysis: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Currency:** USD  
> **ADR Reference:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Cost Summary

No purchase is approved. Current market prices, electricity rates, API usage profile, and model/runtime results are not yet available; monetary totals are intentionally **TBD**, not estimated as facts.

| Option | CAPEX | OPEX | Value / Risk | Decision State |
| --- | --- | --- | --- | --- |
| Existing hardware + open-source local runtimes | $0 incremental assumed | Electricity; operator time: TBD | Establishes measured baseline | Proceed |
| Existing hardware + APIs for overflow | $0 incremental assumed | Token/API usage: TBD | Elastic capacity, recurring cost and data governance constraints | Evaluate after workload profile |
| Desktop RAM upgrade to 64 GB | Price: TBD | Minor electricity delta | More CPU/RAM offload capacity; not VRAM | Defer pending results |
| Intel Arc Pro, NVIDIA RTX, or AMD Radeon/Pro 24 GB/32 GB-class GPU | Price, availability, PSU/cabling compatibility: TBD | Electricity: TBD | Potential larger-model capacity; runtime compatibility and performance must be evidenced | Defer pending decision gate |

## FinOps Findings

| Severity | Finding | Recommendation |
| --- | --- | --- |
| 🟡 | Purchase cannot be evaluated without a workload profile and measured comfortable model size. | Complete model ladder and usability evidence first. |
| 🟡 | Any Intel, NVIDIA, or AMD upgrade total cost includes card price, desktop PSU/headroom, physical fit, drivers, runtime support, and time—not card price alone. | Price the complete upgrade only after technical gate. |
| 🟢 | API comparison needs realistic monthly prompt/completion volume. | Record representative development workload after local baseline. |

## Decision Gate

The operator selected **both** decision drivers. Recommend purchase only if evidence shows either (a) a recurring, valuable workload materially constrained by current VRAM/RAM or (b) API cost that is materially unfavorable for the measured representative workload, and the candidate upgrade meets compatibility and practical-usability requirements.
