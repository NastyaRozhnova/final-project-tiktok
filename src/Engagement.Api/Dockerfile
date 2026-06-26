FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props .editorconfig ./
COPY src/Shared/ src/Shared/
COPY src/Engagement.Domain/ src/Engagement.Domain/
COPY src/Engagement.Application/ src/Engagement.Application/
COPY src/Engagement.Infrastructure/ src/Engagement.Infrastructure/
COPY src/Engagement.Api/ src/Engagement.Api/

RUN dotnet restore src/Engagement.Api/Engagement.Api.csproj
RUN dotnet publish src/Engagement.Api/Engagement.Api.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "TikTokFeed.Engagement.Api.dll"]
