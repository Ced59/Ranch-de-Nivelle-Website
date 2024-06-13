# Base image avec le runtime ASP.NET Core 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Image de construction avec le SDK .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie et restauration des dépendances (packages NuGet)
COPY ["RanchDuBonheur.csproj", "./"]
RUN dotnet restore "RanchDuBonheur.csproj"

# Copie du code source et construction du projet
COPY . .
RUN dotnet build "RanchDuBonheur.csproj" -c Release -o /app/build

# Publication du projet
FROM build AS publish
RUN dotnet publish "RanchDuBonheur.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Image finale avec l'application publiée
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RanchDuBonheur.dll"]