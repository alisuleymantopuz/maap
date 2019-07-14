FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY WebApp/WebApp.csproj WebApp/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "WebApp/WebApp.csproj"
COPY . .
WORKDIR "/src/WebApp"
RUN dotnet build "WebApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Expose ports
EXPOSE 7000/tcp
ENV ASPNETCORE_URLS http://*:7000
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:7000/hc || exit 1


ENTRYPOINT ["dotnet", "WebApp.dll"]
