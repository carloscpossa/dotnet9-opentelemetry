FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CidadesApi/CidadesApi.csproj", "CidadesApi/"]
RUN dotnet restore "CidadesApi/CidadesApi.csproj"
COPY . .
WORKDIR "/src/CidadesApi"
RUN dotnet publish "CidadesApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CidadesApi.dll"]
