# Q-001C Contract Review

**Unit:** Q-001C single authorized corrective planning pass  
**Raw artifact:** `q-001c-ornith-raw.txt`  
**Raw SHA-256:** `35AB417E51D86CFE780BA46FC44360EE3A326D087301454B06F83AAAAC25EA65`  
**Transport outcome:** HTTP 200; no context truncation reported  
**Verdict:** **Fail — output-format and corrective-requirement gates**

## Contract Checks

| Check | Result | Evidence |
|---|---|---|
| Six required labeled sections | Pass | All six required headings are present and in order. |
| No implementation code, tables, citations, or external tools | Pass | No prohibited output form was observed. |
| 450–600 words | **Fail** | 632 whitespace-delimited words; exceeds the maximum by 32 words. |
| Atomic create-or-return admission | **Fail** | It returns the existing row on any 409 but defines neither a same-key/different-request fingerprint conflict nor durable reconciliation for a status write that succeeds while enqueue fails. |
| Crash-safe duplicate external-effect rule | **Fail** | Checking a prior completion before an effect does not prevent a duplicate after an effect succeeds and the worker crashes before recording completion. No external-effect idempotency key or equivalent durable boundary is defined. |
| Least-privilege role design | **Fail** | The response uses one shared system-assigned identity and assigns the worker unnecessary send permission; `Lease` is presented as a Service Bus role rather than a defined authorization boundary. This does not clearly establish distinct least-privilege API and worker roles. |
| Blob reference authorization and DLQ ownership/replay | Pass | It defines allowed container/path scope validation and manual, authorized, tracked DLQ replay owned by operations. |

No repair, trimming, or regeneration was applied. Q-001C consumed the one authorized corrective pass.

## Diagnostic Timing

| Metric | Observation |
|---|---:|
| Client elapsed | 587.460 s |
| Prompt evaluation | 432 tokens at 64.03 tokens/s |
| Generation | 11,640 tokens at 20.05 tokens/s |
| Total server time | 587.373 s for 12,072 tokens |

Timing is diagnostic only and does not affect the verdict.
