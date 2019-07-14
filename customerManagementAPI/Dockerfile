FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY customerManagementAPI/customerManagementAPI.csproj customerManagementAPI/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "customerManagementAPI/customerManagementAPI.csproj"
COPY . .
WORKDIR "/src/customerManagementAPI"
RUN dotnet build "customerManagementAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "customerManagementAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Expose ports
EXPOSE 5100/tcp
ENV ASPNETCORE_URLS http://*:5100
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5100/hc || exit 1

ENTRYPOINT ["dotnet", "customerManagementAPI.dll"]
