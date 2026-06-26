FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY mocks/vision-mock/ ./
RUN dotnet publish VisionMock.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8090
ENTRYPOINT ["dotnet", "VisionMock.dll"]
