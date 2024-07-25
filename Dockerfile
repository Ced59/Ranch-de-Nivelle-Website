# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copie et restauration des dépendances (csproj seulement pour optimiser le cache Docker)
COPY *.csproj ./
RUN dotnet restore

# Copie du code source et publication
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Étape d'exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "RanchDuBonheur.dll"]
tea