# FORGE Canonical Quality Evaluation

This directory is the reusable source for local-model software-quality evaluations. It separates the stable method and fixtures from individual challenge evidence.

## Use This Order

1. Read [the protocol](protocol/quality-evaluation-protocol-v1.md).
2. Verify [the fixture pack](fixtures/azure-csharp-domain-v1/README.md).
3. Create a model-run folder from [the run template](run-template/quality-run-manifest-template-v1.md).
4. Capture raw output, build, test, then review—never reverse that order.
5. Keep challenge-specific results under `community-challenges/`; link to this canonical pack rather than copying or changing it.

## Stable Assets

- `protocol/` — acceptance method and status semantics.
- `fixtures/` — human reference, baseline tests, seeded defects, and hashes.
- `run-template/` — mandatory per-model evidence record.
- `scripts/` — PowerShell helpers for raw capture and fixture verification.
- `model-evaluations/` — one immutable run folder per model/configuration.

The current canonical fixture is synthetic/public only. Do not put corporate code, secrets, PHI, customer data, or proprietary architecture in any run.
