# Utiliser l'image de base officielle de Microsoft pour ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copier le fichier csproj et restaurer les dépendances
COPY RanchDuBonheur.csproj .
RUN dotnet restore "RanchDuBonheur.csproj"
# Copier le reste des fichiers du projet
COPY . .
RUN dotnet build "RanchDuBonheur.csproj" -c Release -o /app/build

# Étape de publication
FROM build AS publish
RUN dotnet publish "RanchDuBonheur.csproj" -c Release -o /app/publish

# Étape finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RanchDuBonheur.dll"]
