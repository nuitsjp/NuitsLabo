# Repository Guidelines

- ユーザーとの対話は日本語で行う
- t_wada式TDDを徹底する

## Project Structure & Module Organization
- `SendFtpTestStudy.sln` binds the main library in `SendFtpTestStudy/` and the xUnit suite in `SendFtpTestStudy.Tests/`.
- Keep product code under `SendFtpTestStudy/` with namespaces matching folder names; reserve `SendFtpTestStudy.Tests/` for mirrors of the production namespaces.
- Generated `bin/` and `obj/` folders should remain untracked; clear them before packaging artifacts.

## Build, Test, and Development Commands
- `dotnet restore` ensures NuGet dependencies (xUnit, coverlet, SDK tooling) are present.
- `dotnet build SendFtpTestStudy.sln` compiles all projects with nullable reference checks enabled.
- `dotnet test SendFtpTestStudy.Tests/SendFtpTestStudy.Tests.csproj --collect:"XPlat Code Coverage"` runs the suite and emits coverage supported by the bundled coverlet collector.

## Coding Style & Naming Conventions
- Follow standard C# conventions: 4-space indentation, file-scoped namespaces, PascalCase for public types/members, and `_camelCase` for private fields.
- Enable nullability warnings (`<Nullable>enable</Nullable>`) and implicit usings already configured in each `.csproj`; address analyzer warnings before committing.
- Organize classes so each file owns a single public type; prefer targeted `using` directives over wildcard imports.

## Testing Guidelines
- Write unit tests with xUnit `[Fact]` or `[Theory]`; mirror production namespaces (e.g., `SendFtpTestStudy.Tests.FtpClientTest`).
- Name tests `MethodUnderTest_StateUnderTest_ExpectedOutcome` for clarity and keep setup logic in private helpers.
- Aim for high coverage on FTP edge cases; use coverlet output to spot gaps and export reports when coverage drops.

## Commit & Pull Request Guidelines
- Keep commit subjects in the imperative mood (e.g., `Add connection retry logic`); short but descriptive messages match the existing history.
- Group related changes per commit and include context in the body when the summary needs elaboration or localization notes.
- Pull requests should link relevant issues, summarize functional changes, list validation steps (build/test commands), and attach screenshots when UI behavior is affected.

## Security & Configuration Tips
- Store FTP credentials in environment variables or user secrets; never hard-code them in `FtpClient` or tests.
- Scrub logs and artifacts before sharing, and review `.gitignore` updates whenever adding new tooling.
