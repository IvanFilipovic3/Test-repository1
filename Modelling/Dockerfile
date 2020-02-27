#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Modelling.API/Modelling.API.csproj", "Modelling.API/"]
RUN dotnet restore "Modelling.API/Modelling.API.csproj"
COPY . .
WORKDIR "/src/Modelling.API"
RUN dotnet build "Modelling.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Modelling.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY bin/Release/netcoreapp3.1/linux-x64/publish .
ENTRYPOINT ["dotnet", "Modelling.API.dll"]
