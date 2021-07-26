# escape=`

FROM nuitsjp/server-core-dotnet-6.0:latest
LABEL maintainer="nuits.jp@live.jp"

SHELL ["pwsh", "-ExecutionPolicy", "unrestricted", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ADD scripts C:\scripts
ADD services C:\services
RUN CD scripts; `
    .\Setup-WindowsServices.ps1

ENTRYPOINT ["pwsh.exe", "-WorkingDirectory", "c:\\scripts", "c:\\scripts\\Start-WindowsServices.ps1"]

