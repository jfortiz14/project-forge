# CC-003B Quality Evaluation Execution Events

This is a chronological operational history. It distinguishes preflight/client events from executed quality units so a tooling issue is not misreported as model behavior. Local filesystem paths are intentionally omitted.

| Sequence | Event | Evidence / outcome | Effect on evaluation |
|---:|---|---|---|
| 1 | Preflight recorded | .NET 10.0.400; ik_llama.cpp version 4848 (`0ed847d3`); baseline free RAM 49,444.94 MiB and free VRAM 6,674 MiB. | Valid quality environment baseline. |
| 2 | Q-001 server startup | 196,608 context, 2,048 batch/microbatch, `q4_0/q4_0` KV, 42/42 layers offloaded, CPU projector, and no-thinking profile observed. | Valid fresh server for Q-001. |
| 3 | Initial Q-001 client request cancelled | Windows PowerShell warned that `Invoke-WebRequest` might parse web content. The operator selected `N`; no response was received. Subsequent script lines created a zero-byte local raw file with SHA-256 `E3B0…B855`. | **Not a quality-unit execution.** Invalid zero-byte response/raw artifacts were removed before the valid attempt. |
| 4 | Capture hardening | `-UseBasicParsing` and `ErrorActionPreference=Stop` added to the operator procedure. | Prevented parsing prompt and failure fall-through; no server-profile change. |
| 5 | Q-001 executed | HTTP 200; raw SHA-256 `F97D61C262B87190E5006D0099B32C42A7AD5BE55E64283B03D46E86DDBAF3F6`; no truncation. | Executed and closed **fail** for 624 words against a 450–600 limit. |
| 6 | Q-001C prompt-path failure | Windows PowerShell could enumerate the prompt file but `Test-Path` and `ReadAllText` failed on its full path. The Q-001C request had not been sent. | **Not a quality-unit execution.** Diagnosed as legacy `MAX_PATH` behavior. |
| 7 | Long-path workaround | Extended-length `\\?\` paths used only for .NET file read/write/hash operations; logical evidence names and content remained unchanged. | Valid transport workaround, documented in the runbook. |
| 8 | Q-001C executed | HTTP 200; raw SHA-256 `35AB417E51D86CFE780BA46FC44360EE3A326D087301454B06F83AAAAC25EA65`; no truncation. | Sole corrective planning pass executed and closed **fail**: 632 words and material corrective gaps. |
| 9 | Q-002 executed | HTTP 200; raw SHA-256 `DA31139A7443ABCD5B275903C7F991F25AEF2917F0C0F1DF9640DF40975369E9`; no truncation. | Raw source captured before build. |
| 10 | Q-002 literal compile gate | 7 compiler errors: opening and closing Markdown fences generated backticks at lines 1 and 298. | Q-002 closed **fail**. No source transformation, test, or semantic review. |
| 11 | Q-002F fence-only derivative | Operator-authorized derivation verified the Q-002 source hash and removed only exact outer fence lines. Derived SHA-256 `55301E6A90759C642874F2EB1F1407E0B20FF484324694FC09D3C6AD490EC699`. | Separate human-intervened measurement; does not alter Q-002. |
| 12 | Q-002F literal compile gate | Restore completed, then `MSB3030`: expected output DLL not found for copy; no C# diagnostic was emitted. | Physical-path invocation is invalid infrastructure evidence after the path boundary was isolated. At this chronological point, the immutable derivative awaited a valid short-path build; no clean/rebuild retry or further transformation was performed. |
| 13 | Q-002F post-closure artifact inspection | Intermediate `obj` DLL/PDB were present while final `bin` DLL was absent. | Supports an output-copy/build-system classification; does not reopen Q-002F or establish semantic correctness. |
| 14 | Q-003 harness preflight physical-path invocation | `dotnet` rejected the known-valid preflight project because the fully qualified project path exceeded the 260-character OS limit, before compilation. | Confirms a workspace-path infrastructure boundary. Q-003 remains unrequested; preflight must be reattempted through a temporary short mapped drive. |
| 15 | Q-003 raw capture | HTTP 200; raw SHA-256 `C5176E1C80F429F9EE6D397CB4C9E1C3EA526989FCA5F1D987F15F7DAB448622`; 34,810 cached tokens; no truncation. | Capture complete. Literal build is blocked pending mapped-drive harness preflight; raw source remains uninspected. |
| 16 | Q-003 harness preflight through temporary mapped drive | Known-valid human-authored project restored, compiled, and produced its output assembly successfully in 1.9 seconds. | Infrastructure preflight passed. Q-003 literal build may proceed through the same mapped drive. |
| 17 | Q-003 literal compile gate | 8 errors from opening/closing Markdown fences at lines 1 and 232. The build also warned that the original Q-003 relative reference path to the human project was invalid. | Q-003 closed **fail** on its independent raw-source format violation. Harness reference path corrected post-closure for future separately authorized work; no Q-003 rerun. |
| 18 | Q-003F corrected-reference preflight | Frozen human reference and Q-003F harness built successfully; executable printed `Forge.DocumentIntake.IntakeRequest`. | The corrected Q-003F project reference is valid through the short mapped drive. Fence-only derivation may proceed. |
| 19 | Q-003F fence-only derivative | Q-003 source hash re-verified; exact outer fences removed and no other text transformed. Derived SHA-256 `0EFFD2F3DA6075D98BBAB21BBDC5440F6DB81A03369C137293D83B60A9714A48`. | Derived artifact awaits literal build through the preflight-validated harness. |
| 20 | Q-003F literal compile gate | Corrected human reference compiled. Derived harness then failed with `CS1513` (`}` expected) at line 230. | Q-003F closed **fail** at a source-level C# diagnostic; no execution or mutant measurement. |
| 21 | Q-004 executed | HTTP 200; raw SHA-256 `1FAB8DFE22E95C14135C376134B603D9562907DD4415547496BD39B53415521A`; 51,087 cached tokens; `truncated=false`. | Immutable read-only review capture complete. |
| 22 | Q-004 post-capture review | Required four-section format, review-only boundary, severity, and artifact traceability were met. Three hidden material defects were identified; query omission from fingerprint construction was missed. The response also incorrectly stated that `LocalPath` includes the query and that the harness has three failures. | Q-004 closed **fail**. Partial defect recognition is not autonomous approval. |
| 23 | Q-002F valid short-path literal compile gate | Restore completed in 0.3 s. `OrnithQ002FenceOnly` succeeded in 1.3 s and emitted its `net10.0` DLL; total build time was 2.0 s with no warnings/errors. | Q-002F compiles and is eligible for frozen contractual tests. This does not alter Q-002's autonomous literal-source failure. |
| 24 | Q-002F contractual-test harness v1 | The first BCL-only harness failed to compile because it targeted a different fixture API: missing required `blobReference` arguments, string-versus-byte-array fingerprint assumption, and incompatible `CreateQueued` calls. | Invalid evaluation-harness attempt; not attributed to Q-002F source. Preserved as history. |
| 25 | Q-002F contractual-test harness v2 frozen | New BCL-only harness targets Q-002F's actual signatures and retains the same eight contract clauses. | Ready for one recorded execution through the short mapped drive. |
| 26 | Q-002F contractual tests v2 | 7 checks passed; `Claim creates a distinct new concurrency token` failed because the claimed record reused `expectedConcurrencyToken`. | Q-002F closed **testable but contract fail**. Contract signature/token-supply ambiguity is recorded; no repair was applied. |
| 27 | Q-003H minimal human repair authorized | One append-only closing-brace repair to the immutable Q-003F artifact is authorized to address `CS1513`. | Separate human-plus-model unit; at this chronological point compilation, reference execution, and mutants remained pending. |
| 28 | Q-003H corrected-reference compile gate | Repaired artifact SHA-256 `ACA6B759C8439815C50BAAA2DAC93F4D935E106B66BC2D66FA54EC947F354BE9`. Human reference compiled in 0.1 s; Q-003H generated-test project compiled in 0.3 s; overall build 1.2 s. | Compile gate passed. Correct-reference execution is next; mutant measurement remains gated. |
| 29 | Q-003H correct-reference execution | Six checks passed. Expired-lease reclaim and matching completion each failed with `NotClaimable` against the frozen human reference. | Q-003H closed **testable but reference-contract fail**. Generated tests are not eligible for mutant measurement. |

## Current Quality State

- Q-001: failed; planning output exceeded the word limit.
- Q-001C: failed; sole corrective planning pass consumed.
- Q-002: failed at literal compile gate because of Markdown fences.
- Q-002F: physical-path build invalidated by infrastructure; immutable derivative subsequently compiled and passed 7/8 contractual tests, failing claim-token rotation.
- Q-003: failed at literal compile gate; Q-003F failed with `CS1513` after fence-only removal.
- Q-004: failed review-quality gate; 3 of 4 material defects identified, with the URI-query fingerprint defect omitted.

No completed autonomous quality unit is approved. Q-002F is an independent human-plus-model format-only derivation and is closed testable but contract fail.
