# Q-002F Contract Tests v1 — Invalid Harness Attempt

**Status:** Invalid evaluation-harness attempt; superseded by `q-002f-contract-tests-v2`.

The v1 harness did not compile against the already compiled Q-002F derivative. Its assumptions were copied from a different fixture API and were not valid for this candidate:

- `IntakeRequest.Create` calls omitted the required `blobReference` parameter.
- The harness treated `Fingerprint` as a mutable byte array, but Q-002F exposes an immutable `string`.
- `IntakeRecord.CreateQueued` calls used an incompatible argument order.
- The harness expected `LeaseExpiresAtUtc`, while Q-002F exposes `LeaseExpiryUtc`.

The compiler emitted 17 errors, including `CS7036`, `CS0200`, `CS1503`, and `CS1061`. This attempt provides no contract result and is not attributed to model-generated source. No change was made to Q-002F; the separately frozen v2 harness was created against the actual compiled public API.
