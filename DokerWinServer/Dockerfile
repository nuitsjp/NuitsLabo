# # escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2019
LABEL maintainer="nuits.jp@live.jp"

RUN powershell -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -Channel 6.0 -Quality preview -InstallDir 'C:\Program Files\dotnet'" && \
        setx /M DOTNET_ROOT "C:\Program Files\dotnet\" && \
        setx /M PATH "%PATH%;C:\Program Files\dotnet" && \
        setx /M DOTNET_MULTILEVEL_LOOKUP 0
# SHELL ["powershell", "-ExecutionPolicy", "unrestricted", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# RUN mkdir work
# COPY script/dotnet-install.ps1 work
# RUN CD work; `
#     .\dotnet-install.ps1 -Channel 6.0 -Quality preview -InstallDir 'C:\Program Files\dotnet'
