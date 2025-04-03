# Usa la imagen base de .NET 7
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Usa el SDK de .NET 7 para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia todo el código al contenedor
COPY . .

# Restaura paquetes
RUN dotnet restore "MisCanchas/MisCanchas.csproj"

# Publica el proyecto en modo Release
RUN dotnet publish "MisCanchas/MisCanchas.csproj" -c Release -o /app/publish

# Crea la imagen final con el código publicado
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "MisCanchas.dll"]
