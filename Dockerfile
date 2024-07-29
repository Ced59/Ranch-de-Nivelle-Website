# Utiliser l'image de base officielle de Microsoft pour ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# �tape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Assurez-vous que le chemin dans COPY est correct; ajustez selon la structure de votre projet
COPY ["RanchDuBonheur/RanchDuBonheur.csproj", "RanchDuBonheur/"]
RUN dotnet restore "RanchDuBonheur/RanchDuBonheur.csproj"
# Assurez-vous que cette COPY copie tous les fichiers n�cessaires � partir de votre r�pertoire de travail local
COPY . .
WORKDIR "/src/RanchDuBonheur"
RUN dotnet build "RanchDuBonheur.csproj" -c Release -o /app/build

# �tape de publication
FROM build AS publish
RUN dotnet publish "RanchDuBonheur.csproj" -c Release -o /app/publish

# �tape finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RanchDuBonheur.dll"]
