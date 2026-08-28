# CC-003B Quality Harness Preflight

**Purpose:** Verify that the CC-003B quality workspace can compile and copy a known-valid human-authored .NET assembly before interpreting Q-003 model output.

This is an infrastructure check, not a model-quality unit. It uses no generated artifact, does not modify Q-002/Q-002F, and has no quality verdict.

Expected result: `dotnet build HarnessPreflight.csproj --nologo` succeeds and produces the expected output assembly. Record the outcome in `../execution-events.md` before Q-003 capture.

If the physical workspace path exceeds the Windows `MAX_PATH` limit, invoke the project through a temporary mapped drive whose root is the POC root (not the `quality-evaluation` directory). That preserves the project’s relative references to canonical fixtures while shortening the command-line and output paths. The mapping is an operator-local transport measure and is removed after the quality run.
