FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY mocks/push-mock/ ./
RUN dotnet publish PushMock.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8091
ENTRYPOINT ["dotnet", "PushMock.dll"]
