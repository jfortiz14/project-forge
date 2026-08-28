# Q-002 Literal Compile Gate

**Unit:** Q-002 independent C# domain implementation  
**Raw artifact:** `q-002-ornith-raw.cs`  
**Raw SHA-256:** `DA31139A7443ABCD5B275903C7F991F25AEF2917F0C0F1DF9640DF40975369E9`  
**Build command:** `dotnet build OrnithQ002.csproj --nologo`  
**Verdict:** **Fail — literal compile gate**

## Observed Build Result

The .NET 10.0 project restore completed, followed by 7 compiler errors. The raw source contains three backtick characters at lines 1 and 298, i.e. an opening and closing Markdown fence:

- `CS1056` unexpected character `` ` `` at line 1, columns 1–3, and line 298, columns 1–3.
- `CS0116` at line 1, column 4, caused by the leading language label after the opening fence.

The build failed in 1.6 seconds. The source was not opened for semantic review, edited, de-fenced, copied, or repaired. Contractual tests and human/contract review were not run because the compile gate did not pass.

This is a terminal autonomous implementation result for Q-002. Any future fence-only derivation or human repair would require separate explicit authorization and a separately named measurement; it cannot alter this verdict.
