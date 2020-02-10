FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/ApiGateways/ApiGw-Base/ApiGw-Base.csproj", "src/ApiGateways/ApiGw-Base/"]
RUN dotnet restore "src/ApiGateways/ApiGw-Base/ApiGw-Base.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/ApiGw-Base"
RUN dotnet build "ApiGw-Base.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ApiGw-Base.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiGw-Base.dll"]