# Architecture

- UI: WPF desktop app.
- Settings: local JSON defaults.
- Script validation: balanced bracket-count heuristic only.
- Execution: optional external Lua process with a five-second timeout.

Standard Lua does not provide Roblox APIs. Full Luau parsing, editor tooling, StreamHide, chat, and authorized Studio integration are not implemented in this foundation. No arbitrary process injection or security-boundary bypass is provided.