# デバッグ コンテナーをカスタマイズする方法と、Visual Studio がこの Dockerfile を使用してより高速なデバッグのためにイメージをビルドする方法については、https://aka.ms/customizecontainer をご覧ください。

# このステージは、VS から高速モードで実行するときに使用されます (デバッグ構成の既定値)
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 8080


# このステージは、サービス プロジェクトのビルドに使用されます
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FunctionApp1/FunctionApp1.csproj", "FunctionApp1/"]
RUN dotnet restore "./FunctionApp1/FunctionApp1.csproj"
COPY . .
WORKDIR "/src/FunctionApp1"
RUN dotnet build "./FunctionApp1.csproj" -c $BUILD_CONFIGURATION -o /app/build

# このステージは、最終ステージにコピーするサービス プロジェクトを公開するために使用されます
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FunctionApp1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# このステージは、運用環境または VS から通常モードで実行している場合に使用されます (デバッグ構成を使用しない場合の既定)
FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true