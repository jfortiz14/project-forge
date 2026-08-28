# CC-003B Findings and Decision

> **Status:** Closed — capacity/performance recommendation recorded

## Current State

CC-003B-001 established that reducing context from 262144 to 32768 materially reduces the auto-fit device-memory requirement (7,367 MiB to 5,365 MiB after CPU expert overrides), but does not yet produce a successful load. CC-003B-002 then retained 32768 context and reduced fit margin to 1024, producing the first successful model load and server readiness. Loaded-idle telemetry shows 294 MiB free VRAM, so the configuration is feasible but has narrow GPU headroom. Its warm Prompt v1 measurement completed without truncation at 74.37 prefill tok/s and 20.83 generation tok/s; generation includes reasoning tokens.

Post-prompt telemetry remained stable at 341 MiB free VRAM. CC-003B-003 will test the next context-ladder step, 65536, while retaining the accepted fit margin before considering cache, batching, or placement changes.

CC-003B-003 reached server readiness at 65536 context. `--fit` increased expert CPU overrides from 39 to 40, reducing CUDA tensor allocation while increasing CUDA host allocation and the KV cache. Loaded-idle and Prompt v1 performance evidence remain pending.

CC-003B-003 completed warm Prompt v1 without truncation at 81.74 prefill tok/s and 19.05 generation tok/s. Relative to CC-003B-002, prefill increased while generation decreased; completion token counts differ slightly and include reasoning, so this is a directional trade-off observation.

Post-prompt telemetry for CC-003B-003 remained stable at 428 MiB free VRAM. The next context-ladder step is 98304 while retaining the same fit margin; all 40 expert layers are already overridden to host memory, so this step tests remaining KV-cache headroom directly.

CC-003B-004 reached server readiness at 98304 context without further changing automatic placement: the runtime retained 40 expert overrides to host memory and increased only the CUDA KV buffer (582.82 MiB to 842.82 MiB). Its fit-estimate headroom fell to 139 MiB, making this a successful but materially narrower configuration than 65536 until loaded-idle and request telemetry are captured.

Loaded-idle telemetry confirms the narrow margin: 263 MiB VRAM free at 98304 context, compared with 356 MiB at 65536. The next gate is the unchanged warm Prompt v1 request; it must complete without OOM or truncation before this candidate is considered usable.

CC-003B-004 completed its bounded evidence set. The server returned HTTP 200, retained 4,745 tokens in its 98,304-token context, and reported `truncated=false`. It measured 80.80 prefill tok/s and 18.81 generation tok/s; the client observed 4,584 completion tokens and 249.936 seconds elapsed. After the request, the server remained idle with 23,003.32 MiB free RAM and 372 MiB free VRAM.

At this stage, 65536 and 98304 are both demonstrated usable under the bounded warm Prompt v1 request. The 98304 candidate provides 50% more configured context and completed the fastest observed prefill of the three successful context measurements, but its 139 MiB auto-fit estimate makes it materially less resilient than 65536 (425 MiB). A true large-context fill is still required before treating the configured 98304 tokens as validated usable capacity.

CC-003B-005 provides that large-context evidence: with no server-flag change from the 98,304-token candidate, RC-002 admitted and retained 85,017 prompt tokens with HTTP 200 and `truncated=false`. It completed prefill in 71.416 seconds (1,190.44 tok/s) and generated the requested single token. Post-request telemetry showed 21,294.44 MiB free RAM and 253 MiB free VRAM; the server remained idle with no OOM or runtime failure. This validates at least 85K usable context capacity. The throughput applies only to the frozen synthetic repeated-token workload and must not be compared directly with RC-001 interactive prompt throughput.

CC-003B-006 establishes the baseline 131,072-token boundary under the original cache types. The runtime failed before tensor load after applying all 40 available expert overrides: 6,223 MiB was required against 6,076 MiB available, a 147 MiB auto-fit shortfall. Therefore, 98,304 is the highest successful configured context before cache-type tuning. The next isolated change is to lower only the K-cache type from `q8_0` to `q4_0` while retaining 131,072 context; any resulting quality trade-off is outside this challenge's current measurement scope.

CC-003B-007 crossed that boundary by changing only the K-cache type to `q4_0`. It loaded a 131,072-token slot with 205 MiB of fit-estimate headroom, 40 expert overrides, and a 720 MiB KV self cache (360 MiB K plus 360 MiB V). This is a configuration feasibility result only until its idle resource state and frozen request contracts are measured; lower K-cache precision is an explicit, unevaluated quality trade-off.

RC-001 completed under CC-003B-007 without truncation, measuring 83.26 prefill tok/s and 18.55 generation tok/s. The client observed 4,595 completion tokens and 249.715 seconds elapsed. Post-request telemetry was 22,990.73 MiB free RAM and 240 MiB free VRAM, with the server still loaded and idle. This completes the bounded interactive evidence for the 131K/q4 K configuration; a high-context utilization test remains required.

CC-003B-008 provides that high-context evidence: RC-003 admitted and retained 110,017 prompt tokens in the 131,072-token / q4 K-cache configuration, returned HTTP 200, and reported `truncated=false`. The server completed the synthetic prefill in 79.977 seconds (1,375.61 tok/s) and returned to idle. Client elapsed time and post-request resource telemetry remain required to close the candidate. This throughput is contract-specific and is not compared to RC-001.

CC-003B-008 closed with 21,312.43 MiB free RAM and 241 MiB free VRAM after the 110K request, with no OOM or runtime failure. Therefore, CC-003B-007 is the provisional leading capacity configuration: it has demonstrated 110,017 retained tokens in 131,072 configured context. It is not a final recommendation because VRAM headroom remains narrow and the lower K-cache precision has not been evaluated for quality.

CC-003B-009 demonstrates that reducing batch and micro-batch to 2048 does not automatically improve practical margin. Although CUDA compute buffers halve, the auto-fit strategy changes placement from 40 to 37 host expert overrides and consumes the recovered capacity with more GPU-resident tensors. Startup succeeds with only 31 MiB fit-estimate headroom, versus 205 MiB for CC-003B-007. Idle and RC-001 evidence are required before quantifying performance, but this profile should be treated as higher risk.

CC-003B-009 completed RC-001 without truncation at 130.60 prefill tok/s and 19.98 generation tok/s, versus 83.26 and 18.55 for CC-003B-007. Completion-token counts differ under reasoning-enabled generation, so this is a directional performance observation rather than a controlled deterministic throughput claim. Post-request telemetry remains required; the 31 MiB startup fit estimate remains the primary stability risk.

CC-003B-009 closed RC-001 with 25,748.02 MiB free RAM and 469 MiB free VRAM after the request, without OOM. Its bounded interactive behavior is therefore successful, but RC-003 must still exercise 110K admitted context before it can be compared as a practical alternative to CC-003B-007.

CC-003B-010 passed that utilization gate: RC-003 admitted and retained 110,017 prompt tokens under batch 2048, returned HTTP 200, and reported `truncated=false`. It completed synthetic prefill in 75.626 seconds (1,454.75 tok/s), directionally higher than CC-003B-008's 1,375.61 tok/s under the same request contract. Post-request resource telemetry remains required; the 31 MiB startup fit estimate continues to be the key resilience concern.

CC-003B-010 closed with 23,323.69 MiB free RAM and 214 MiB free VRAM after RC-003. Batch 2048 therefore provides directionally higher measured prefill under both request contracts, but it has a lower post-110K VRAM snapshot than batch 4096 (214 MiB versus 241 MiB) and a much narrower startup fit estimate (31 MiB versus 205 MiB). CC-003B-007 remains the resilience-oriented provisional leader; CC-003B-009 is a performance-oriented alternative pending the next context-scaling probe.

CC-003B-011 surpassed the prior context target: 196,608 context loaded successfully under batch 2048 and K/V cache `q4_0`/`q4_0`. The runtime added one host expert override relative to CC-003B-009, offsetting the 360 MiB larger KV buffer and producing a 265 MiB fit estimate. This is a feasibility result pending idle telemetry, RC-001, and a substantial large-context utilization request.

CC-003B-011 completed RC-001 without truncation at 78.54 prefill tok/s and 19.11 generation tok/s; the client observed 4,660 completion tokens and 245.961 seconds elapsed. Post-request telemetry was 24,843.07 MiB free RAM and 785 MiB free VRAM, with the server still loaded and idle. A substantial context-utilization request remains required to close the candidate.

CC-003B-012 provides that utilization evidence: RC-004 admitted and retained 165,017 prompt tokens in the 196,608-token profile, returned HTTP 200, and reported `truncated=false`. The server completed the synthetic prefill in 131.392 seconds (1,255.92 tok/s); the client elapsed time was 131.877 seconds. Post-request resource telemetry remains required to close the candidate. This throughput is contract-specific and is not compared to RC-001.

CC-003B-012 closed with 23,033.13 MiB free RAM and 265 MiB free VRAM after RC-004, with no OOM or runtime failure. CC-003B-011 is now the provisional leading capacity configuration: it has demonstrated 165,017 retained tokens in 196,608 configured context. A clean-restart RC-001 confirmation remains before the configuration is recommended within this challenge's performance-and-capacity scope; K-cache quality remains outside that scope.

CC-003B-013 reproduced the 196K configuration after a clean restart: the 38 expert overrides, buffer placement, and 265 MiB fit estimate all matched CC-003B-011. Loaded-idle telemetry and one final RC-001 request remain before the performance-and-capacity recommendation can be confirmed.

CC-003B-013 completed that final confirmation. After clean restart, RC-001 returned HTTP 200 with `truncated=false`, at 83.46 prefill tok/s and 19.37 generation tok/s; the client elapsed time was 241.282 seconds. The server remained idle afterward with 25,328.49 MiB free RAM and 811 MiB free VRAM. This closes the capacity/performance recommendation for the selected profile.

## Decision

For Machine A v2 and this fixed model/runtime, select 196,608 context with batch/micro-batch 2048, K/V cache `q4_0`/`q4_0`, and `--fit-margin 1024` for the measured capacity/performance scope. It clean-restarted successfully, retained 165,017 tokens in a controlled large-context request without truncation, and completed repeated bounded interactive requests without OOM.

Do not interpret this decision as a quality endorsement of lower-precision KV cache. A separately authorized quality challenge is required to determine any response-quality trade-off.

## Decision Rule

Do not recommend a configuration until the results matrix contains complete evidence for at least one candidate that reaches model load, context allocation, stable placement, and bounded performance measurement.
