FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 443
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./Modelling.API.csproj", "./"]
RUN dotnet restore "./Modelling.API.csproj"
COPY . .
WORKDIR "/src/Modelling.API"
RUN dotnet build "Modelling.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Modelling.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Modelling.API.dll"]