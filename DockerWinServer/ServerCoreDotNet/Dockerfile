# escape=`

FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-20210713-windowsservercore-ltsc2019
LABEL maintainer="nuits.jp@live.jp"

SHELL ["powershell", "-ExecutionPolicy", "unrestricted", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

COPY scripts C:\scripts
RUN CD scripts; `
    .\install-dotnet.ps1
RUN Remove-Item C:\scripts -Recurse  -Force

RUN dotnet tool install --global PowerShell
