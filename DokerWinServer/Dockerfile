# escape=`

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-20210713-windowsservercore-ltsc2019
LABEL maintainer="nuits.jp@live.jp"

SHELL ["powershell", "-ExecutionPolicy", "unrestricted", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

RUN mkdir script
COPY script/install-dotnet.ps1 script
RUN CD script; `
    .\install-dotnet.ps1
