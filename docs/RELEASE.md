# Build and release

1. Build and test on Windows with .NET 8 SDK.
2. Run `dotnet restore`, `dotnet build -c Release`, and `dotnet test -c Release`.
3. Publish self-contained with:
   ```powershell
   dotnet publish src/RedEngine.App/RedEngine.App.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/win-x64
   ```
4. Test startup, navigation, local file open/save, no-runtime behavior, and permission failures on a clean Windows account.
5. Do not call the release production-ready until remaining README checklist and integration tests pass.

GitHub Actions builds and tests on Windows; it does not publish a signed installer.