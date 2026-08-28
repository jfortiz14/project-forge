# Q-002F Contract Tests v2 — Execution Record

**Unit:** Q-002F authorized fence-only derivative, human-plus-model measurement  
**Derived SHA-256:** `55301E6A90759C642874F2EB1F1407E0B20FF484324694FC09D3C6AD490EC699`  
**Harness:** `q-002f-contract-tests-v2`  
**Result:** **7 pass / 1 fail**

## Complete Observed Result

```text
PASS Required enum values
PASS Request rejects blank idempotency key
PASS Fingerprint is metadata-order invariant
PASS Fingerprint includes the normalized URI
PASS Request fingerprint is immutable to callers
PASS UTC and lease validation
FAIL Claim creates a distinct new concurrency token: Claim did not apply a distinct new concurrency token.
PASS Completion clears the lease
RESULT failures=1
```

## Contractual Interpretation

`TryClaim` constructs its `Processing` record with `expectedConcurrencyToken`, so the result reuses the input token instead of carrying a distinct new token. This fails the frozen claim-token check and conflicts with the contract statement that a successful claim returns a record with a new supplied concurrency token.

The contract has a design ambiguity: the documented `TryClaim` signature exposes `expectedConcurrencyToken` but no separate `newConcurrencyToken` parameter, unlike `TryComplete` and `TryFail`. That ambiguity does not turn the recorded test into a pass. The implementation did not produce a distinct token, and no repair was applied.

## Verdict

**Testable after format-only fence removal, but contract fail.** Q-002F remains distinct from the permanently failed autonomous Q-002 raw output. This result must be attributed to the unchanged model output plus the operator-authorized removal of the two outer Markdown fences.
