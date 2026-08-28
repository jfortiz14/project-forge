# CC-003B Quality Evaluation — Frozen Inputs

All paths below are read-only sources. CC-003B records their identities and hashes but does not copy, modify, or re-adjudicate them.

## Canonical Method Sources

| Asset | Source | SHA-256 |
| --- | --- | --- |
| Quality contract | `../../../05-quality-evaluation/forge-quality-contract-v1-software-architecture-coding.md` | `88997FE67CD946A336E45C5C5480B321D43F20FC6FD3ADDB92785CC70B33992C` |
| Reference architecture | `../../../05-quality-evaluation/azure-csharp-reference-architecture-v1.md` | `8A101E93A37A2F32A40C080DF58636B426358CC78339EBCF7B3C1AE08F52ACCC` |
| Domain contract | `../../../05-quality-evaluation/azure-csharp-domain-contract-v1.md` | `81DEC1163E9A16E7F3ACAF28AB2BE35A8B0F9624241B7FCEE82FFB4A0BA9F6DB` |
| Quality protocol | `../../../05-quality-evaluation/protocol/quality-evaluation-protocol-v1.md` | Record before execution if the protocol changes |

## Q-003 Human Reference

Use the canonical `azure-csharp-domain-v1` fixture. Its frozen reference implementation is:

| Asset | Source | SHA-256 |
| --- | --- | --- |
| Reference implementation | `../../../05-quality-evaluation/fixtures/azure-csharp-domain-v1/src/IntakeDomain.cs` | `FB734AB5F851C80CF5C79E78CF9BDA44884EE3FFD93436F800BB54BE2BADF196` |
| Reference project | `../../../05-quality-evaluation/fixtures/azure-csharp-domain-v1/src/Forge.DocumentIntake.Reference.csproj` | `FC05E8B0A79435FCA5D5A73D3CB43646FE1C2747E580EF1142F22A76D74076AA` |
| Baseline test project | `../../../05-quality-evaluation/fixtures/azure-csharp-domain-v1/baseline-tests/Forge.DocumentIntake.BaselineTests.csproj` | `5F3E2C4C47170ACE5DC21CC2AA9C994B8618863BBD9A616B6712FF040DF1EC2F` |
| Baseline test source | `../../../05-quality-evaluation/fixtures/azure-csharp-domain-v1/baseline-tests/Program.cs` | `5B4A035C8D24D7097EE25FA13F09A1C4963A90B1D5A984A0EFA8CD43AB0583BB` |

The four frozen mutants and their detection signals are defined in the canonical fixture README. Verify the fixture baseline and mutant compilation before Q-003.

## Q-004 Review Fixture v2

For CC-002 comparability, Q-004 uses CC-002's frozen v2 fixture as read-only input. Before composing the review prompt, verify:

| Asset | Source | SHA-256 |
| --- | --- | --- |
| Implementation | `../../CC-002-qwen/quality-evaluation/fixtures/q-004-review-fixture-v2/implementation/OrnithQ002H.minimal-repair.cs` | `2380F6A745567B848F17DE90B6AFB548786811E79D9D6F5E8841BC9FCB70B3F9` |
| Implementation project | `../../CC-002-qwen/quality-evaluation/fixtures/q-004-review-fixture-v2/implementation/Forge.DocumentIntake.Fixture.csproj` | `FCFDC71065451BA8940895F1D44451D9BB31D6E5ABF13F5F95E630198968C221` |
| Test harness | `../../CC-002-qwen/quality-evaluation/fixtures/q-004-review-fixture-v2/tests/Program.cs` | `81417ECFE0C62A8C8CC971E02F93BE8FD4CD79B0AA8D7A91C06AAAA4435F8384` |
| Test project | `../../CC-002-qwen/quality-evaluation/fixtures/q-004-review-fixture-v2/tests/ContractTests.csproj` | `1E8DE50C44A22A558B18557E76D0773F3B9F504E806A12D9DCED167D740C1AF4` |

The hidden Q-004 scoring baseline is not sent to the model. The human evaluator uses it only after raw output is preserved.
