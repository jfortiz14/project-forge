# Q-003H Minimal Human Repair Manifest

**Status:** Closed — testable but reference-contract fail.  
**Relationship:** Separate human-plus-model measurement derived from the closed Q-003F fence-only artifact. It cannot revise Q-003 or Q-003F.

## Frozen Source and Permitted Change

| Field | Value |
| --- | --- |
| Parent artifact | `q-003f-ornith-fence-only.cs` |
| Parent SHA-256 | `0EFFD2F3DA6075D98BBAB21BBDC5440F6DB81A03369C137293D83B60A9714A48` |
| Parent diagnostic | `CS1513`: `}` expected at line 230 |
| Derived artifact | `q-003h-ornith-minimal-repair.cs` — SHA-256 `ACA6B759C8439815C50BAAA2DAC93F4D935E106B66BC2D66FA54EC947F354BE9` |
| Permitted repair | Append exactly one closing-brace line at end of file. |
| Not permitted | Any other source edit, reformat, refactor, test alteration, generated-test regeneration, or mutation to parent evidence. |

The repaired artifact was copied from Q-003F byte-for-byte and then received one final `}` line. The parent hash remains `0EFFD2F3DA6075D98BBAB21BBDC5440F6DB81A03369C137293D83B60A9714A48`; the derived hash is recorded above. Through the short mapped drive, the frozen human reference built in 0.1 seconds and `OrnithQ003HumanRepairTests` built in 0.3 seconds; the overall build completed in 1.2 seconds.

Only if the repaired generated tests compile may they run against the frozen human reference and then against the frozen mutants. Results are human-plus-model evidence, never autonomous generated-test acceptance.
