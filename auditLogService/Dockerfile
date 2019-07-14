FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY auditLogService/auditLogService.csproj auditLogService/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "auditLogService/auditLogService.csproj"
COPY . .
WORKDIR "/src/auditLogService"
RUN dotnet build "auditLogService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "auditLogService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "auditLogService.dll"]
