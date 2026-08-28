# Frozen Quality Prompts

These files are the immutable prompt source for `FORGE-CC-003B-QE-v1`. Their SHA-256 values are calculated over the exact committed UTF-8 files. Any content change creates a new quality-run version.

| Unit | File | SHA-256 |
|---|---|---|
| Q-001 | `q-001-azure-csharp-quality-planning-v1.txt` | `7640A997DFB79A093C8FFC43B23C54B8E8BFE085EEC5CDAC6E04A48BDA22B4DA` |
| Q-001C | `q-001c-azure-csharp-quality-planning-corrective-v1.txt` | `3432E1F58556E30C1CE7078D11FA13EF8133F5EE830F11514871D7106EE6902D` |
| Q-002 | `q-002-azure-csharp-domain-implementation-human-baseline-v1.txt` | `3880D595047FB679220746A18C86D8BFFC4991F6EC3E3E6A128B8153A15954EF` |
| Q-003 | `q-003-azure-csharp-contract-test-generation-v1.txt` | `9E46804BD252FE0D39CD016BA4D80B70F5A1C09C26B88CF58EEF2B6C23426309` |
| Q-004 | `q-004-azure-csharp-contract-review-v1.txt` | `93FC353FBF99B613C33655E24947BA5DB0873EA292551342FDCBBB4C2F68B85A` |

Q-004 is a template. Its placeholders are replaced only with the hash-verified read-only fixture sources listed in `../frozen-inputs.md`; the composed request is retained separately as evidence.
