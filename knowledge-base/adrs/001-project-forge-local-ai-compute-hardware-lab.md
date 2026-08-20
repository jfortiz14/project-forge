# ADR-001: Project FORGE — Local AI Compute & Hardware Lab Baseline and Evidence Gates

> **Status:** Accepted  
> **Date:** 2026-08-15
> **Created By:** Francisco Ortiz - Software Architect
> **Deciders:** Francisco Ortiz  
> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Executive Proposal:** `knowledge-base/proposals/001-project-forge-local-ai-compute-hardware-lab.md`

## Context

Machine A, a Windows desktop, is the local LLM development-inference baseline. The decision concerns its current hardware plus APIs versus a potential future 24 GB/32 GB GPU. The laboratory must prefer free/open-source software, use PowerShell where possible, measure prefill separately from generation, and use no restricted data.

## Decision

We will run a staged, local-only, evidence-driven POC on Machine A. We will first validate hardware, then establish one reproducible Ollama baseline, adding `llama.cpp` only when it adds a defined comparison value. We will not buy hardware or make performance claims before recorded evidence exists. All inputs remain synthetic, public, or personally authored non-sensitive content. Software and model artifacts must come from official/verifiable sources with version and quantization recorded.

## Rationale

This preserves a practical baseline while isolating runtime/backend variables. It makes memory placement, offload, context impact, and developer usability first-class decision evidence rather than optimizing a benchmark score.

## Alternatives Considered

| Alternative | Pros | Cons | Why Rejected |
| --- | --- | --- | --- |
| Buy a large-VRAM GPU now | Faster potential access to larger models | No evidence of workload fit, compatibility, or ROI | Deferred pending lab evidence |
| Install all candidate runtimes before baseline | Broad coverage | Confounded results and unnecessary runtime change | Rejected |
| Use only token generation throughput | Simple comparison | Misses prefill, load, memory, offload, and usability | Rejected |

## Consequences

- **Positive:** Results will be comparable and auditable; unnecessary purchases and runtime sprawl are avoided.
- **Negative:** The decision is deliberately slower because it requires staged user-run evidence.
- **Risks:** Data-classification violations block execution; power/thermal state can bias results.

## Compliance & Validation

| NFR | How It Is Addressed | Verified By |
| --- | --- | --- |
| Security | Non-sensitive data boundary and approved sources | Security review |
| Reliability | Power/thermal/configuration capture and failed-run handling | Reliability assessment |
| Integration | Local-only boundary and result schema | Integration artifacts |
| Compliance | No PHI/corporate data | Compliance artifacts |
| Domain | Defined terms and measurement invariants | Domain artifacts |
| FinOps | Purchase deferred until workload and cost evidence exists | Cost analysis |

## Agent Review Summary

| Agent | Artifact | Key Findings | Status |
| --- | --- | --- | --- |
| Security | `knowledge-base/reviews/security/001-project-forge-local-ai-compute-hardware-lab-security-review.md` | Non-sensitive-data and provenance controls confirmed. | ✅ |
| Reliability | `knowledge-base/reviews/reliability/001-project-forge-local-ai-compute-hardware-lab-reliability-assessment.md` | Capture power/thermal/configuration; no production SLA. | ✅ |
| Integration | `knowledge-base/reviews/integration/001-project-forge-local-ai-compute-hardware-lab-integration-guide.md` | Keep baseline local-only; no production API contract. | ✅ |
| Compliance | `knowledge-base/reviews/compliance/001-project-forge-local-ai-compute-hardware-lab-compliance-checklist.md` | Non-sensitive scope confirmed. | ✅ |
| Domain | `knowledge-base/reviews/domain/001-project-forge-local-ai-compute-hardware-lab-domain-glossary.md` | Comparable-run invariants are defined. | ✅ |
| FinOps | `knowledge-base/reviews/finops/001-project-forge-local-ai-compute-hardware-lab-cost-analysis.md` | Purchase decision deferred until evidence exists. | ✅ |

## Entry Conditions for POC Execution

1. **Complete:** Machine A has a verified read-only inventory.
2. **Complete:** A non-sensitive common benchmark prompt and result-row format are approved.
3. **Active rule:** Baseline execution begins with one runtime only; results are captured before another backend is introduced.

## Related Documents

All review artifacts are listed in the Agent Review Summary and stored under `knowledge-base/reviews/` for initiative `001`.

## Implementation Outcome

The Machine A POC completed its planned evaluation. Qwen3 8B and Llama 3.1 8B are practical for supervised local drafting; Qwen3 14B is usable but slow; and Qwen3 32B is not interactive on the existing RTX 3070 8 GB. The evaluated models did not meet the Azure/C# quality contract for autonomous acceptance.

No GPU purchase is approved. The GPU-versus-API economic decision remains deferred pending a representative workload profile, API-cost evidence, candidate compatibility, current pricing, and measured candidate performance.

## Changelog

| Version | Date | Author | Change |
| --- | --- | --- | --- |
| 1.0 | 2026-08-15 | Francisco Ortiz | Initial consolidated ADR |
| 1.1 | 2026-08-15 | Francisco Ortiz | Closed security, compliance, provenance, and FinOps decision gates from operator confirmation |
| 1.2 | 2026-08-15 | Francisco Ortiz | Recorded completed Machine A POC outcome; retained current hardware and deferred procurement/API economics decision |
