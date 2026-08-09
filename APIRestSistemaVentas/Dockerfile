FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER $APP_UID
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV CONNECTION_STRING="Data Source=192.168.100.133,1433;Initial Catalog=Sistema_Supermercado;User Id=user_sistema_venta;Password=ApiSistemaVenta;TrustServerCertificate=True;"

# BUILD
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["APIRestSistemaVentas/APIRestSistemaVentas.csproj", "APIRestSistemaVentas/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

RUN dotnet restore "./APIRestSistemaVentas/APIRestSistemaVentas.csproj"

COPY . .
WORKDIR "/src/APIRestSistemaVentas"

RUN dotnet build "./APIRestSistemaVentas.csproj" -c $BUILD_CONFIGURATION -o /app/build

# PUBLISH
FROM build AS publish

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./APIRestSistemaVentas.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FINAL
FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "APIRestSistemaVentas.dll"]
