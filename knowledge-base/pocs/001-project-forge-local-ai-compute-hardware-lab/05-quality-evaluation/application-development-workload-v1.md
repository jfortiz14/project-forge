# Application Development Workload v1

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Purpose:** Qualitative, staged evaluation of local-model usefulness for application development.  
> **Data classification:** Synthetic/public only; no corporate code, credentials, or sensitive data.
> **Quality contract:** `forge-quality-contract-v1-software-architecture-coding.md`

## Reference Problem

Build a small TypeScript `feature-flags` library. A flag has a default Boolean value and zero or more ordered rules. A rule matches only when every condition matches the supplied user context. Rules run from the lowest numeric `priority` to the highest. The first matching rule supplies the value. A missing context field does not match. Unknown operators must be rejected rather than silently treated as matches. Supported operators are `equals`, `notEquals`, and `in`.

## Staged Evaluation

1. **Planning:** Produce an API/module design, types, algorithm, test matrix, and risks.
2. **Implementation:** Produce a minimal, complete TypeScript implementation with no external runtime dependencies.
3. **Testing:** Produce focused unit tests for ordering, missing fields, unknown operators, and all supported operators.
4. **Review:** Inspect the implementation and tests against this contract for correctness, maintainability, and edge cases.

### Implementation Clarifications

For this POC, an empty `conditions` array is invalid and must be rejected. If two rules have the same numeric priority, preserve their original input order. Validate unsupported operators and malformed `in` operands before evaluation so they are rejected even when a user context field is absent.

## Planning Prompt v1

```text
You are a senior application engineer. Design a small TypeScript library named feature-flags using the following fixed requirements:

- A flag has a Boolean default value and zero or more ordered rules.
- A rule has a numeric priority, a Boolean value, and one or more conditions.
- Evaluate rules from the lowest priority number to the highest. The first fully matching rule supplies the result.
- A rule matches only when every condition matches a supplied user-context object.
- A missing context field does not match.
- Supported condition operators: equals, notEquals, and in.
- Unknown operators must be rejected, never silently treated as a match.
- Use TypeScript with no external runtime dependencies.

Deliver exactly these five labeled sections:
1. Module/API design
2. Type design
3. Evaluation algorithm
4. Unit-test matrix
5. Risks and edge cases

Use clear technical English. Keep the answer between 350 and 500 words. Do not write implementation code, tables, citations, or use external tools.
```

## Evaluation Rules

- Record latency/timings separately from output quality.
- Do not treat a fluent answer as correct unless it preserves every fixed requirement.
- Record omissions, contradictions, and unnecessary complexity.
- The same prompt is used for each compared model/runtime whenever its interface permits.

## Execution Record

### APP-001 — Planning / Desktop / Ollama / Qwen3 8B

| Field | Result |
| --- | --- |
| Model and runtime | Qwen3 8B Q4_K_M / Ollama / NVIDIA CUDA / 4,096 context / no-thinking |
| Timing | Cold load 2.649 s; prompt evaluation 1,518.45 tok/s for 225 tokens; generation 69.42 tok/s for 483 tokens; total 9.760 s |
| Format | Pass — five requested labeled sections, no code/table/citations, 350–500 word range |
| Requirement coverage | Pass — covers default fallback, ascending priority, first full match, missing fields, all three operators, and rejection of unknown operators |
| Observations | It proposes a same-priority insertion-order policy, which is a reasonable extension but must be made explicit if adopted. Its statement about empty conditions is ambiguous and must not override the contract requirement that a rule contains one or more conditions. Type detail and precise `in` operand semantics remain light. |
| Qualitative status | Pass with implementation guardrails |

### X-APP-001 — Planning / Desktop / Ollama / Llama 3.1 8B

| Field | Result |
| --- | --- |
| Model and runtime | Llama 3.1 8B Instruct Q4_K_M / Ollama / NVIDIA CUDA / 4,096 context |
| Timing | Cold load 2.762 s; prompt evaluation 2,360.05 tok/s for 207 tokens; generation 73.67 tok/s for 622 tokens; total 11.294 s |
| Format | Partial pass — supplies the five requested labeled sections and no code, table, citation, or tool use; response is materially shorter than the requested 350–500-word range. |
| Requirement coverage | Partial pass — preserves ascending priority, first matching rule, default fallback, rejection of unknown operators, and non-match for missing fields. |
| Defects and omissions | The `UserContext` type incorrectly implies a literal field named `field`, rather than a general user-context object. `Condition.value` is restricted to string/string-array values without requirement basis. It does not state that a rule must have one or more conditions or that *every* condition must match in the design section. The test matrix omits default fallback, priority ordering, first-match behavior, each supported operator, multi-condition matching, and invalid `in` operands. |
| Qualitative status | Partial pass — useful outline, but less complete and precise than APP-001 / Qwen3 8B; not ready to drive implementation without human correction. |

### APP-002 — Implementation / Desktop / Ollama / Qwen3 8B

| Field | Result |
| --- | --- |
| Artifact | [Generated TypeScript](artifacts/featureFlags-app-002.ts) |
| Timing | Warm load 0.240 s; prompt evaluation 1,059.14 tok/s for 250 tokens; generation 69.00 tok/s for 609 tokens; total 9.362 s |
| Format | Pass — one TypeScript code block only |
| Correct behavior covered | Priority sort, default fallback, all requested operators, upfront operator validation, malformed `in` array rejection, finite-priority validation, and no direct mutation of input rules/context |
| Defects found in review | **Material:** `!condition.value` rejects valid falsy values such as `false`, `0`, and `''`. **Material:** `in` uses `Array.includes`, not required `Object.is` semantics (`-0`/`0` differ). **Additional:** `contextValue === undefined` tests value rather than own-field existence; same-priority behavior relies on runtime sort stability; array/object shape validation is incomplete. |
| Qualitative status | Partial pass — do not accept as contract-compliant implementation |

### APP-003 — Test Generation / Desktop / Ollama / Qwen3 8B

| Field | Result |
| --- | --- |
| Artifact | [Generated tests](artifacts/featureFlags-app-003.test.ts) |
| Timing | Warm load 0.133 s; prompt evaluation 1,633.19 tok/s for 741 tokens; generation 67.67 tok/s for 1,174 tokens; total 18.006 s |
| Executability | **Fail:** does not import `FeatureFlag` or `FeatureFlagEvaluator`; the unknown-operator literal is also incompatible with the declared union type without an explicit cast. |
| Contract fidelity | **Fail:** priority tests use invalid empty-condition rules; the empty-condition test expects a match although the POC requires rejection; the "missing fields" test supplies the field. |
| Semantic-test quality | **Fail:** its `in` test expects separately allocated `{ x: 1 }` objects to match, contradicting required `Object.is` identity semantics. It omits falsy values, `NaN`, and `-0`/`0` tests that would expose implementation defects. |
| Qualitative status | Fail — fluent but unreliable test generation; do not execute without repair |

### APP-004 — Code Review / Desktop / Ollama / Qwen3 8B

| Field | Result |
| --- | --- |
| Timing | Cold load 13.517 s; prompt evaluation 2,763.03 tok/s for 1,792 tokens; generation 66.22 tok/s for 327 tokens; total 19.113 s |
| Correct findings | Correctly identifies that `in` uses `Array.includes` rather than required `Object.is` semantics; correctly flags the intended meaning of missing fields and invalid empty conditions. |
| Incorrect or weak findings | Claims that a deep clone is required to avoid mutation although the candidate does not mutate nested rules/context and sorts a copied rule array. It describes the "missing fields" test as expecting a match for a missing field, but that test actually supplies the field and fails to test absence. |
| Important omissions | Fails to identify the `!condition.value` falsy-value defect, the absent imports/union-type compile failures in the test suite, the invalid object-identity expectation in the `in` test, and the `undefined`-value versus own-property distinction. |
| Qualitative status | Partial pass — useful signal but incomplete and includes a false-positive fix recommendation |

## Application-Workflow Conclusion

For this 8B desktop configuration, planning was useful and correctly structured, but implementation, tests, and review were not independently reliable. The local model is appropriate as an assisted drafting tool with mandatory human review and executable validation; it is not approved for autonomous code generation, test acceptance, or code-review sign-off.
