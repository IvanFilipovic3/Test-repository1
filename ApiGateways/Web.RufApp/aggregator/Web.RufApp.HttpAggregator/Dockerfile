FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/ApiGateways/Web.RufApp/aggregator/Web.RufApp.HttpAggregator/Web.RufApp.HttpAggregator.csproj", "src/ApiGateways/Web.RufApp/aggregator/Web.RufApp.HttpAggregator/"]
RUN dotnet restore "src/ApiGateways/Web.RufApp/aggregator/Web.RufApp.HttpAggregator/Web.RufApp.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/Web.RufApp/aggregator/Web.RufApp.HttpAggregator"
RUN dotnet build "Web.RufApp.HttpAggregator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Web.RufApp.HttpAggregator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Web.RufApp.HttpAggregator.dll"]
