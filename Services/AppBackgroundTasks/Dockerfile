FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Services/AppBackgroundTasks/AppBackgroundTasks.csproj", "src/Services/AppBackgroundTasks/"]
RUN dotnet restore "src/Services/AppBackgroundTasks/AppBackgroundTasks.csproj"
COPY . .
WORKDIR "/src/src/Services/AppBackgroundTasks"
RUN dotnet build "AppBackgroundTasks.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AppBackgroundTasks.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AppBackgroundTasks.dll"]
