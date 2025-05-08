# Etapa 1: construir el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: imagen final mínima
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
COPY certs/devcert.pfx ./certs/devcert.pfx
EXPOSE 9095
ENV ASPNETCORE_URLS=https://+:9095

ENTRYPOINT ["dotnet", "RetroTrackRestNet.dll"]
