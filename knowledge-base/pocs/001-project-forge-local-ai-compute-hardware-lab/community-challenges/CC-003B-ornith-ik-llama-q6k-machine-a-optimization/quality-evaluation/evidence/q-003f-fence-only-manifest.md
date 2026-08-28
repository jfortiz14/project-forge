# Q-003F Authorized Fence-Only Derivation Manifest

**Status:** Closed — C# compile gate failed.  
**Relationship:** A separate human-intervened format-only measurement. It cannot alter the closed Q-003 autonomous verdict.

## Authorization and Scope

The operator authorized removal of only the exact outer opening and closing Markdown fence lines from Q-003 raw output. No other content change, extraction, repair, formatting rewrite, prompt rerun, or mutation of the raw artifact is permitted.

| Field | Value |
|---|---|
| Immutable source | `q-003-ornith-raw.cs` |
| Source SHA-256 | `C5176E1C80F429F9EE6D397CB4C9E1C3EA526989FCA5F1D987F15F7DAB448622` |
| Required shape | Exact outer opening ` ```csharp ` or ` ```cs ` line and closing ` ``` ` line |
| Derived output | `q-003f-ornith-fence-only.cs` |
| Derived SHA-256 | `0EFFD2F3DA6075D98BBAB21BBDC5440F6DB81A03369C137293D83B60A9714A48` |
| Reference preflight | Pass — reference and harness built; run printed `Forge.DocumentIntake.IntakeRequest` |
| Derived build / execution | Fail — `CS1513` (`}` expected) at line 230; not executed |

## Sequence

1. Build and run `HarnessReferencePreflight.csproj` through the short mapped drive; it must resolve the frozen human reference.
2. Verify source hash and exact outer-fence shape.
3. Create and hash the derivative.
4. Compile the derivative through `OrnithQ003FenceOnlyTests.csproj`.
5. Run it against the correct reference only if compilation succeeds.
6. Run applicable mutant measurements only if the correct-reference run succeeds.

The derived build reached a C# source diagnostic after the corrected reference compiled. See [Q-003F literal build](q-003f-literal-build.md). Q-003F is closed without further transformations or retries.
