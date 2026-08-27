# Results Matrix: Experiment 001

> **Status:** Completed -- three-run performance baseline captured
> **Comparison:** CC-002A 32 GiB baseline vs. Experiment 001 64 GiB baseline

`N/R` means not recorded, not zero. Populate the 64 GiB rows only with
observed evidence. The 32 GiB rows are an immutable transcription of the
accepted CC-002A matrix.

| Baseline | Run | RAM | Load Time | Prompt Tokens/s | Generation Tokens/s | Prompt / Generated Tokens | VRAM Used / Total | CPU / GPU Placement | Context | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Historical CC-002A | CC-002A-001 | 32 GiB | 1m10.8099811s | 75.50 | 27.60 | 131 / 654 | 7,785 / 8,192 MiB | 76% / 24% | 4096 | No-thinking; total 1m36.2592308s |
| Historical CC-002A | CC-002A-002 | 32 GiB | 1m11.9134137s | 63.09 | 30.23 | 131 / 629 | 7,724 / 8,192 MiB | 76% / 24% | 4096 | No-thinking; total 1m34.8129141s |
| Historical CC-002A | CC-002A-003 | 32 GiB | 1m11.5753753s | 104.48 | 29.62 | 170 / 218 | 7,694 / 8,192 MiB | 76% / 24% | 4096 | No-thinking; total 1m20.5724832s |
| 64 GiB experiment | FORGE-EXP-001-001 | 64 GiB | 14.2500198s | 191.02 | 27.74 | 131 / 553 | 7,694 / 8,192 MiB | 76% / 24% | 4096 | Cold state confirmed; exact 131-token CC-002A historical prompt; no-thinking; total 34.873387s. Post-run `ollama ps` was empty because `--keepalive=0`; `nvidia-smi` therefore measured only unloaded GPU state. |
| 64 GiB experiment | FORGE-EXP-001-002 | 64 GiB | 14.491993s | 191.47 | 28.83 | 131 / 593 | 7,694 / 8,192 MiB | 76% / 24% | 4096 | Cold state confirmed; exact 131-token CC-002A historical prompt; no-thinking; total 35.7502343s. Post-run `ollama ps` was empty because `--keepalive=0`; `nvidia-smi` therefore measured only unloaded GPU state. |
| 64 GiB experiment | FORGE-EXP-001-003 | 64 GiB | 14.7605321s | 184.36 | 28.94 | 131 / 568 | 7,569 / 8,192 MiB | 76% / 24% | 4096 | Cold state confirmed; exact 131-token CC-002A historical prompt; no-thinking; total 35.1031664s; 56C, 54W / 220W, 42% GPU utilization while loaded. |

## Excluded Observations

These records are retained as observed evidence but are not part of the
three-run comparable baseline.

| Observation | Reason excluded | Load Time | Prompt Tokens/s | Generation Tokens/s | Prompt / Generated Tokens | VRAM Used / Total | CPU / GPU Placement | Context | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| FORGE-EXP-001-OBS-001 | No pre-run unloaded-state capture was supplied, so the 131-token historical prompt run cannot be accepted as cold. | 1m5.4930778s | 137.94 | 26.80 | 131 / 601 | 7,718 / 8,192 MiB | 76% / 24% | 4096 | Total 1m28.8816043s; no visible reasoning; 55C, 21W / 220W, 4% GPU utilization. |
| FORGE-EXP-001-OBS-002 | The submitted prompt-evaluation count was 151 tokens rather than the frozen 131-token historical prompt; no pre-run unloaded-state capture was supplied. | 14.4983075s | 170.87 | 26.73 | 151 / 674 | 7,673 / 8,192 MiB | 76% / 24% | 4096 | Total 40.604639s; no visible reasoning; 56C, 20W / 220W, 5% GPU utilization. |
| FORGE-EXP-001-OBS-003 | A pre-run unloaded state was captured, but `--keepalive=3m` differed from the frozen `--keepalive=0` control. | 14.7356249s | 159.58 | 26.08 | 131 / 550 | 7,652 / 8,192 MiB | 76% / 24% | 4096 | Total 36.6501849s; no visible reasoning; 57C, 24W / 220W, 8% GPU utilization. |
| FORGE-EXP-001-OBS-004 | The exact long-form Benchmark Contract v1 prompt was used rather than the 131-token CC-002A historical prompt selected for direct RAM comparison. | 15.2001158s | 228.24 | 28.68 | 164 / 615 | N/R | N/R | 4096 | Total 37.3668977s; cold state confirmed; no-thinking; post-run unloaded-state telemetry only. |

## Comparison Summary

| Metric | CC-002A (32 GiB) observed range | Experiment 001 (64 GiB) observed range | Interpretation |
| --- | --- | --- | --- |
| Load time | 1m10.8099811s to 1m11.9134137s | 14.2500198s to 14.7605321s | The observed 64 GiB runs loaded much faster. This is descriptive evidence; RAM alone is not proven as the cause. |
| Prompt processing | 63.09 to 104.48 tokens/s | 184.36 to 191.47 tokens/s | Higher in the 64 GiB runs. The two historical 131-token rows were 63.09 to 75.50 tokens/s; the historical third row had 170 prompt tokens, so it is not an exact prefill match. |
| Generation | 27.60 to 30.23 tokens/s | 27.74 to 28.94 tokens/s | Ranges overlap. The RAM upgrade did not demonstrate a material generation-rate increase for this model and profile. |
| VRAM use | 7,694 to 7,785 MiB / 8,192 MiB | 7,569 MiB / 8,192 MiB in run 003; N/R in runs 001-002 | Loaded-state observation remains near VRAM saturation and is directionally consistent with the historical baseline. |
| CPU / GPU placement | 76% / 24% | 76% / 24%  | The observed placement is unchanged. |

## Three-Run Aggregate (64 GiB)

- Mean load time: `14.501s`.
- Mean prompt processing: `188.95 tokens/s`.
- Mean generation: `28.50 tokens/s`.
- Mean total duration: `35.242s`.

## Conclusion

With the frozen 131-token CC-002A historical prompt, `qwen3.5:35B-A3B` is
reproducibly runnable on the 64 GiB Machine A configuration. The experiment
observed substantially faster loading and prompt processing than the recorded
32 GiB CC-002A rows, while generation remained in the same approximate range.
The one loaded-state capture shows the same 76% CPU / 24% GPU placement and
near-full VRAM use, so the evidence does not show that added system RAM changed
GPU residency or made generation materially faster. No procurement, quality,
or autonomous-use decision follows from this performance experiment.

## Evidence Checklist

- [x] Preflight hardware and software inventory fully recorded.
- [x] Exact source model and operational alias verified.
- [x] Three cold runs captured with the 131-token CC-002A historical prompt.
- [x] Verbose Ollama timings captured for every accepted run.
- [x] Loaded-state `ollama ps` and `nvidia-smi` evidence captured for every accepted run.
- [x] Comparability caveats and deviations documented.
