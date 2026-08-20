# Compliance Checklist: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **ADR Reference:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Regulatory Scope

The POC is explicitly non-regulated only if it processes no PHI, PII, corporate data, or restricted data. HIPAA/SOC 2 controls are not triggered by approved synthetic/public inputs, but the prohibition remains mandatory.

| Control | Status | Evidence / Required Action |
| --- | --- | --- |
| Data classification | ⚠️ | Label all benchmark prompts and evidence non-sensitive. |
| PHI / corporate-data exclusion | Required | BR-001 and operator attestation. |
| Retention | ⚠️ | Keep only non-sensitive configuration and performance evidence. |
| Third-party/API processing | Not in scope | Re-review if added. |
| HIPAA applicability | Not applicable to approved inputs | Becomes applicable immediately if PHI/ePHI is introduced; then stop. |

## Decision

No BAA, consent workflow, or PHI audit trail is required for the approved non-sensitive POC. They are not substitutes for the prohibition on restricted data.
