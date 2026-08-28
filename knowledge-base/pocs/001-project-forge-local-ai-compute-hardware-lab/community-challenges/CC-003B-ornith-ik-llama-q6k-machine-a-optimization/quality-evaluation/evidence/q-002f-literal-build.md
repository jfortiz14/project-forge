# Q-002F Fence-Only Literal Compile Gate

**Unit:** Q-002F authorized fence-only derivation  
**Derived artifact:** `q-002f-ornith-fence-only.cs`  
**Derived SHA-256:** `55301E6A90759C642874F2EB1F1407E0B20FF484324694FC09D3C6AD490EC699`  
**Physical-path command:** `dotnet build OrnithQ002FenceOnly.csproj --nologo`  
**Short-path command:** the same project command, executed through the temporary short mapped drive  
**Status:** **Pass — short-path literal compile gate**

## Observed Build Result

The first physical-path invocation restored, then failed with one `MSB3030` error after 2.0 seconds:

```text
Could not copy the file "obj\Debug\net10.0\OrnithQ002FenceOnly.dll" because it was not found.
```

The fence-only derivation removed the known fence syntax from the raw artifact, but this physical-path build result did not produce a usable assembly. Later preflight proved that the physical workspace path exceeded Windows `MAX_PATH` and that the known-valid harness succeeds through a short mapped drive. The result is therefore **invalid infrastructure evidence, not evidence about the retained C# body or the model.**

The observed `MSB3030` occurs after restore and does not name a C# diagnostic. The later path preflight resolves the relevant attribution boundary: this run was confounded by infrastructure. No inference about the retained C# body is made.

## Post-Closure Read-Only Build-Artifact Observation

After the physical-path failure, a read-only inspection found the intermediate `obj` DLL and PDB present while the final `bin` DLL was absent. This is consistent with the reported copy/output-stage failure and makes a compiler-source diagnostic less likely; it does not prove that every semantic requirement of the retained source is satisfied.

## Valid Short-Path Build Result

The unchanged Q-002F project was then built through the temporary short mapped drive. Restore completed in 0.3 seconds; `OrnithQ002FenceOnly` succeeded in 1.3 seconds and emitted its `net10.0` DLL. The build completed successfully in **2.0 seconds** with no reported warnings or errors.

This establishes only that the exact fence-only derivative is compilable. It does not alter the closed autonomous Q-002 raw-source failure, and it does not establish contract correctness. The frozen contractual-test suite is now eligible to run before any semantic assessment.
