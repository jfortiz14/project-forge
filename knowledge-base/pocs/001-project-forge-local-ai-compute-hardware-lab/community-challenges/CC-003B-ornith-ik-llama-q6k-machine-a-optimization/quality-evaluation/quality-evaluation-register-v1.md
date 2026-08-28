# CC-003B Ornith / ik_llama.cpp — Quality Evaluation Register v1

> **Initiative:** Project FORGE — Local AI Compute & Hardware Lab  
> **Challenge:** `FORGE-CC-003B`  
> **Evaluation type:** Quality only; separate from the completed capacity/performance study  
> **Status:** Q-001 through Q-004 recorded; Q-002F testable but contract fail  
> **Data class:** Synthetic/public only  
> **Quick view:** [quality summary](quality-evaluation-register-summary.md)

## Purpose and Boundary

Evaluate the selected Machine A v2 configuration as a human-supervised drafting aid for the frozen Azure/C# document-intake workload. Timing is diagnostic only and never changes a quality verdict.

This register does not reopen CC-003B capacity findings. It evaluates a no-thinking invocation profile and its conclusions do not generalize to reasoning-enabled use.

## Frozen Execution Configuration

| Field | Value |
| --- | --- |
| Runtime | `ik_llama.cpp`, version 4848, commit `0ed847d3` |
| Model | Ornith 1.5 35B-A3B Q6_K |
| Context / batch / microbatch | 196,608 / 2,048 / 2,048 |
| Cache | K `q4_0`, V `q4_0` |
| Placement | CUDA0, `--fit --fit-margin 1024`; projector on CPU |
| Reasoning | Disabled: `--reasoning-budget 0 --reasoning-tokens none` |
| Session behavior | Fresh server per model unit; one request; no history |
| Prompt source | [frozen prompts](prompts/README.md) |
| Contract and fixtures | [frozen inputs](frozen-inputs.md) |

## Evaluation Map

```text
Q-001  Planning -------------------------- FAIL (624 words)
  `- Q-001C corrective pass ------------- FAIL (632 words; material gaps remain)

Q-002  Independent implementation -------- FAIL (literal Markdown fences)
  `- Q-002F fence-only derivation -------- TESTABLE → 7/8 contract tests pass

Q-003  Independent test generation ------- FAIL (literal Markdown fences)
  `- Q-003F fence-only derivation -------- FAIL (CS1513: missing })
      `- Q-003H minimal human repair ----- TESTABLE → 6/8 reference checks pass

Q-004  Code and test review -------------- FAIL (3/4 material defects identified)
```

## Q-001 — Azure/C# Planning

**Prompt:** `azure-csharp-quality-planning-v1`, SHA-256 `7640A997DFB79A093C8FFC43B23C54B8E8BFE085EEC5CDAC6E04A48BDA22B4DA`.

| Criterion | Observed result |
| --- | --- |
| Raw evidence | `q-001-ornith-raw.txt`, SHA-256 `F97D61C262B87190E5006D0099B32C42A7AD5BE55E64283B03D46E86DDBAF3F6` |
| Transport | HTTP 200; no truncation |
| Required sections / prohibited forms | Pass — six headings present; no code, table, citation, or tool output |
| Word range | **Fail** — 624 words, above the fixed 600-word maximum |
| Verdict | **Fail — output-format gate** |

See [Q-001 review](evidence/q-001-review.md). One corrective planning pass was authorized.

### Q-001 Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Client elapsed | 159.378 s |
| Prompt evaluation | 300 tokens; 1.780 s; 168.56 tokens/s |
| Generation | 3,099 tokens; 157.008 s; 19.74 tokens/s |
| Total server time | 158.787 s; 3,399 tokens |
| Transport / context | HTTP 200; 3,398 cached tokens; `truncated=false` |

## Q-001C — Single Corrective Planning Pass

**Prompt:** `azure-csharp-quality-planning-corrective-v1`, SHA-256 `3432E1F58556E30C1CE7078D11FA13EF8133F5EE830F11514871D7106EE6902D`.

| Criterion | Observed result |
| --- | --- |
| Raw evidence | `q-001c-ornith-raw.txt`, SHA-256 `35AB417E51D86CFE780BA46FC44360EE3A326D087301454B06F83AAAAC25EA65` |
| Word range | **Fail** — 632 words |
| Corrective requirements | **Fail** — no same-key/different-request conflict or durable enqueue reconciliation; no crash-safe external-effect boundary; least-privilege roles remain imprecise |
| Verdict | **Fail — planning closed** |

See [Q-001C review](evidence/q-001c-review.md). This consumed the only corrective planning pass.

### Q-001C Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Client elapsed | 587.460 s |
| Prompt evaluation | 432 tokens; 6.747 s; 64.03 tokens/s |
| Generation | 11,640 tokens; 580.626 s; 20.05 tokens/s |
| Total server time | 587.373 s; 12,072 tokens |
| Transport / context | HTTP 200; 12,071 cached tokens; `truncated=false` |

## Q-002 — Independent C# Domain Implementation

Q-002 uses the human-reviewed architecture/domain boundary only; no Q-001 output was supplied.

| Criterion | Observed result |
| --- | --- |
| Raw evidence | `q-002-ornith-raw.cs`, SHA-256 `DA31139A7443ABCD5B275903C7F991F25AEF2917F0C0F1DF9640DF40975369E9` |
| Literal build | **Fail** — 7 errors: opening and closing Markdown fences caused `CS1056`; opening label also caused `CS0116` |
| Tests / semantic review | N/R — prohibited after failed literal build |
| Verdict | **Fail — raw-source compile gate** |

See [Q-002 literal build](evidence/q-002-literal-build.md).

### Q-002 Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Client elapsed | 2,937.000 s |
| Prompt evaluation | 514 tokens; 3.246 s; 158.33 tokens/s |
| Generation | 57,695 tokens; 2,928.738 s; 19.70 tokens/s |
| Total server time | 2,931.984 s; 58,209 tokens |
| Transport / context | HTTP 200; 58,208 cached tokens; `truncated=false` |
| Literal build | 7 compiler errors; build duration 1.6 s |

### Q-002F — Authorized Fence-Only Derivation

The original remained immutable. Exact outer fences were removed after source-hash verification; derived SHA-256: `55301E6A90759C642874F2EB1F1407E0B20FF484324694FC09D3C6AD490EC699`.

The first physical-path invocation ended with `MSB3030`, without a C# diagnostic. Later preflight evidence established that the physical workspace path exceeded Windows `MAX_PATH`; the same workspace succeeds through a short mapped drive. The intermediate `obj` DLL/PDB and missing final `bin` DLL are consistent with that output-path failure. That invocation is invalid infrastructure evidence only. The immutable Q-002F artifact then passed its literal short-path build in 2.0 seconds with no reported warnings/errors; see [Q-002F record](evidence/q-002f-literal-build.md).

### Q-002F Contractual-Test Entry

The successful short-path build made the exact format-only derivative eligible for contractual tests. The first human-authored harness (`q-002f-contract-tests-v1`) was attempted and failed to compile because it assumed a different fixture's API: its calls omitted Q-002F's required `blobReference`, treated its string fingerprint as a mutable byte array, and used the wrong `CreateQueued` parameter order. That harness failure is evaluation infrastructure only and is not attributed to the model candidate; see [v1 invalid harness evidence](evidence/q-002f-contract-tests-v1-build.md).

The corrected frozen BCL-only suite is `q-002f-contract-tests-v2`; it uses the actual public signatures of Q-002F:

- [ContractTests.csproj](tests/q-002f-contract-tests-v2/ContractTests.csproj) — SHA-256 `298A28857774A38586CE3D0A6521EFD5E0D6FE756F26F77D362C0CB3B4F4FE44`
- [Program.cs](tests/q-002f-contract-tests-v2/Program.cs) — SHA-256 `046E590E21D2D7178B2C634E4EE8B0C39D71D716CB1699664EE024F530231D55`

Observed result: **7 pass / 1 fail.** The sole failure is `Claim creates a distinct new concurrency token`: Q-002F reuses `expectedConcurrencyToken` in the `Processing` record. This fails the contract's new-token requirement. The signature does not include a separate new-token argument, so the contract is ambiguous about how the value is supplied; that ambiguity does not make the failing frozen check pass. See [Q-002F contract-test record](evidence/q-002f-contract-tests-v2.md).

**Q-002F verdict: Testable after format-only fence removal, but contract fail.** It measures the human-plus-model Q-002F derivative and cannot revise the autonomous Q-002 failure.

### Q-002F Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Physical-path build | Restore completed; `MSB3030` output-copy failure after 2.0 s |
| Attribution | Invalid infrastructure invocation; Windows path limit later isolated |
| Short-path build | Pending |

## Q-003 — Independent Contract-Test Generation

The frozen human reference passed 4/4 baseline checks and all four mutants compiled before model capture.

| Criterion | Observed result |
| --- | --- |
| Raw evidence | `q-003-ornith-raw.cs`, SHA-256 `C5176E1C80F429F9EE6D397CB4C9E1C3EA526989FCA5F1D987F15F7DAB448622` |
| Literal build | **Fail** — opening/closing Markdown fences at lines 1 and 232 |
| Correct-reference / mutant execution | N/R — not run after build failure |
| Verdict | **Fail — raw-source compile gate** |

The initial project also had an incorrect relative reference path. It was corrected after Q-003 closure and validated by a human harness preflight; Q-003 itself was not rerun.

### Q-003 Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Client elapsed | 1,751.167 s |
| Prompt evaluation | 487 tokens; 2.439 s; 199.71 tokens/s |
| Generation | 34,324 tokens; 1,748.587 s; 19.63 tokens/s |
| Total server time | 1,751.025 s; 34,811 tokens |
| Transport / context | HTTP 200; 34,810 cached tokens; `truncated=false` |
| Literal build | 8 compiler errors; project-reference warning also recorded |

### Q-003F — Authorized Fence-Only Derivation

Exact outer fences were removed after hash verification; derived SHA-256: `0EFFD2F3DA6075D98BBAB21BBDC5440F6DB81A03369C137293D83B60A9714A48`.

The corrected human reference and harness compiled first. The derived source then failed with `CS1513` (`}` expected) at line 230. No repair, execution, or mutant measurement occurred. **Verdict: Fail — C# compile gate.** See [Q-003F build](evidence/q-003f-literal-build.md).

### Q-003F Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Reference/harness preflight | Build succeeded in 1.5 s; executable resolved `Forge.DocumentIntake.IntakeRequest` |
| Derived build | Reference project succeeded; generated derivative failed with `CS1513` after 1.1 s |

### Q-003H — Authorized Minimal Human Repair

Q-003H is a separate human-plus-model diagnostic. It starts from the immutable Q-003F fence-only artifact and permits exactly one source change: append the missing final closing brace identified by `CS1513`. It does not reopen Q-003/Q-003F and does not permit any semantic edit or test alteration.

The repair scope, parent hash, and required compile/test sequence are frozen in the [Q-003H manifest](evidence/q-003h-minimal-repair-manifest.md). The corrected reference build passed, but the generated harness then passed only 6 of 8 checks against that reference: expired-lease reclaim and matching completion each returned `NotClaimable` where the test expected success. See [Q-003H reference execution](evidence/q-003h-reference-execution.md).

**Q-003H verdict: Testable after one human closing-brace repair, but reference-contract fail.** The reference gate failure prevents mutant measurement. No generated test, reference source, or mutant was changed.

## Q-004 — C# Implementation and Test Review

**Status:** Closed — fail review-quality gate.

The read-only template is frozen at SHA-256 `93FC353FBF99B613C33655E24947BA5DB0873EA292551342FDCBBB4C2F68B85A`. The capture script verified the frozen contract, CC-002 v2 implementation, and test-harness hashes before composing the request. The hidden scoring baseline, prior outputs, and repair history were not included.

| Criterion | Observed result |
| --- | --- |
| Raw reviewer output | [q-004-ornith-review-raw.txt](evidence/q-004-ornith-review-raw.txt), SHA-256 `1FAB8DFE22E95C14135C376134B603D9562907DD4415547496BD39B53415521A` |
| Required response format | Pass — four labeled sections in the required order; no table or code fence. |
| Review-only boundary | Pass — no replacement or modified code was emitted. |
| Severity and traceability | Pass — reported findings identify contract requirements, symbols, and test evidence. |
| True material findings | 3 of 4 — absent client idempotency-key parameter, exposed mutable fingerprint bytes, and reused claim token. |
| Omitted material defect | Query omission from fingerprint construction: `normalizedUri.LocalPath` excludes the query. |
| False material findings | None. |
| False technical conclusion | The output states that `LocalPath` includes the query and that the harness returns three failures; that conclusion conflicts with the submitted artifacts and the omitted query defect. |
| Quality verdict | **Fail — partial recognition is insufficient for autonomous review sign-off.** |

### Q-004 Run Diagnostics (not quality scoring)

| Metric | Observed value |
| --- | --- |
| Client elapsed | 2,426.414 s |
| Prompt evaluation | 4,210 tokens; 4.852 s; 867.60 tokens/s |
| Generation | 46,878 tokens; 2,421.378 s; 19.36 tokens/s |
| Total server time | 2,426.230 s; 51,088 tokens |
| Transport / context | HTTP 200; 51,087 cached tokens; `truncated=false` |

See [Q-004 human review](evidence/q-004-review.md). Q-004 is independent of Q-001 through Q-003 and does not revise their verdicts.

## Recording Rules

- Preserve and hash raw model content before review or transformation.
- Literal compilation precedes tests; tests precede semantic review.
- A derivative is separately named and cannot overwrite an autonomous verdict.
- Record infrastructure events separately from model-quality findings; use `N/R` rather than inference.
- Changed prompt, fixture hash, runtime configuration, context, cache, batch, or reasoning profile requires a new run version.

## Related Records

- [Execution manifest](execution-manifest.md)
- [Execution events](execution-events.md)
- [Operator runbook](operator-runbook.md)
- [Historical draft register](quality-evaluation-register-v1.old.md)
