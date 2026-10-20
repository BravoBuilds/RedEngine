# User Manual

## Executor
Use New/Open/Save to edit local `.lua` or `.luau` files. Validate performs only a balanced-bracket heuristic, not full syntax checking. Run local Lua invokes an installed `lua` executable on PATH and stops it after five seconds. It does not emulate Roblox or provide Roblox APIs.

## Other modules
Dumper, Resources, Menus, Misc, and Chat currently display their status and limitations. Chat remains disconnected until a backend is configured.

## Data
Settings are stored in `%LOCALAPPDATA%\RedEngine\settings.json`. Check Windows file permissions if file operations fail.