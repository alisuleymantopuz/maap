FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY workshopManagementAPI/workshopManagementAPI.csproj workshopManagementAPI/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "workshopManagementAPI/workshopManagementAPI.csproj"
COPY . .
WORKDIR "/src/workshopManagementAPI"
RUN dotnet build "workshopManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "workshopManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Expose ports
EXPOSE 5200/tcp
ENV ASPNETCORE_URLS http://*:5200
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5200/hc || exit 1

ENTRYPOINT ["dotnet", "workshopManagementAPI.dll"]
