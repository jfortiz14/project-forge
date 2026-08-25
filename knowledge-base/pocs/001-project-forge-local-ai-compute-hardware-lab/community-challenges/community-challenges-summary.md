# Community Challenges — Living Summary

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Purpose:** Living top-level comparison of frozen performance and quality evidence across community challenges.  
> **Data:** Synthetic/public only.  
> **Evidence rule:** `N/R` means not recorded or not measured; it is not a loss.

## How To Use This Summary

This document compares observed evidence without declaring a permanent overall winner. Performance and quality are separate dimensions, and differing prompt lengths, output lengths, capture methods, and evaluation fixtures limit direct ranking. Each row links to the detailed challenge evidence that remains authoritative.

## Experimental Baseline

| Field | Frozen baseline |
| --- | --- |
| Machine | Machine A — personal desktop; 32 GiB RAM; NVIDIA GeForce RTX 3070 with 8,192 MiB VRAM |
| Runtime | Ollama with NVIDIA CUDA mixed offload |
| Context | 4,096 tokens |
| Thinking | Disabled for the independent baseline and quality runs |
| Performance workload | FORGE-style prompt family covering prefill/generation concepts, factors, reproducible benchmarking, and software-development workflow |
| Quality workload | Frozen Azure/C# planning, implementation, contract-test generation, and read-only review units |
| Measurement rule | Preserve raw output and telemetry; run compile/test gates before semantic acceptance where applicable |

## Challenge Comparison

| Challenge | Model / profile | Performance baseline | Quality outcome | Current interpretation | Detailed evidence |
| --- | --- | --- | --- | --- | --- |
| CC-001 Ornith | Ornith 1.5 35B-A3B Q4_K_M; `forge-ornith-35B-A3B-ctx4096-nothink` | 3 observed no-thinking runs | No autonomous quality acceptance | Runnable on Machine A; baseline format drift limits strict qualitative equivalence. | [challenge](CC-001-ornith/) · [performance](CC-001-ornith/CC-001A-independent-baseline/results-matrix.md) · [quality](CC-001-ornith/quality-evaluation-register-summary.md) |
| CC-002 Qwen | `qwen3.5:35B-A3B` Q4_K_M; observed alias `forge-qwen3-35B-A3B-ctx4096-nothink:latest` | 3 observed no-thinking runs | No autonomous quality acceptance | Runnable on Machine A; baseline prose adhered more closely to requested structure, while quality units did not meet acceptance gates. | [challenge](CC-002-qwen/) · [performance](CC-002-qwen/CC-002A-independent-baseline/results-matrix.md) · [quality](CC-002-qwen/quality-evaluation/quality-evaluation-register-summary.md) |

## Performance Evidence

The ranges below are observed values from each challenge's accepted baseline rows. They are descriptive, not rankings: prompt/output token counts and exact output formatting differ across runs.

| Challenge | Runs | Load time | Prompt evaluation | Generation | Total duration | VRAM observed | Placement / format caveat |
| --- | --- | --- | --- | --- | --- | --- | --- |
| CC-001 Ornith | 3 | 1m6.19s–1m7.27s | 89.04–106.13 tok/s | 28.42–31.24 tok/s | 1m30.75s–1m33.38s | 7,750–7,795 MiB / 8,192 MiB where recorded | 75%/25% CPU/GPU where recorded; Markdown headings/bold caused partial benchmark-format comparability. |
| CC-002 Qwen | 3 | 1m10.81s–1m11.91s | 63.09–104.48 tok/s | 27.60–30.23 tok/s | 1m20.57s–1m36.26s | 7,694–7,785 MiB / 8,192 MiB | 76%/24% CPU/GPU; plain prose stayed closer to the requested baseline structure. |

## Quality Capability Outcomes

| Capability | CC-001 Ornith | CC-002 Qwen | Comparison boundary |
| --- | --- | --- | --- |
| Azure/C# planning | Fail; planning and one corrective pass did not satisfy the fixed requirements. | Fail; planning and one corrective pass exceeded the fixed range and retained material design gaps. | Same quality contract/prompt family; outcomes do not establish a general model ranking. |
| C# implementation | Fail; raw output and pipeline re-run did not compile. | Fail; raw output and two separate reproducibility captures did not compile. | Both failed the raw-source compile gate. |
| Minimal human repair | Testable after fence removal plus one code-line repair; 4 of 8 contract checks failed. | N/R; no eligible two-fence baseline, so no repair was invented. | Not directly comparable. |
| C# test generation | Fail; generated suite did not compile, including its fence-only derivation. | Fail; generated suite did not compile against the frozen human reference. | Neither produced execution or mutant-detection evidence. |
| Code and test review | Fail; 0 of 4 known material defects identified and 1 material false positive. | Fail; 4 of 4 known defects identified, but read-only, format, and traceability constraints were violated. | CC-002 uses review fixture v2 after CC-001 v1 hash drift; compare defect recognition cautiously. |

## Notable Failure Modes

| Area | CC-001 Ornith | CC-002 Qwen |
| --- | --- | --- |
| Output discipline | Markdown fences appeared in raw C# implementation and generated-test outputs. | Markdown fences or non-source/trailing text appeared in all measured implementation captures. |
| Implementation acceptance | Minimal repair enabled compilation, but tests exposed missing idempotency validation, URI-query fingerprinting, fingerprint immutability, and claim-token rotation. | No implementation candidate reached contractual tests because no raw capture passed compilation. |
| Test generation | Fence removal still left unresolved API references. | Required BCL imports were absent and a local was redeclared. |
| Review behavior | Invented retry/lease semantics and proposed a non-compliant replacement. | Generated a replacement solution and assumed the submitted implementation was partial. |

## Interpretation And Limitations

- Both profiles were demonstrably loadable and runnable on the frozen Machine A baseline under no-thinking controls.
- The current quality evidence does not support autonomous use of either profile for the evaluated Azure/C# tasks.
- Qwen's baseline response formatting and review defect recognition are useful observations, not an overall win: its quality units still failed their respective acceptance contracts.
- Ornith's one-line repair result measures human-plus-model effort only, not autonomous implementation quality.
- Performance numbers should not be used as procurement, cost, production, or security decisions. Mixed CPU/GPU placement, VRAM pressure, prompt/output length differences, and load semantics remain material caveats.
- Each challenge remains responsible for its own frozen configuration, raw outputs, execution manifest, results matrix, and quality register.

## Add A Future Challenge

Add one row to each applicable table using the same fields:

| Challenge | Model / profile | Performance baseline | Quality outcome | Current interpretation | Detailed evidence |
| --- | --- | --- | --- | --- | -
| CC-NNN name | Source model, quantization, operational alias | Run count and evidence status | Per-unit outcome or `N/R` | Evidence-backed, non-ranking interpretation | Challenge, performance, and quality links |

Record only observed ranges and completed quality gates. Link to the challenge's frozen artifacts rather than copying raw evidence into this summary.

## Related Indexes

- [Quality-specific cross-challenge evidence index](community-challenges-quality-summary.md)
- [Community challenge operating rules and index](README.md)
