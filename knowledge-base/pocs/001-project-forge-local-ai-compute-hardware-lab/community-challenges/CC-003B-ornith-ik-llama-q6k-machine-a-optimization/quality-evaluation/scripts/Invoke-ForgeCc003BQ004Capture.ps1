Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$endpoint = 'http://127.0.0.1:8080/v1/chat/completions'

function Get-Sha256Hex {
    param([Parameter(Mandatory)] [string] $Path)

    return (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash
}

function Assert-FileHash {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [string] $ExpectedHash,
        [Parameter(Mandatory)] [string] $Label
    )

    $actualHash = Get-Sha256Hex -Path $Path
    if ($actualHash -ne $ExpectedHash) {
        throw "$Label hash mismatch. Expected $ExpectedHash; observed $actualHash."
    }

    return $actualHash
}

$qualityRoot = Split-Path -Path $PSScriptRoot -Parent
$pocRoot = (Resolve-Path -LiteralPath (Join-Path $qualityRoot '..\..\..')).Path

$templatePath = Join-Path $qualityRoot 'prompts\q-004-azure-csharp-contract-review-v1.txt'
$contractPath = Join-Path $pocRoot '05-quality-evaluation\azure-csharp-domain-contract-v1.md'
$implementationPath = Join-Path $pocRoot 'community-challenges\CC-002-qwen\quality-evaluation\fixtures\q-004-review-fixture-v2\implementation\OrnithQ002H.minimal-repair.cs'
$testsPath = Join-Path $pocRoot 'community-challenges\CC-002-qwen\quality-evaluation\fixtures\q-004-review-fixture-v2\tests\Program.cs'
$evidenceRoot = Join-Path $qualityRoot 'evidence'

$templateHash = Assert-FileHash -Path $templatePath -ExpectedHash '93FC353FBF99B613C33655E24947BA5DB0873EA292551342FDCBBB4C2F68B85A' -Label 'Q-004 template'
$contractHash = Assert-FileHash -Path $contractPath -ExpectedHash '81DEC1163E9A16E7F3ACAF28AB2BE35A8B0F9624241B7FCEE82FFB4A0BA9F6DB' -Label 'Domain contract'
$implementationHash = Assert-FileHash -Path $implementationPath -ExpectedHash '2380F6A745567B848F17DE90B6AFB548786811E79D9D6F5E8841BC9FCB70B3F9' -Label 'Implementation fixture'
$testsHash = Assert-FileHash -Path $testsPath -ExpectedHash '81417ECFE0C62A8C8CC971E02F93BE8FD4CD79B0AA8D7A91C06AAAA4435F8384' -Label 'Test fixture'

$template = [System.IO.File]::ReadAllText($templatePath)
$contract = [System.IO.File]::ReadAllText($contractPath)
$implementation = [System.IO.File]::ReadAllText($implementationPath)
$tests = [System.IO.File]::ReadAllText($testsPath)

$prompt = $template.Replace('{CONTRACT}', $contract).Replace('{IMPLEMENTATION}', $implementation).Replace('{TESTS}', $tests)
if ($prompt.Contains('{CONTRACT}') -or $prompt.Contains('{IMPLEMENTATION}') -or $prompt.Contains('{TESTS}')) {
    throw 'Q-004 prompt construction failed: one or more placeholders remain.'
}

$body = @{
    model = 'Ornith-1.5-35B-Q6_K.gguf'
    messages = @(@{ role = 'user'; content = $prompt })
    stream = $false
} | ConvertTo-Json -Depth 5

$utf8NoBom = [System.Text.UTF8Encoding]::new($false)
$requestPath = Join-Path $evidenceRoot 'q-004-request.json'
$responsePath = Join-Path $evidenceRoot 'q-004-response.json'
$rawPath = Join-Path $evidenceRoot 'q-004-ornith-review-raw.txt'
$fixtureVerificationPath = Join-Path $evidenceRoot 'q-004-fixture-verification.txt'

[System.IO.File]::WriteAllText(
    $fixtureVerificationPath,
    "TemplateSHA256: $templateHash`nContractSHA256: $contractHash`nImplementationSHA256: $implementationHash`nTestsSHA256: $testsHash`n",
    $utf8NoBom
)
[System.IO.File]::WriteAllText($requestPath, $body, $utf8NoBom)

$watch = [System.Diagnostics.Stopwatch]::StartNew()
$http = Invoke-WebRequest -UseBasicParsing -Uri $Endpoint -Method Post -ContentType 'application/json' -Body $body
$watch.Stop()

[System.IO.File]::WriteAllText($responsePath, $http.Content, $utf8NoBom)
$response = $http.Content | ConvertFrom-Json
$content = [string] $response.choices[0].message.content
if ([string]::IsNullOrWhiteSpace($content)) {
    throw 'Q-004 stopped: message.content is empty. Transport JSON was preserved; no raw review artifact was written.'
}

[System.IO.File]::WriteAllText($rawPath, $content, $utf8NoBom)
"RawSHA256: $(Get-Sha256Hex -Path $rawPath)"
"ClientElapsedSeconds: {0:N3}" -f $watch.Elapsed.TotalSeconds
