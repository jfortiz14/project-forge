[CmdletBinding()]
param(
    [Parameter(Mandatory)] [string] $Model,
    [Parameter(Mandatory)] [string] $PromptPath,
    [Parameter(Mandatory)] [string] $RawOutputPath
)

$prompt = Get-Content -Raw -LiteralPath $PromptPath
$outputDirectory = Split-Path -Parent $RawOutputPath
New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null
& ollama run $Model --think=false --keepalive=0 --nowordwrap $prompt > $RawOutputPath
Get-FileHash -LiteralPath $RawOutputPath -Algorithm SHA256 | Select-Object Algorithm, Hash, Path
