FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN mkdir -p "/var/www/Apps/KelolaWarung/Files" "/var/www/Apps/KelolaWarung/Logs/StoreApi"
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . .

RUN dotnet publish -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80
ENTRYPOINT ["dotnet", "StoreManagementMonoApi.dll"]