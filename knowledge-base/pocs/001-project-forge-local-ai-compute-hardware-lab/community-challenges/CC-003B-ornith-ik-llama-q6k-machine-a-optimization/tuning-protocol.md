# CC-003B Tuning Protocol

## Candidate Sequence

The sequence intentionally starts from the CC-003A failure boundary and relaxes one resource constraint at a time. Values below are planned candidates, not observed results.

| Candidate | Single decision under test | Planned change from CC-003A | Acceptance evidence |
| --- | --- | --- | --- |
| CC-003B-001 | Context feasibility | Set `-c 32768` while retaining the community fit behavior and all other flags | Successful tensor load; actual context; placement; RAM/VRAM |
| CC-003B-002 | Fit headroom | Retain `-c 32768`; change `--fit-margin` from 3030 to 1024 only | Successful tensor load plus safe observed VRAM headroom |
| CC-003B-003 | Context scaling | Retain `--fit-margin 1024`; increase `-c` from 32768 to 65536 only | Successful tensor load; actual context; placement; RAM/VRAM |
| CC-003B-004 | Context scaling | Retain `--fit-margin 1024`; increase `-c` from 65536 to 98304 only | Successful tensor load; actual context; placement; RAM/VRAM |
| CC-003B-005 | Large-context utilization | Retain CC-003B-004 server configuration; send a controlled approximately 85K-token synthetic prefill with minimal output | Actual admitted prompt tokens; HTTP outcome; retained tokens; truncation state; RAM/VRAM before and after |
| CC-003B-006 | Context-boundary probe | Retain `--fit-margin 1024`; increase `-c` from 98304 to 131072 only | Startup success with allocation, or explicit auto-fit/runtime failure evidence |
| CC-003B-007 | KV-cache trade-off | Retain `-c 131072`; change `-ctk q8_0` to `-ctk q4_0` only | Startup outcome; actual context; RAM/VRAM; measured performance delta if it loads |
| CC-003B-008 | Large-context utilization | Retain CC-003B-007 server configuration; send RC-003 synthetic 110K prefill with minimal output | Actual admitted prompt tokens; HTTP outcome; retained tokens; truncation state; RAM/VRAM before and after |
| CC-003B-009 | Batching trade-off | Retain CC-003B-007; change `-b 4096 -ub 4096` to `-b 2048 -ub 2048` only | Stable startup, actual placement and resource headroom, RC-001 performance evidence |
| CC-003B-010 | Large-context utilization | Retain CC-003B-009 server configuration; send RC-003 synthetic 110K prefill with minimal output | Actual admitted prompt tokens; HTTP outcome; retained tokens; truncation state; RAM/VRAM before and after |
| CC-003B-011 | Context scaling | Retain CC-003B-009; increase `-c` from 131072 to 196608 only | Startup success with allocation, or explicit auto-fit/runtime failure evidence |
| CC-003B-012 | Large-context utilization | Retain CC-003B-011 server configuration; send RC-004 synthetic 165K prefill with minimal output | Actual admitted prompt tokens; HTTP outcome; retained tokens; truncation state; RAM/VRAM before and after |
| CC-003B-013 | Reproducibility confirmation | Stop and restart CC-003B-011 unchanged; repeat RC-001 | Fresh startup, loaded-idle resources, HTTP outcome, timings, and no truncation |
| CC-003B-014+ | Placement or other memory control | Change one named control with a written hypothesis | Same measurement set and explicit comparison to parent candidate |

## Context Ladder

Test context from lower to higher values. The initial feasible value is not automatically the selected value.

1. CC-003B-001 sets the conservative first context value to 32768, materially below 262144.
2. If it loads, increase context in a documented step.
3. If it fails, decrease context before changing another parameter family.
4. Stop increasing when the candidate violates the stability or practical-use gate.

## Measurement Contract

For every candidate, record:

- candidate and parent candidate ID;
- one-sentence hypothesis;
- exact changed parameters and exact command (with local paths redacted in committed documents);
- pre-start and loaded-idle RAM/VRAM plus GPU temperature/power/utilization;
- startup outcome, actual context, and runtime-reported placement;
- cold-load time where valid;
- frozen request-contract identity, prompt-token count, generated-token count, prefill, generation, and total duration;
- operator observation and failure evidence, if any.

## Decision Rules

- A failed candidate remains failed; a later successful configuration does not overwrite it.
- Compare candidates only when their prompt, runtime, model identity, and measurement procedure match.
- A higher context wins only when it meets the same stability and interactive-use gates as the lower candidate.
- A faster candidate wins only when its context, placement, and resource headroom are stated alongside the timing.
- Record a recommended configuration only after at least one accepted candidate has complete startup and bounded-performance evidence.
- A configured context is not treated as validated usable capacity until a large-context request records its admitted prompt size and `truncated=false`.
