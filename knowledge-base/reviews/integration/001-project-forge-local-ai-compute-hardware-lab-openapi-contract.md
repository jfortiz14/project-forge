# OpenAPI Contract Assessment: Project FORGE — Local AI Compute & Hardware Lab

> **Initiative:** 001-project-forge-local-ai-compute-hardware-lab  
> **Status:** No network API is in scope  
> **ADR Reference:** `knowledge-base/adrs/001-project-forge-local-ai-compute-hardware-lab.md`

## Contract Decision

The initial POC has no service-to-service or remote API integration. Ollama and `llama.cpp` may expose local HTTP endpoints, but the laboratory will treat them as local runtime interfaces, not production APIs. A formal OpenAPI 3.0 contract is deferred unless a result-ingestion service or remote execution controller is introduced.

## Result Record Schema (logical contract)

| Field | Required | Notes |
| --- | --- | --- |
| runId, timestamp, machine, backend | Yes | Identifies execution environment |
| model, quantization, modelSize | Yes | Identifies model artifact |
| contextTokens, promptVersion | Yes | Ensures comparability |
| loadTimeSeconds, prefillTokensPerSecond, generationTokensPerSecond | Yes | Distinct performance measures |
| ramGiB, vramGiB, sharedMemoryGiB, gpuOffload | Yes | Memory and placement evidence |
| cpuUtilization, gpuUtilization, usability, notes | Yes | Practical experience and caveats |

## Integration Finding

🟢 No OpenAPI artifact is required for the current local-only boundary. Any future API/cloud comparison must define authentication, data classification, retention, and request/response schemas before use.
