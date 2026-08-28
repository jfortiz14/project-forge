# Q-003H Correct-Reference Execution

**Unit:** Q-003H minimal human repair, human-plus-model measurement  
**Generated test artifact SHA-256:** `ACA6B759C8439815C50BAAA2DAC93F4D935E106B66BC2D66FA54EC947F354BE9`  
**Result:** **6 pass / 2 fail against the frozen human reference**

## Complete Observed Output

```text
PASS: blank idempotency key is rejected with ArgumentException
PASS: equivalent URI plus reordered metadata yields the same fingerprint
PASS: different URI query values produce different fingerprints
PASS: a queued record claims with the expected new token
PASS: reusing the same claim token is rejected with ArgumentException
PASS: an active unexpired lease cannot be claimed by a second worker
FAIL: an expired lease can be reclaimed by a second worker with a new token (AssertionException: expected Claimed but got NotClaimable)
FAIL: a matching worker and token completes the record and clears the lease (AssertionException: expected Completed but got NotClaimable)
RESULT: 2 check(s) failed
```

## Interpretation

The repaired generated suite is executable, but it does not pass the frozen correct-reference entry gate. Its expectations for expired-lease reclaim and completion are incompatible with the reference behavior under the supplied calls. Therefore it cannot be used to measure mutant detection.

No generated test was changed after this result, no reference source was changed, and no mutant was run. Q-003H is a separate human-plus-model result and does not alter the closed autonomous Q-003 or Q-003F failures.

## Verdict

**Testable after one minimal human repair, but reference-contract fail.** No mutant score exists.
