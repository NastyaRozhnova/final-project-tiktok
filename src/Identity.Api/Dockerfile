FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props .editorconfig ./
COPY src/Shared/ src/Shared/
COPY src/Identity.Domain/ src/Identity.Domain/
COPY src/Identity.Application/ src/Identity.Application/
COPY src/Identity.Infrastructure/ src/Identity.Infrastructure/
COPY src/Identity.Api/ src/Identity.Api/

RUN dotnet restore src/Identity.Api/Identity.Api.csproj
RUN dotnet publish src/Identity.Api/Identity.Api.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "TikTokFeed.Identity.Api.dll"]
