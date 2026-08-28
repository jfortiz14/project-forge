# Q-003 Literal Compile Gate

**Unit:** Q-003 independent contract-test generation  
**Raw artifact:** `q-003-ornith-raw.cs`  
**Raw SHA-256:** `C5176E1C80F429F9EE6D397CB4C9E1C3EA526989FCA5F1D987F15F7DAB448622`  
**Verdict:** **Fail — raw-source compile gate**

## Observed Build Result

The raw generated test harness failed compilation with 8 errors and one project-reference warning.

The conclusive source diagnostics are:

- Opening Markdown fence: three `CS1056` backtick errors at line 1 and `CS0116` at line 1, column 4.
- Closing Markdown fence: three `CS1056` backtick errors at line 232 and `CS1513` at line 232, column 4.

These errors independently establish that the output violated the required raw-C# boundary.

The build also emitted `MSB9008` because the Q-003 project initially used an incorrect relative path to the frozen reference project. The harness path was corrected after this terminal Q-003 build for any future separately authorized derivation, but Q-003 was not rerun and its raw artifact was not modified.

No execution against the correct reference, mutant measurement, source transformation, or semantic review was performed. Q-003 is closed. Any fence-only derivative requires separate explicit authorization and a new unit.
