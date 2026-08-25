[CmdletBinding()]
param([Parameter(Mandatory)] [string] $FixtureRoot)

Push-Location $FixtureRoot
try {
    dotnet run --project 'baseline-tests\Forge.DocumentIntake.BaselineTests.csproj'
    if ($LASTEXITCODE -ne 0) { throw 'Baseline tests failed.' }
    foreach ($mutant in (Get-ChildItem 'mutants' -Filter 'MUT-*.cs' | Sort-Object Name)) {
        dotnet build 'mutants\MutantCompile.csproj' --nologo "-p:MutantSource=$($mutant.Name)"
        if ($LASTEXITCODE -ne 0) { throw "Mutant failed to compile: $($mutant.Name)" }
    }
}
finally { Pop-Location }
