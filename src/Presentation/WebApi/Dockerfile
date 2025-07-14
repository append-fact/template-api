FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER app
WORKDIR /app
EXPOSE 8087
EXPOSE 8088

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/WebApi/WebApi.csproj", "Presentation/WebApi/"]
COPY ["src/Core/Application/Application.csproj", "Core/Application/"]
COPY ["src/Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["src/Infrastructure/Persistence/Persistence.csproj", "Infrastructure/Persistence/"]
COPY ["src/Infrastructure/Identity/Identity.csproj", "Infrastructure/Identity/"]
COPY ["src/Infrastructure/Shared/Shared.csproj", "Infrastructure/Shared/"]
RUN dotnet restore "./Presentation/WebApi/WebApi.csproj"

COPY /src .
WORKDIR "/src/Presentation/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]

