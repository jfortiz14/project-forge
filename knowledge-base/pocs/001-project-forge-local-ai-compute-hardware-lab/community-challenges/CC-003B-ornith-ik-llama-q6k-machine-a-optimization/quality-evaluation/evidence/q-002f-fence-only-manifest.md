# Q-002F Authorized Fence-Only Derivation Manifest

**Status:** Compiles — valid short-path literal build passed.  
**Relationship:** Separate human-intervention measurement derived from the closed Q-002 autonomous artifact. It cannot alter the Q-002 verdict.

## Authorization and Scope

The operator authorized a comparability check modeled on CC-002's `Q-002R2F` fence-only preflight. The only permitted transformation is removal of the exact outer opening and closing Markdown fence lines from the frozen Q-002 raw artifact.

Not permitted:

- removing any non-fence text;
- adding, repairing, formatting, or reordering code;
- extracting an inner code block;
- changing whitespace within the retained body;
- running contractual tests or semantic review unless the derived file first compiles.

## Source and Expected Output

| Field | Value |
|---|---|
| Immutable source | `q-002-ornith-raw.cs` |
| Source SHA-256 | `DA31139A7443ABCD5B275903C7F991F25AEF2917F0C0F1DF9640DF40975369E9` |
| Required source shape | Opening ` ```csharp ` (or ` ```cs `) line and closing ` ``` ` line as the two outer lines |
| Derived output | `q-002f-ornith-fence-only.cs` |
| Compile project | `../build/q-002f-ornith/OrnithQ002FenceOnly.csproj` |
| Derived SHA-256 | `55301E6A90759C642874F2EB1F1407E0B20FF484324694FC09D3C6AD490EC699` |
| Compile result | Physical-path attempt invalid — `MSB3030` under the overlong workspace path; short-path build passed in 2.0 s with 0 warnings/errors |

The source hash was re-verified before the derivative was created. The required outer-fence shape matched. The derivative contains the retained body only. Its physical-path build ended with an MSBuild output-copy failure; later mapped-drive preflight isolated the infrastructure confounder. The unchanged artifact then passed its valid short-path literal build; see [Q-002F literal build](q-002f-literal-build.md). It is eligible for the separately frozen contractual-test gate.
