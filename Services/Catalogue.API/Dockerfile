FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Services/Catalogue.API/Catalogue.API.csproj", "src/Services/Catalogue.API/"]
COPY ["src/EventBus/EventBus/EventBus.csproj", "src/EventBus/EventBus/"]
RUN dotnet restore "src/Services/Catalogue.API/Catalogue.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Catalogue.API"
RUN dotnet build "Catalogue.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Catalogue.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Catalogue.API.dll"]