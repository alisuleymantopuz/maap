FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY workshopManagementEventHandler/workshopManagementEventHandler.csproj workshopManagementEventHandler/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "workshopManagementEventHandler/workshopManagementEventHandler.csproj"
COPY . .
WORKDIR "/src/workshopManagementEventHandler"
RUN dotnet build "workshopManagementEventHandler.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "workshopManagementEventHandler.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "workshopManagementEventHandler.dll"]
