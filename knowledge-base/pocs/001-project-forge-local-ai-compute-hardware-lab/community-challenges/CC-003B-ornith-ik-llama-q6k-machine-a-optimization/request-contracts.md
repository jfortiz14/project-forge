# CC-003B Request Contracts

This document freezes the request identities used by CC-003B. A result is comparable only when it names one of these contracts or explicitly documents a replacement.

## RC-001 — Benchmark Prompt v1

**Purpose:** Bounded interactive prefill and generation measurement.

**Canonical source:** [`03-benchmark-method/benchmark-contract-v1.md`](../../03-benchmark-method/benchmark-contract-v1.md) in the parent POC.

**Request form:** Local OpenAI-compatible `POST /v1/chat/completions`, one user message containing the canonical prompt, `stream=false`, with no request-level sampling or reasoning override. The server's configured reasoning behavior remains part of the measurement and is always reported.

The canonical source controls the prompt text and constraints. CC-003B records the actual prompt and completion token counts returned by the server for each use.

## RC-002 — Synthetic 85K Context-Utilization Prompt

**Purpose:** Exercise high-context prompt admission and prefill with negligible requested output. This is not a quality, answer-following, or generation-throughput benchmark.

**Frozen construction:**

```powershell
$largePrompt = (' a' * 85000) + "`nReply only with: OK."
```

**Request form:** Local OpenAI-compatible `POST /v1/chat/completions`, one user message containing `$largePrompt`, `max_tokens=1`, and `stream=false`. No server launch flag changes, request-level reasoning override, sampling override, or retry is permitted for the candidate.

**Required evidence:** Returned prompt/completion/total tokens, client elapsed time, server prefill/generation/total times, retained token count, truncation state, HTTP outcome, and pre-request/post-request RAM and VRAM telemetry.

The construction is frozen instead of storing the fully expanded repeated string. PowerShell string multiplication and the specified suffix deterministically reproduce the exact request text.

## RC-003 — Synthetic 110K Context-Utilization Prompt

**Purpose:** Validate substantial usable context under the 131,072-token / `q4_0` K-cache configuration. This is not a quality, answer-following, or generation-throughput benchmark.

**Frozen construction:**

```powershell
$largePrompt = (' a' * 110000) + "`nReply only with: OK."
```

**Request form:** Local OpenAI-compatible `POST /v1/chat/completions`, one user message containing `$largePrompt`, `max_tokens=1`, and `stream=false`. No server launch flag changes, request-level reasoning override, sampling override, or retry is permitted for the candidate.

**Required evidence:** Same as RC-002, with the returned prompt-token count treated as the authoritative admitted-context measurement.

The construction is frozen instead of storing the fully expanded repeated string. It differs from RC-002 only in its 110,000-repeat count and is not comparable to RC-001 interactive-prompt throughput.

## RC-004 — Synthetic 165K Context-Utilization Prompt

**Purpose:** Validate substantial usable context under the 196,608-token / `q4_0` K-cache / batch-2048 configuration. This is not a quality, answer-following, or generation-throughput benchmark.

**Frozen construction:**

```powershell
$largePrompt = (' a' * 165000) + "`nReply only with: OK."
```

**Request form:** Local OpenAI-compatible `POST /v1/chat/completions`, one user message containing `$largePrompt`, `max_tokens=1`, and `stream=false`. No server launch flag changes, request-level reasoning override, sampling override, or retry is permitted for the candidate.

**Required evidence:** Same as RC-002, with returned prompt-token count treated as the authoritative admitted-context measurement.

The construction is frozen instead of storing the expanded repeated string. It is not comparable to RC-001 interactive-prompt throughput.
