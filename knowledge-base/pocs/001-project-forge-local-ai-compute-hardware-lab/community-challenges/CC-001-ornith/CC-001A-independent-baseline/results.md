# CC-001A Results: Ornith 1.5 35B-A3B Independent Baseline

> **Status:** Baseline runs captured
> **Scope:** Machine A — personal desktop
> **Challenge:** `CC-001-ornith`

## Evidence Summary

Ornith 1.5 35B-A3B was successfully exercised on Machine A under the frozen `no-thinking` profile with the FORGE-style prompt family. The source model `hf.co/ornith-ai/Ornith-1.5-35B-A3B-GGUF:Q4_K_M` was imported into Ollama, then wrapped in the FORGE alias `forge-ornith-35B-A3B-ctx4096-nothink` with `num_ctx 4096`.

### Prompt Evidence

The benchmark prompt used for the strict-format rerun followed the original FORGE `Benchmark Contract v1` text and was submitted through PowerShell to `ollama run` with the frozen baseline alias. The prompt asked for:

- the difference between prompt processing and token generation
- exactly three factors affecting prefill performance
- exactly three factors affecting token-generation performance
- a reproducible benchmarking procedure for a Windows desktop with a local GPU
- a local-LLM software-development workflow covering planning, implementation assistance, testing, and code review

The run was executed with:

```powershell
ollama run forge-ornith-35B-A3B-ctx4096-nothink --think=false --verbose --keepalive=5m
```

The response was observed in the same session, followed by `ollama ps` and `nvidia-smi` while the model remained loaded.

The first baseline run, `CC-001A-001`, completed with a total duration of `1m30.751606s`, load duration of `1m6.1930145s`, prompt-eval rate of `92.49 tokens/s`, and generation rate of `28.42 tokens/s`. The model processed `138` prompt tokens and produced `655` output tokens. The response covered the intended conceptual areas, but it used Markdown headings and bold formatting, so it was only partially comparable to the original FORGE benchmark contract.

The second baseline run, `CC-001A-002`, repeated the same alias and profile with `--keepalive=5m` so post-run state could be observed. It completed with a total duration of `1m31.0929904s`, load duration of `1m7.0623469s`, prompt-eval rate of `89.04 tokens/s`, and generation rate of `30.13 tokens/s`. It processed `138` prompt tokens and produced `677` output tokens. While the model remained within the same qualitative topic boundaries, the response again used Markdown headings and bold formatting and therefore remained only partially comparable to the original FORGE benchmark contract.

During the second run, `ollama ps` reported the loaded alias at `4096` context and `nvidia-smi` captured `7,795 MiB / 8,192 MiB` VRAM usage, `3%` GPU utilization, `51C`, and `21W` board power. Ollama also reported the loaded model at `21 GB`, giving a complete post-run snapshot for the challenge matrix.

## Interpretation

These runs show that Ornith 1.5 35B-A3B is loadable and runnable on the Machine A RTX 3070 baseline under the FORGE-style `no-thinking` control, but the output format drift means the text is not an exact match to the original benchmark contract. The measured timing data is valid as observed performance evidence, but the qualitative comparison should be treated as partial until a strict-format rerun is captured.

The challenge therefore establishes a usable independent baseline, not a final quality judgment. The next useful step is a strict-format repeat that preserves the original FORGE prompt shape more exactly, followed by the separate quality phase that FORGE used for software-architecture and coding assessment.

## Decision

**Baseline conclusion:** Ornith 1.5 35B-A3B is a viable candidate for further FORGE-style evaluation on Machine A, but the current runs only partially satisfy exact prompt-format comparability.

**Operational boundary:** Keep performance and quality separated, and do not treat these runs as acceptance evidence for software-architecture or code-generation quality.

See [`results-matrix.md`](results-matrix.md) for the complete measurement rows and [`execution-manifest.md`](execution-manifest.md) for the frozen run protocol.
