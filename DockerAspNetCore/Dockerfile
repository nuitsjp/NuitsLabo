FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY DockerAspNetFramework/*.csproj ./DockerAspNetFramework/
COPY DockerAspNetFramework/*.config ./DockerAspNetFramework/
RUN nuget restore

# copy everything else and build app
COPY DockerAspNetFramework/. ./DockerAspNetFramework/
WORKDIR /app/DockerAspNetFramework
RUN msbuild /p:Configuration=Release -r:False


FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS runtime

COPY start.ps1 C:/

# Next, this Dockerfile creates a directory for your application
RUN mkdir C:\randomanswers

# configure the new site in IIS.
RUN powershell -NoProfile -Command \
    Import-module IISAdministration; \
    New-IISSite -Name "ASPNET" -PhysicalPath C:\randomanswers -BindingInformation "*:8000:"

# This instruction tells the container to listen on port 8000.
EXPOSE 8000

WORKDIR /randomanswers
COPY --from=build /app/DockerAspNetFramework/. ./

WORKDIR /inetpub/wwwroot
COPY --from=build /app/DockerAspNetFramework/. ./

ENTRYPOINT ["powershell", "c:\\start.ps1"]