# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Architecture

This solution contains two separate FTP client libraries with mirrored test projects:

### Main Libraries
- `SendFtpTestStudy/` - Regular FTP client using FluentFTP library
- `SendSftpTestStudy/` - SFTP client using SSH.NET library

### Test Projects
- `SendFtpTestStudy.Tests/` - xUnit tests for FTP client with FubarDev.FtpServer for test fixtures
- `SendSftpTestStudy.Tests/` - xUnit tests for SFTP client with embedded SFTP server

### Core Client Design
Both clients follow similar patterns:
- `UploadAsync` method with connection options, remote path, stream content, and cancellation token
- Path normalization (Windows backslashes to Unix forward slashes)
- Directory creation before upload
- Stream position reset for uploads
- Connection management with proper disposal

## Build and Test Commands

### Build
```bash
dotnet restore
dotnet build SendFtpTestStudy.sln
```

### Testing
```bash
# Run all tests
dotnet test

# Run tests with code coverage
dotnet test SendFtpTestStudy.Tests/SendFtpTestStudy.Tests.csproj --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test SendSftpTestStudy.Tests/SendSftpTestStudy.Tests.csproj
```

## Development Guidelines

### Language and TDD
- ユーザーとの対話は日本語で行う (Japanese communication)
- t_wada式TDDを徹底する (Follow t_wada-style TDD strictly)

### Code Conventions
- .NET 8.0 target framework with nullable reference types enabled
- File-scoped namespaces and implicit usings
- 4-space indentation, PascalCase for public members, `_camelCase` for private fields
- Mirror production namespaces in test projects

### Testing Infrastructure
- Test servers run on dynamically allocated ports using `PortHelper.GetAvailablePort()`
- FTP tests use `FtpServerFixture` with FubarDev.FtpServer
- SFTP tests use `SftpServerFixture` with FxSshSftpService
- Tests follow `MethodUnderTest_StateUnderTest_ExpectedOutcome` naming convention

### Security
- Never hardcode FTP/SFTP credentials in source code
- Use environment variables or user secrets for test credentials
- Scrub logs and artifacts before sharing