# CC-003B Quality Evaluation — Operator Runbook

The operator runs every command. Use placeholders in committed records; never record a local drive path.

## 1. Preflight

Record these before the first quality unit:

```text
dotnet --version
<ik_llama_server> --version
git rev-parse HEAD
```

Verify the canonical fixture before Q-003:

```text
Test-ForgeQualityFixture.ps1 -FixtureRoot <canonical-fixture-root>
```

## 2. Quality Server Profile

Start a fresh server with the selected CC-003B placement parameters and explicit no-thinking controls:

```text
--host 0.0.0.0 --port 8080 --device CUDA0
--model <model-path>
--mmproj <projector-path> --no-mmproj-offload
--metrics -c 196608 -b 2048 -ub 2048 --parallel 1
--reasoning-budget 0 --reasoning-tokens none
--jinja -ctv q4_0 -ctk q4_0 -muge -mqkv -p 1
--fit --fit-margin 1024
```

Record the startup placement, context, K/V cache types, and loaded-idle RAM/VRAM telemetry. Start a fresh server before each quality unit unless the register explicitly records an exception.

## 3. Raw OpenAI-Compatible Capture

For each unit, create a request with one user message and `stream=false`. Save only `choices[0].message.content` to the declared raw-evidence path; do not save JSON wrappers, timing lines, or reasoning fields as the evaluated content.

The raw file must be written once, hashed immediately, and left unchanged. Capture response JSON and server timing separately as diagnostics.

```powershell
$ErrorActionPreference = 'Stop'
$prompt = Get-Content -Raw -LiteralPath '<frozen-prompt-path>'
$body = @{
  model = 'Ornith-1.5-35B-Q6_K.gguf'
  messages = @(@{ role = 'user'; content = $prompt })
  stream = $false
} | ConvertTo-Json -Depth 5

$response = Invoke-RestMethod `
  -Uri 'http://127.0.0.1:8080/v1/chat/completions' `
  -Method Post `
  -ContentType 'application/json' `
  -Body $body

$rawPath = '<declared-raw-evidence-path>'
[System.IO.File]::WriteAllText(
  $rawPath,
  [string] $response.choices[0].message.content,
  [System.Text.UTF8Encoding]::new($false)
)
Get-FileHash -LiteralPath $rawPath -Algorithm SHA256
```

If `message.content` is null, empty, or missing, preserve the response JSON separately as transport evidence and stop the unit. Do not substitute `reasoning_content` or manually extract text.

When using `Invoke-WebRequest` instead of `Invoke-RestMethod` on Windows PowerShell, include `-UseBasicParsing`. If the request is cancelled before a response exists, stop immediately and remove any zero-byte response or raw-output files; the event is a client-side aborted attempt, not a quality-unit execution.

### Windows PowerShell Long-Path Note

Some evidence and prompt paths can exceed the legacy `MAX_PATH` limit in Windows PowerShell even when the parent directory can be enumerated. In that case, keep the repository path as the logical path but pass the equivalent Windows extended-length path (`\\?\` prefix) to .NET file read/write APIs and hashing code. This is a transport workaround only; it does not change file content, filenames, or the frozen prompt.

The `dotnet` CLI can reject a project path before .NET file APIs receive it. When that occurs, map a temporary short drive to the POC root, then invoke the project through the mapped path. Map the POC root rather than `quality-evaluation` so existing relative references to `05-quality-evaluation` remain valid. Remove the mapping after the run; do not record the local drive letter in committed evidence.

## 4. Gate Order

- Q-001: preserve raw planning output, hash, then review against the planning contract.
- Q-002: preserve raw `.cs`, hash, compile the literal file, then run tests only if compilation succeeds.
- Q-003: preserve raw `Program.cs`, hash, compile/run against the correct reference, then run applicable mutants only if it passes the reference.
- Q-004: preserve raw review output, then score format, read-only adherence, traceability, true findings, omissions, severity, and false material findings.

For all code units, do not remove fences, repair source, or inspect semantics before the literal build gate. Any authorized derivation or human repair is a separate recorded unit.
