# Сборка из корня репозитория: docker build -f src/Content.Api/Dockerfile .
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props .editorconfig ./
COPY src/Shared/ src/Shared/
COPY src/Content.Domain/ src/Content.Domain/
COPY src/Content.Application/ src/Content.Application/
COPY src/Content.Infrastructure/ src/Content.Infrastructure/
COPY src/Content.Api/ src/Content.Api/

RUN dotnet restore src/Content.Api/Content.Api.csproj
RUN dotnet publish src/Content.Api/Content.Api.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "TikTokFeed.Content.Api.dll"]
