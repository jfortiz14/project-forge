# CC-001 Quality Evaluation Register

> **Contract:** `forge-quality-contract-v1-software-architecture-coding.md`
> **Data class:** Synthetic/public only
> **Status:** Planning output captured; implementation-quality evaluation in progress

## Evaluation Sequence

1. Planning baseline
2. Implementation
3. Test generation
4. Code/architecture review

## Q-001 — Planning Baseline

| Field | Value |
| --- | --- |
| Model | Ornith 1.5 35B-A3B |
| Runtime | Ollama / NVIDIA CUDA / Desktop RTX 3070 |
| Context | 4,096 |
| Thinking | Disabled explicitly with `--think=false` |
| Prompt | FORGE quality-planning prompt for document-intake architecture |
| Required review | Requirement fidelity, technical correctness, constraint adherence, completeness, maintainability, verifiability |
| Status | Closed — planning output captured for challenge comparison. |

### Q-001a — Planning Output Review

| Dimension | Result |
| --- | --- |
| Timing | 1m5.7440446s load; 174.26 prompt tok/s for 309 tokens; 29.49 generation tok/s for 1,211 tokens; 1m48.6023871s total. |
| Format | Pass — six labeled sections and no code fence. |
| Requirement coverage | Partial pass — covers architecture, API, data/identity/idempotency, failure handling, tests, and risks. |
| Material findings | The answer introduces implementation details not strictly required by the planning prompt (for example specific frameworks and technologies) and narrows some open decisions too early. It still follows the expected architecture-planning shape, but it is more prescriptive than the contract asks for. |
| Quality status | Partial pass — useful planning draft, but not enough by itself to validate the later implementation and review units. |

## Current Status

Planning baseline is captured. Implementation-quality unit `Q-002` has been captured and reviewed as a failure.

## Q-002 — C# Domain Implementation

| Field | Value |
| --- | --- |
| Model | Ornith 1.5 35B-A3B |
| Runtime | Ollama / NVIDIA CUDA / Desktop RTX 3070 |
| Context | 4,096 |
| Thinking | Disabled explicitly with `--think=false` |
| Prompt | FORGE quality implementation prompt for the document-intake domain |
| Required review | Compile, API/contract fidelity, idempotency semantics, conditional-claim semantics, state transitions, cancellation, and error handling |
| Status | Closed — implementation candidate rejected. |

### Q-002a — Generated Implementation Review

| Dimension | Result |
| --- | --- |
| Timing | 1m4.265344s load; 210.95 prompt tok/s for 373 tokens; 28.87 generation tok/s for 2,083 tokens; 2m18.2106345s total. |
| Format | Pass — raw C# only, no markdown fence. |
| Compile readiness | **Fail by inspection:** the code references undeclared or mismatched members (`Matches` returns `IntakeRecord`-like behavior but is typed as `IntakeRecord`, `Fingerprint` is never assigned, `Completed`/`Failed` properties are nonsensical, and the record construction/return flow is internally inconsistent). |
| Contract fidelity | **Fail:** `IntakeState` and `IntakeDecision` are incomplete versus the contract; `Matches(IntakeRequest)` does not compare fingerprints; `TryClaim` returns only `IntakeDecision` rather than a new `Processing` record; `TryComplete`/`TryFail` do not return updated records and misuse the result paths. |
| Material findings | The model invented a different shape for the API, omitted required state values, generated incorrect record-transition semantics, and introduced uninitialized/private members that do not support the requested domain contract. |
| Quality status | **Fail — reject.** |

## Q-003 — Test Generation

| Field | Value |
| --- | --- |
| Model | Ornith 1.5 35B-A3B |
| Runtime | Ollama / NVIDIA CUDA / Desktop RTX 3070 |
| Context | 4,096 |
| Thinking | Disabled explicitly with `--think=false` |
| Prompt | FORGE quality test-generation prompt for the document-intake domain |
| Required review | Executability, contract fidelity, success/failure coverage, and meaningful edge cases |
| Status | Closed — test candidate rejected. |

### Q-003a — Generated Test Review

| Dimension | Result |
| --- | --- |
| Timing | 1m2.8745948s load; 115.68 prompt tok/s for 310 tokens; 30.09 generation tok/s for 3,772 tokens; 3m10.9192995s total. |
| Format | Fail — raw C# was returned, but it targeted a different API shape than the contract. |
| Executability | **Fail by inspection:** the tests call methods and constructors that do not exist in the FORGE contract (`IntakeRequest.Create` with extra parameters, `IntakeRecord.CreateProcessing`, `CreateCompleted`, `TryClaim` returning a bool, etc.). |
| Contract fidelity | **Fail:** the test set validates an invented implementation surface rather than the specified domain contract; it does not align with the actual `azure-csharp-domain-contract-v1.md` signatures and result types. |
| Coverage | Partial pass only on broad themes: request validation, fingerprint stability, record creation, claim/completion/failure behavior, and UTC/lease checks. |
| Material findings | The generated tests are not usable against the real contract because they are written for a different API, include undefined members, and make assertions that would not compile or would not apply to the intended implementation. |
| Quality status | **Fail — reject.** |

## Next Step

Proceed to the code/architecture review unit only if you want to continue the FORGE sequence with a corrected implementation or another model attempt. The current challenge record has planning, implementation, and test-generation evidence, and both implementation and test units failed.


## Current Status

Planning, implementation, and test generation have all been captured; review was attempted but is not reviewable because no implementation was supplied with the prompt.

## Recording Rules

- Record model, model id/tag, quantization, runtime/backend, hardware, context, and thinking mode.
- Record prompt version and effective prompt/output token counts.
- Record load time, prefill tokens/s, generation tokens/s, and subjective usability.
- Record format result, requirement-fidelity result, executable-validation result, findings by severity, and final status.
- Record comparability caveats, including template/tokenizer differences, changed prompt lengths, or different output lengths.

## Status

Quality prompts have been executed for `CC-001`; planning passed partially, implementation failed, test generation failed, and code review was not reviewable because the implementation was omitted.
