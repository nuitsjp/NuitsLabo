# Repository Guidelines

- ユーザーとの対話は日本語で行う事
- t_wada式TDDを徹底すること

## Project Structure & Module Organization
SendFtpTestStudy.sln ties together the main library under SendFtpTestStudy/ and the mirrored xUnit suite in SendFtpTestStudy.Tests/. Source namespaces should mirror folder names, keeping each public type in its own file. Generated build output such as in/ and obj/ stays untracked; clear these directories before zipping or packaging.

## Build, Test, and Development Commands
Run dotnet restore to fetch NuGet dependencies and tooling. Use dotnet build SendFtpTestStudy.sln for a full compile with nullable checks enabled. Execute dotnet test SendFtpTestStudy.Tests/SendFtpTestStudy.Tests.csproj --collect:""XPlat Code Coverage"" to run the suite and emit coverlet-compatible reports. When experimenting locally, prefer creating throwaway Git branches rather than editing directly on main.

## Coding Style & Naming Conventions
Adhere to C# conventions: file-scoped namespaces, 4-space indentation, PascalCase for public members, and _camelCase for private fields. Nullability must remain enabled (<Nullable>enable</Nullable>), so resolve warnings before merging. Keep using directives minimal and explicit, and align namespaces with their folder structure.

## Testing Guidelines
All unit tests live in SendFtpTestStudy.Tests and mirror production namespaces. Use xUnit [Fact] or [Theory] methods with names like MethodUnderTest_StateUnderTest_ExpectedOutcome. Aim for high coverage on FTP edge cases; review the coverlet output and extend tests whenever new behaviors or bug fixes appear.

## Commit & Pull Request Guidelines
Write imperative commit subjects (e.g., Add connection retry logic), grouping related changes together. Pull requests should reference relevant issues, summarize functional changes, and list validation steps such as dotnet build and dotnet test. Include screenshots or logs whenever behavior varies from previous releases.

## Security & Configuration Tips
Store FTP credentials in environment variables or user secrets, never in code or test data. Scrub sensitive details from logs and artifacts before sharing. Revisit .gitignore when adding tools that produce new build output to keep secrets and binaries out of version control.
