FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY vehicleManagementAPI/vehicleManagementAPI.csproj vehicleManagementAPI/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "vehicleManagementAPI/vehicleManagementAPI.csproj"
COPY . .
WORKDIR "/src/vehicleManagementAPI"
RUN dotnet build "vehicleManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "vehicleManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Expose ports
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5000/hc || exit 1


ENTRYPOINT ["dotnet", "vehicleManagementAPI.dll"]
