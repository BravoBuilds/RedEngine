# RedEngine

Windows desktop Lua/Luau local-development workspace based on the supplied RedEngine visual references.

> **Status:** initial foundation, not a production release. Some modules are informational placeholders until integrations are implemented.

## Requirements
- Windows 10/11
- .NET 8 SDK
- Optional: Lua runtime available as `lua` on PATH for local Lua execution

## Build and run
```powershell
dotnet restore RedEngine.sln
dotnet build RedEngine.sln -c Release
dotnet test RedEngine.sln -c Release
dotnet run --project src/RedEngine.App/RedEngine.App.csproj
```

The project uses WPF and should be built on Windows with the .NET 8 Windows Desktop targeting pack.

## Included
- Seven-tile RedEngine home screen
- Local Lua/Luau text editing with open/save/new
- Basic bracket-count validation (not a full parser)
- Optional local Lua runtime invocation with a timeout
- Local file indexing, filename filtering, and collision-safe import
- Local JSON settings defaults
- Unit tests and Windows GitHub Actions workflow

## Incomplete modules
Full Luau parsing/highlighting, production licensing, StreamHide, complete settings UI, utilities, community chat, and authorized Roblox Studio integration remain to be implemented. Standard Lua does not provide Roblox APIs. This app does not implement arbitrary Roblox process injection, security bypasses, or extraction of protected source.

## Local data
Settings: `%LOCALAPPDATA%\RedEngine\settings.json`  
Imported files: `%LOCALAPPDATA%\RedEngine\Local\Scripts\`

See [docs/USER_MANUAL.md](docs/USER_MANUAL.md), [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md), [docs/CHAT_BACKEND.md](docs/CHAT_BACKEND.md), and [docs/STREAMHIDE.md](docs/STREAMHIDE.md).

No license has been selected. Add a LICENSE file before redistribution.