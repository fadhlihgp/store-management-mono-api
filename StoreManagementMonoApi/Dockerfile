FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["store-management-mono-api/store-management-mono-api.csproj", "store-management-mono-api/"]
RUN dotnet restore "store-management-mono-api/store-management-mono-api.csproj"
COPY . .
WORKDIR "/src/store-management-mono-api"
RUN dotnet build "store-management-mono-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "store-management-mono-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "store-management-mono-api.dll"]
