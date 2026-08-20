[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$OutFile,

    [ValidateRange(1, 60)]
    [int]$IntervalSeconds = 1
)

$outDirectory = Split-Path -Parent $OutFile
if ($outDirectory) {
    New-Item -ItemType Directory -Path $outDirectory -Force | Out-Null
}

$os = Get-CimInstance Win32_OperatingSystem
$totalRamMiB = [math]::Round(($os.TotalVisibleMemorySize * 1KB) / 1MB, 0)

Write-Host "Collecting Project FORGE telemetry every $IntervalSeconds second(s). Press Ctrl+C to stop."
Write-Host "Output: $OutFile"

try {
    while ($true) {
        $sampleTime = Get-Date -Format 'o'
        $counters = Get-Counter '\Processor(_Total)\% Processor Time', '\Memory\Available MBytes'
        $cpuSample = $counters.CounterSamples | Where-Object { $_.Path -like '*Processor(_Total)*' } | Select-Object -First 1
        $memorySample = $counters.CounterSamples | Where-Object { $_.Path -like '*Memory*Available MBytes' } | Select-Object -First 1
        $cpuPercent = if ($cpuSample) { [math]::Round($cpuSample.CookedValue, 2) } else { $null }
        $availableRamMiB = if ($memorySample) { [math]::Round($memorySample.CookedValue, 0) } else { $null }

        $llmProcesses = Get-Process -Name 'ollama', 'llama-server' -ErrorAction SilentlyContinue
        $llmWorkingSetMiB = [math]::Round((($llmProcesses | Measure-Object -Property WorkingSet64 -Sum).Sum / 1MB), 0)

        $nvidia = $null
        if (Get-Command nvidia-smi -ErrorAction SilentlyContinue) {
            $nvidia = nvidia-smi --query-gpu=utilization.gpu,memory.used,memory.total,power.draw,temperature.gpu --format=csv,noheader,nounits 2>$null |
                Select-Object -First 1
        }

        $gpuUtil = $gpuUsedMiB = $gpuTotalMiB = $gpuPowerW = $gpuTempC = $null
        if ($nvidia) {
            $parts = $nvidia -split ',' | ForEach-Object { $_.Trim() }
            if ($parts.Count -ge 5) {
                $gpuUtil = $parts[0]
                $gpuUsedMiB = $parts[1]
                $gpuTotalMiB = $parts[2]
                $gpuPowerW = $parts[3]
                $gpuTempC = $parts[4]
            }
        }

        [pscustomobject]@{
            Timestamp              = $sampleTime
            SystemCpuPercent       = $cpuPercent
            TotalRamMiB            = $totalRamMiB
            AvailableRamMiB        = $availableRamMiB
            UsedRamMiB             = if ($null -ne $availableRamMiB) { $totalRamMiB - $availableRamMiB } else { $null }
            OllamaWorkingSetMiB    = $llmWorkingSetMiB
            NvidiaGpuUtilPercent   = $gpuUtil
            NvidiaMemoryUsedMiB    = $gpuUsedMiB
            NvidiaMemoryTotalMiB   = $gpuTotalMiB
            NvidiaPowerWatts       = $gpuPowerW
            NvidiaTemperatureC     = $gpuTempC
        } | Export-Csv -Path $OutFile -NoTypeInformation -Append

        Start-Sleep -Seconds $IntervalSeconds
    }
}
finally {
    Write-Host "Telemetry collection stopped."
}
