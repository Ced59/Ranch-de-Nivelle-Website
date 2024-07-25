# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RanchDuBonheur.csproj", "./"]
RUN dotnet restore "RanchDuBonheur.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "RanchDuBonheur.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RanchDuBonheur.csproj" -c Release -o /app/publish

# Copy the app to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RanchDuBonheur.dll"]
