# Q-001 Contract Review

**Unit:** Q-001 Azure/C# planning  
**Raw artifact:** `q-001-ornith-raw.txt`  
**Raw SHA-256:** `F97D61C262B87190E5006D0099B32C42A7AD5BE55E64283B03D46E86DDBAF3F6`  
**Transport outcome:** HTTP 200; no context truncation reported  
**Verdict:** **Fail — output-format gate**

## Contract Checks

| Check | Result | Evidence |
|---|---|---|
| Six required labeled sections | Pass | All six required headings are present and in order. |
| Clear technical English | Pass | Response is readable and technically coherent. |
| No implementation code, tables, citations, or external tools | Pass | No prohibited output form was observed. |
| 450–600 words | **Fail** | 624 whitespace-delimited words; exceeds the 600-word maximum by 24 words. |

The response is preserved unchanged. The failure is a material explicit-constraint violation, independent of the otherwise useful design content. No semantic repair, trimming, or regeneration was applied.

## Diagnostic Timing

| Metric | Observation |
|---|---:|
| Client elapsed | 159.378 s |
| Prompt evaluation | 300 tokens at 168.56 tokens/s |
| Generation | 3,099 tokens at 19.74 tokens/s |
| Total server time | 158.787 s for 3,399 tokens |

Timing is diagnostic only and does not affect the verdict.
