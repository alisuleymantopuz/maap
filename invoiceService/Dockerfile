FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY invoiceService/invoiceService.csproj invoiceService/
COPY infrastructure.messaging/infrastructure.messaging.csproj infrastructure.messaging/
RUN dotnet restore "invoiceService/invoiceService.csproj"
COPY . .
WORKDIR "/src/invoiceService"
RUN dotnet build "invoiceService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "invoiceService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "invoiceService.dll"]
