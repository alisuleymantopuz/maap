FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY timeService/timeService.csproj timeService/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "timeService/timeService.csproj"
COPY . .
WORKDIR "/src/timeService"
RUN dotnet build "timeService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "timeService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "timeService.dll"]
