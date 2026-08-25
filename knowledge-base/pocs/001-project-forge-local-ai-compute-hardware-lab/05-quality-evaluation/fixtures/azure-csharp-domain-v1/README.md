# Azure C# Domain Fixture v1

Synthetic/public fixture for Q-002 through Q-004. It implements the human-adjudicated `IntakeRequest`/`IntakeRecord` API, including explicit claim-token rotation.

## Frozen Files

| File | SHA-256 |
| --- | --- |
| `src/Forge.DocumentIntake.Reference.csproj` | `FC05E8B0A79435FCA5D5A73D3CB43646FE1C2747E580EF1142F22A76D74076AA` |
| `src/IntakeDomain.cs` | `B568D4113F1C8939AAE8893DB024DAA96DA52745281B752E39C2AA4CDCAC8CEF` |
| `baseline-tests/Forge.DocumentIntake.BaselineTests.csproj` | `5F3E2C4C47170ACE5DC21CC2AA9C994B8618863BBD9A616B6712FF040DF1EC2F` |
| `baseline-tests/Program.cs` | `5B4A035C8D24D7097EE25FA13F09A1C4963A90B1D5A984A0EFA8CD43AB0583BB` |
| `mutants/MUT-001-no-idempotency-validation.cs` | `9EACD0A01F0CFF0E59D490E37E820CB8D3F5676DBCA7AE0EAF5BC11E3379F651` |
| `mutants/MUT-002-query-omitted-from-fingerprint.cs` | `937A1BFA9FC3ED078AE28090CD9C928D9CCB2BD9677A597AFA4ABF1DA237C383` |
| `mutants/MUT-003-claim-token-not-rotated.cs` | `3C862CA2F967869D8B81B04FA5308C6E892DCD10616D6174E6553FD06FCBC83D` |
| `mutants/MUT-004-active-lease-reclaim.cs` | `F20A1B44FCDA719385CF9FA2F8FD0403DA52A663EBC76A2716F3FE06235F8369` |

## Baseline

Run `dotnet run --project baseline-tests/Forge.DocumentIntake.BaselineTests.csproj`. Expected: 4 passes, 0 failures. Each mutant must compile before use; a generated suite should pass the reference and fail its applicable mutants.
