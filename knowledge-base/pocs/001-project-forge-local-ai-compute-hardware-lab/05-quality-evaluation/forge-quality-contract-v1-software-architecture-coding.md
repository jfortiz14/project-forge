# FORGE Quality Contract v1 — Software Architecture & Coding

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Status:** Completed evaluation contract  
> **Version:** 1.0  
> **Applies to:** Local LLM-assisted Azure architecture and C#/.NET application-development work

## 1. Purpose

This contract evaluates whether a local model is useful for Azure architecture and C#/.NET coding work. It measures output quality separately from inference speed. A fast, coherent, or well-formatted response is not considered correct without evidence against the stated requirements.

## 2. Scope and Safety Boundary

Use only synthetic, public, or user-approved non-sensitive inputs. Do not send corporate source code, credentials, PHI, secrets, customer data, or proprietary architecture details to a local runtime unless the applicable policy explicitly permits it.

The model is an assisted drafting tool. A human remains accountable for architecture decisions, code acceptance, test acceptance, security review, and deployment.

## 3. Evaluation Units

Each model is evaluated independently in four units. The exact prompt, model identifier, quantization, runtime, context setting, output, timings, and reviewer findings must be retained.

1. **Azure architecture/planning** — requirements decomposition, C#/.NET boundaries, Azure service choices, identity, data/control flow, resilience, observability, risks, trade-offs, and test strategy.
2. **C# implementation** — complete minimal .NET code that satisfies a fixed contract without unnecessary dependencies or invented cloud configuration.
3. **Test generation** — executable .NET tests that cover stated behavior, invalid inputs, resilience paths, and meaningful edge cases.
4. **Code/architecture review** — accurate findings, prioritized risk, Azure operational implications, and actionable remediation without invented defects.

## 4. Required Quality Dimensions

| Dimension | Pass condition |
| --- | --- |
| Requirement fidelity | Every explicit requirement is preserved; conflicts, ambiguities, and assumptions are identified rather than silently invented. |
| Technical correctness | Claims, algorithms, types, and code are valid for the stated language/runtime. |
| Constraint adherence | The response follows requested output format, size, scope, and prohibited-content rules. |
| Completeness | All requested deliverables are present, including error paths and stated edge cases. |
| Maintainability | Names, boundaries, types, validation, and tests are understandable and avoid needless complexity. |
| Security and safety | No unsafe defaults, secret exposure, insecure patterns, unsupported Azure claims, or authorization gaps. |
| Verifiability | Code compiles or runs where applicable; tests are executable; claims map to evidence. |

## 5. Severity and Acceptance Rules

Classify each finding as follows:

- **Critical:** security exposure, data loss/corruption, unsafe authorization behavior, or a result that cannot be safely reviewed in scope. Automatic fail.
- **Material:** violates a fixed requirement, fails to compile/run, creates incorrect behavior, omits a mandatory test path, or recommends an incorrect repair. Not acceptable without repair and revalidation.
- **Minor:** clarity, completeness, style, or non-blocking maintainability issue. May be accepted with recorded human remediation.

An evaluation unit passes only when it has no critical or material findings and satisfies its requested format. A model is not approved for autonomous use by passing one unit; approval is limited to the completed unit and workload.

## 6. Unit-Specific Evidence Rules

### 6.1 Architecture and Planning

The output must retain fixed requirements, state assumptions, identify risks and alternatives where requested, and propose a testable Azure acceptance path. Where Azure services are selected, it must address identity, failure handling, observability, and cost/operational implications relevant to the prompt. It fails if it changes the problem, omits a mandatory requirement, or presents a preference as a verified fact.

### 6.2 Implementation

Review source against the contract before execution. Then restore, build, analyze, and run focused .NET tests where tooling is available. Verify boundary cases such as null/empty values, invalid input, cancellation, idempotency, transient-failure handling, authorization, and non-mutation when relevant.

### 6.3 Test Generation

Tests must import the target correctly, compile/run, assert expected behavior rather than merely execute code, and include both success and failure paths. Generated tests are not accepted until their own contract fidelity is reviewed.

### 6.4 Review

Every reported defect must be traceable to the submitted artifact and requirement. Review output fails for fabricated observations, false-positive mandatory fixes, or failure to identify an obvious material violation present in the supplied code/tests.

## 7. Reporting Record

Record one row or section per run with at least:

- Model, model ID, quantization, runtime/backend, hardware, context, and thinking mode.
- Prompt version and effective prompt/output token counts.
- Load time, prefill tokens/s, generation tokens/s, and subjective usability.
- Format result, requirement-fidelity result, executable-validation result, findings by severity, and final status.
- Comparability caveats, including template/tokenizer differences, changed prompt lengths, or different output lengths.

## 8. Current FORGE Interpretation

The prior TypeScript evidence demonstrates that a model can be useful for planning while still producing material implementation defects, invalid tests, and incomplete reviews. It remains historical evidence only. The formal evaluation sequence now focuses on Azure and C#/.NET. These observations reinforce the contract: fluent output and high tokens/s are not acceptance evidence.

## 9. Decision Boundary

Use a local model for brainstorming, outlining, drafting, explanation, and candidate test ideas only when a human validates the result. Do not use an evaluated model as the sole authority for architecture sign-off, merge approval, test acceptance, security review, or production deployment.

## 10. Related Evidence

- `application-development-workload-v1.md`
- `../03-benchmark-method/benchmark-contract-v1.md`
- `../04-performance-evidence/results-matrix.md`
