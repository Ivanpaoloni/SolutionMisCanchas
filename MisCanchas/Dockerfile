# Usa la imagen de .NET 7
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MisCanchas/MisCanchas.csproj", "MisCanchas/"]
RUN dotnet restore "MisCanchas/MisCanchas.csproj"
COPY . .
WORKDIR "/src/MisCanchas"
RUN dotnet build "MisCanchas.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MisCanchas.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MisCanchas.dll"]
