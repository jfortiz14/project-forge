# CC-002A Results: `qwen3.5:35B-A3B` Independent Baseline

> **Status:** Baseline run captured
> **Scope:** Machine A - personal desktop
> **Challenge:** `CC-002-qwen`

## Evidence Summary

`qwen3.5:35B-A3B` was successfully exercised on Machine A under the frozen `no-thinking` profile with the FORGE-style prompt family. The source model was imported into Ollama, then wrapped in the FORGE alias `forge-qwen35-35B-A3B-ctx4096-nothink` for the benchmark run.

### Prompt Evidence

The benchmark prompt asked for:

- the difference between prompt processing and token generation
- exactly three factors affecting prompt processing speed
- exactly three factors affecting token-generation performance
- a reproducible benchmarking procedure for a Windows desktop with a local GPU
- a local-LLM software-development workflow covering planning, implementation assistance, testing, and code review

The observed response covered all five requested areas. It used plain prose with short paragraphs and no Markdown headings or bullets, so it was more format-consistent than the prior Ornith baseline rerun.

The response stated that prompt processing, or prefill, analyzes the input sequence as a whole, while token generation is the autoregressive step that emits tokens one at a time. It identified three prompt-processing factors as available video memory, sequence length, and precision format. It identified three generation factors as GPU compute throughput, batch size capability, and cooling/power delivery. It also described a repeatable Windows benchmark procedure using a fixed prompt, time to first token, tokens-per-second measurement, and repeated runs. For software development, it outlined a workflow covering planning, implementation assistance, tests, and code review.

The run was executed with:

```powershell
ollama run forge-qwen35-35B-A3B-ctx4096-nothink --think=false --verbose --keepalive=0
```

### Runtime Evidence

The following runtime measurements were observed:

- Total duration: `1m36.2592308s`
- Load duration: `1m10.8099811s`
- Prompt eval count: `131` tokens
- Prompt eval duration: `1.735207s`
- Prompt eval rate: `75.50 tokens/s`
- Eval count: `654` tokens
- Eval duration: `23.699613s`
- Eval rate: `27.60 tokens/s`

### Post-Run Snapshot

`ollama ps` showed:

- Name: `forge-qwen3-35B-A3B-ctx4096-nothink:latest`
- Size: `23 GB`
- Processor: `76%/24% CPU/GPU`
- Context: `4096`

`nvidia-smi` showed:

- GPU: `NVIDIA GeForce RTX 3070`
- Temperature: `53C`
- Power: `48W / 220W`
- Memory usage: `7785MiB / 8192MiB`
- GPU utilization: `1%`

## Interpretation

This run shows that `qwen3.5:35B-A3B` is loadable and runnable on the Machine A RTX 3070 baseline under the FORGE-style `no-thinking` control. The response quality is acceptable for a first pass because it stayed on-topic, covered all requested areas, and avoided format drift that would make comparison harder.

The baseline also shows the expected tradeoff for a 35B-class model on an 8 GB GPU: the model loads, but the runtime leans heavily on CPU as shown by the `76%/24% CPU/GPU` processor split and the near-full VRAM use at `7785MiB / 8192MiB`.

## Decision

**Baseline conclusion:** `qwen3.5:35B-A3B` is viable for further FORGE-style evaluation on Machine A and produces a cleaner first-pass response than the prior Ornith baseline rerun, but it still needs follow-up interpretation before any quality claim.

**Operational boundary:** Keep performance and quality separated, and do not treat this baseline as acceptance evidence for software-architecture or code-generation quality.

See [`execution-manifest.md`](execution-manifest.md) for the frozen run protocol.
