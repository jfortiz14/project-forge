# CC-003B Quality Results

> **Challenge:** `FORGE-CC-003B`
> **Phase:** Quality evaluation
> **Status:** Q-001/Q-001C planning, Q-002, Q-003, and Q-004 are closed as failed; Q-002F is testable but contract fail (7/8 checks pass).

## Preflight

| Observation | Result |
|---|---|
| .NET SDK | 10.0.400 |
| Server binary | version 4848 (`0ed847d3`), MSVC 19.44.35228.0, x64 |
| Baseline free RAM | 49,444.94 MiB |
| Baseline VRAM | 1,345 MiB used / 6,674 MiB free of 8,192 MiB |
| NVIDIA driver | 576.88; driver reports CUDA 12.9 |

The baseline is sufficient to begin Q-001. It is an observation, not a resource reservation or a guarantee that later units will have identical headroom.

## Q-001 — Azure/C# Planning

| Field | Result |
|---|---|
| Raw artifact SHA-256 | `F97D61C262B87190E5006D0099B32C42A7AD5BE55E64283B03D46E86DDBAF3F6` |
| Transport / context | HTTP 200; 3,398 cached tokens; `truncated=false` |
| Client elapsed | 159.378 s |
| Verdict | **Fail — 624 words exceeds the frozen 450–600-word limit** |

All six required sections were present and no prohibited output form was observed. The response is nevertheless a failure because it violates an explicit output constraint. See [Q-001 contract review](evidence/q-001-review.md).

## Q-001C — Corrective Azure/C# Planning

| Field | Result |
|---|---|
| Raw artifact SHA-256 | `35AB417E51D86CFE780BA46FC44360EE3A326D087301454B06F83AAAAC25EA65` |
| Transport / context | HTTP 200; 12,071 cached tokens; `truncated=false` |
| Client elapsed | 587.460 s |
| Verdict | **Fail — 632 words exceeds the maximum, and material corrective requirements remain incomplete** |

The response covered blob-scope authorization and manual DLQ replay, but did not define a same-key/different-request conflict or durable enqueue reconciliation, did not provide a crash-safe external-effect boundary, and did not establish a precise least-privilege split. See [Q-001C contract review](evidence/q-001c-review.md). The sole corrective planning pass is consumed; no further planning generation is allowed.

## Q-002 — Independent C# Domain Implementation

| Field | Result |
|---|---|
| Raw artifact SHA-256 | `DA31139A7443ABCD5B275903C7F991F25AEF2917F0C0F1DF9640DF40975369E9` |
| Transport / context | HTTP 200; 58,208 cached tokens; `truncated=false` |
| Client elapsed | 2,937.000 s |
| Literal build | **Fail — 7 compiler errors caused by opening and closing Markdown fences** |
| State | Closed at compile gate; no test, semantic review, transformation, or repair performed. |

See [Q-002 literal compile gate](evidence/q-002-literal-build.md). Any derivative experiment requires separate authorization and cannot revise this autonomous result.

## Q-002F — Fence-Only Derivation

The operator authorized one separate fence-only derivation before Q-003. It removed only exact outer Markdown fence lines from the immutable Q-002 artifact. The derived artifact hash is `55301E6A90759C642874F2EB1F1407E0B20FF484324694FC09D3C6AD490EC699`; its physical-path build ended with `MSB3030`. Later short-drive preflight isolated Windows path/output infrastructure as the confounder. **That physical invocation is invalid; the unchanged Q-002F artifact subsequently passed its valid short-path compile gate in 2.0 seconds with no warnings/errors.** The corrected contractual suite then passed 7 of 8 checks, failing only claim-token rotation. See the [Q-002F contract-test record](evidence/q-002f-contract-tests-v2.md). Q-002 remains independently failed at its literal compile gate.

## Q-003 — Independent Contract-Test Generation

| Entry gate | Result |
|---|---|
| Human reference baseline tests | Pass — 4 checks passed; 0 failures. |
| Frozen mutant compilation | Pass — MUT-001 through MUT-004 each compiled successfully. |
| Model output | Captured; SHA-256 `C5176E1C80F429F9EE6D397CB4C9E1C3EA526989FCA5F1D987F15F7DAB448622` |
| Capture timing | 1,751.167 s client elapsed; HTTP 200; 34,810 cached tokens; `truncated=false` |

The Q-003 reference is valid for test-generation measurement. The raw generated test harness failed its literal compile gate due to Markdown fences; see [Q-003 literal build](evidence/q-003-literal-build.md). No execution, mutation measurement, or review was performed.

### Q-003F — Fence-Only Derivation

The operator-authorized derivation removed only exact outer fences and retained hash `0EFFD2F3DA6075D98BBAB21BBDC5440F6DB81A03369C137293D83B60A9714A48`. With the corrected reference harness, it reached a C# source error: `CS1513` (`}` expected) at line 230. No test execution or mutation measurement occurred. Q-003F is closed; see [Q-003F literal build](evidence/q-003f-literal-build.md).

### Q-003H — Minimal Human Repair

The authorized append-only closing-brace repair produced SHA-256 `ACA6B759C8439815C50BAAA2DAC93F4D935E106B66BC2D66FA54EC947F354BE9` and compiled against the frozen reference. The generated harness then passed 6 of 8 checks against that reference. Its expired-lease-reclaim and matching-completion checks expected success but observed `NotClaimable`. **Q-003H is testable but reference-contract fail; no mutant measurement occurred.** See [Q-003H reference execution](evidence/q-003h-reference-execution.md).

### Q-003 Harness Preflight

**State:** Pass. Physical-path invocation was blocked by Windows `MAX_PATH`, but the known-valid human-authored project built successfully through the temporary short mapped drive in 1.9 seconds and produced its output assembly. Q-003 may now proceed to its literal build gate through the same mapping.

## Q-004 — C# Implementation and Test Review

| Field | Result |
|---|---|
| Raw artifact SHA-256 | `1FAB8DFE22E95C14135C376134B603D9562907DD4415547496BD39B53415521A` |
| Transport / context | HTTP 200; 51,087 cached tokens; `truncated=false` |
| Client elapsed | 2,426.414 s |
| Required format and review-only boundary | Pass — four required sections; no code modifications or replacement code. |
| Known material defects identified | 3 of 4 — idempotency-key parameter, mutable fingerprint, and claim-token reuse. |
| Verdict | **Fail — query omission from fingerprint construction was missed, and the review incorrectly stated that `LocalPath` includes the query.** |

See [Q-004 review assessment](evidence/q-004-review.md). Partial defect recognition does not provide autonomous review approval.

No quality outcome is inferred from CC-003B capacity or performance evidence. Results appear here only after each quality unit has immutable raw evidence and its required gates.
