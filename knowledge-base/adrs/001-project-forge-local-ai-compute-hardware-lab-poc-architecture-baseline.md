# ADR-001 POC Architecture Baseline: Project FORGE — Local AI Compute & Hardware Lab

> **Status:** Accepted — hardware-validation stage  
> **Date:** 2026-08-15
> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Parent ADR:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Decision

The POC execution architecture is a user-operated, local-only Windows laboratory with no production service boundary. Its first work package is read-only inventory and policy validation. Ollama is the first runtime only after entry gates pass; additional runtimes/backends are introduced sequentially when justified by an evidence gap.

The evidence stage uses Ollama as the reproducible baseline runtime and llama.cpp as a targeted placement-audit runtime. The current default operational profile is Qwen3 8B Q4_K_M with no-thinking and 4,096 context: it is the only tested profile rated usable–excellent on the desktop. Any model-produced application code, tests, or reviews remain advisory and require human review plus executable validation.

Hardware procurement remains explicitly outside this ADR's acceptance decision. The POC must first quantify the real local-workload and API-cost boundary; see `knowledge-base/pocs/001-project-forge-local-ai-compute-hardware-lab/06-findings-and-decision/poc-final-decision.md`.

## Entry and Exit Conditions

Policy and data-classification conditions (EC-001 and EC-002) are complete. Hardware inventory (EC-003) and benchmark-contract approval (EC-004) remain prerequisites to the first runtime benchmark. The POC exits only with an evidence-backed Go/Pivot/Stop decision, including practical-usability ratings and a stated capacity-and-API-cost hardware recommendation boundary.
