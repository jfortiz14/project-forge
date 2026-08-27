# CC-003A-002 Pre-Start Evidence — Reduced Background GPU Load

> **Experiment:** `FORGE-CC-003A`  
> **Run:** `CC-003A-002`  
> **Boundary:** Runtime, model artifacts, context, and frozen community flags remain unchanged from CC-003A-001.

## Observed Pre-Start State

| Resource | Observed value |
| --- | --- |
| Free physical memory | 50,738 MiB |
| GPU VRAM total / used / free | 8,192 / 1,280 / 6,739 MiB |
| GPU display-active state | Off |
| GPU temperature / power / utilization | 40 °C / 14.48 W / 6% |

## Comparison to CC-003A-001

The retry begins with 57 MiB more reported free VRAM and 122 MiB less free physical memory than CC-003A-001. These are observed transient-state differences only; no configuration parameter was changed.
