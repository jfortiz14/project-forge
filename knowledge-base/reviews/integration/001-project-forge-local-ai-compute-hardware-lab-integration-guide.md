# Integration Guide: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **ADR Reference:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Boundary

The baseline uses local Windows runtimes only. The operator executes PowerShell commands and supplies results to the evidence matrix. There is no automated cross-machine orchestration, shared service, or cloud submission.

## Interface Rules

1. Use one approved runtime at a time.
2. Preserve runtime version, command/configuration, model identifier, quantization, context setting, and output logs sufficient to establish backend/offload.
3. Do not expose local runtime ports beyond the device without a separate integration and security review.
4. Do not send benchmark prompts or logs to external APIs during the baseline unless that path is explicitly added and reviewed.

## Decision

The integration scope is deliberately minimal; reproducible local evidence precedes automation or network integration.
