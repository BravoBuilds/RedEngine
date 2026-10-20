# RedEngine

A Windows desktop Lua/Luau local-development workspace inspired by the supplied RedEngine references.

> **Status:** early foundation. The Windows CI pipeline builds, tests, and publishes a portable x64 executable. Some modules remain placeholders and the app is not a production Roblox executor.

## Build and test
Requires Windows and the .NET 8 SDK.

```powershell
dotnet restore RedEngine.sln
dotnet build RedEngine.sln --configuration Release
dotnet test RedEngine.sln --configuration Release
```

## Publish a portable EXE
```powershell
dotnet publish src/RedEngine.App/RedEngine.App.csproj --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=false --output artifacts/RedEngine-win-x64
```

The output is `artifacts/RedEngine-win-x64/RedEngine.exe`. A self-contained publish bundles the .NET runtime. Do not enable trimming for this WPF app.

## Automated builds
GitHub Actions builds and tests on Windows and uploads a `RedEngine-win-x64` artifact. Download it from the successful run's **Artifacts** section: [Actions](https://github.com/BravoBuilds/RedEngine/actions).

## Included features
- RedEngine-themed home screen
- Open/save Lua and Luau files
- Basic bracket validation (heuristic only, not a full parser)
- Optional local Lua runtime execution with a timeout
- Local JSON settings defaults
- Local file index/import service
- Unit tests and CI publish pipeline

## Not yet implemented
Full Luau syntax analysis/highlighting, live chat, full settings editor, complete Resources/Dumper UI, StreamHide, installer, and authorized Roblox Studio integration.

Standard Lua does not provide Roblox APIs. This application does not inject into Roblox processes, bypass security controls, or retrieve protected source. No license has been selected; choose one before redistribution.
