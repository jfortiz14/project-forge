# Q-004 — Human Review of Ornith Output

## Immutable Capture

| Field | Observed value |
| --- | --- |
| Raw artifact | `q-004-ornith-review-raw.txt` |
| SHA-256 | `1FAB8DFE22E95C14135C376134B603D9562907DD4415547496BD39B53415521A` |
| Transport | HTTP 200; `truncated=false` |
| Client elapsed | 2,426.414 s |
| Server timing | 4,210 prompt tokens in 4.852 s (867.60 tokens/s); 46,878 generated tokens in 2,421.378 s (19.36 tokens/s); 51,088 total tokens in 2,426.230 s |

The hash-verified review template, domain contract, implementation fixture, and test fixture were submitted as read-only inputs. The hidden scoring baseline was not included in the request.

## Contract-Review Assessment

| Criterion | Observed result |
| --- | --- |
| Required four labeled sections | **Pass** — all four are present in the required order. |
| Review-only boundary | **Pass** — no replacement or modified C# was emitted. |
| Severity and traceability | **Pass** — each reported material finding names a contract requirement and concrete symbol/test evidence. |
| True material findings | **3 of 4** — the absent client idempotency-key parameter, exposed mutable fingerprint bytes, and reused claim concurrency token were correctly identified. |
| Material defect omitted | **1 of 4** — fingerprint construction appends `normalizedUri.LocalPath`, which excludes the URI query; query-distinct blob references can therefore collide. |
| False material finding | None. |
| False technical conclusion | The response states that `Uri.LocalPath` includes the query and therefore the query test passes, then asserts `RESULT failures=3`. Both claims conflict with the supplied implementation/test artifacts: `LocalPath` excludes the query, so the query-distinct fingerprint check is another failure. |
| Additional-validation scope | Useful, but not a substitute for correctly identifying the omitted material defect. |

## Verdict

**Fail — non-compliant review-quality result.** The output followed the required review structure and correctly found three material defects without leaving the review-only boundary. It nevertheless omitted one of four hidden material defects and reached a contrary technical conclusion about URI query handling and the harness failure count. It is not evidence for autonomous review sign-off.
