# Q-002a C# Generation Review

> **Model:** Qwen3 8B Q4_K_M / Ollama / Desktop RTX 3070  
> **Outcome:** Rejected before compilation

The generated candidate was not retained as an implementation artifact because it was visibly incomplete and cannot compile. It ended with `return new Intake`, used a forbidden `JsonConvert` dependency, referenced members absent from its own result type, and omitted required concurrency/lease semantics.

Timing evidence: 5.794 s cold load; 1,893.53 prompt tokens/s for 327 tokens; 68.12 generation tokens/s for 1,300 tokens; 25.060 s total.

The correction must return raw C# only and must be deliberately smaller: immutable domain types plus interfaces and a deterministic, BCL-only API acceptance service. Worker claim processing will be a separate unit after the acceptance service compiles and passes tests.

## Q-002b Follow-up

The reduced pure-domain candidate was retained as `azure-intake-q-002b.cs` and compiled with .NET SDK 10.0.400. Build failed with four errors: invalid `Dictionary<string, object>` construction from an `object` parameter, and missing `StringBuilder`, `SHA256`, and `Encoding` namespaces. Independent of build errors, its claim and transition functions return decisions without returning updated records, so they cannot implement the contract.
