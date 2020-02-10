FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Services/Orders.API/Orders.API.csproj", "src/Services/Orders.API/"]
RUN dotnet restore "src/Services/Orders.API/Orders.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Orders.API"
RUN dotnet build "Orders.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Orders.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orders.API.dll"]