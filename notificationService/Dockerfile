FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY notificationService/notificationService.csproj notificationService/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "notificationService/notificationService.csproj"
COPY . .
WORKDIR "/src/notificationService"
RUN dotnet build "notificationService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "notificationService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "notificationService.dll"]
