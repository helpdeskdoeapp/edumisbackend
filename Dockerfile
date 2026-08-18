# Stage 1: Runtime Base Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 10000

# Create application data and log directories
RUN mkdir -p /app/uploads /app/logs /app/service_logs/maillogs /app/service_logs/smslogs

# Stage 2: SDK Build Image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files for layer caching
COPY ["edumisbackend.sln", "./"]
COPY ["edumisbackend/edumisbackend.csproj", "edumisbackend/"]
COPY ["edumis.Common/edumis.Common.csproj", "edumis.Common/"]
COPY ["edumis.DataAccess/edumis.DataAccess.csproj", "edumis.DataAccess/"]
COPY ["edumis.Models/edumis.Models.csproj", "edumis.Models/"]

# Restore dependencies
RUN dotnet restore "edumisbackend.sln"

# Copy remaining source code and build
COPY . .
WORKDIR "/src/edumisbackend"
RUN dotnet build "edumisbackend.csproj" -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "edumisbackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final Runtime Container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set default ASP.NET Core environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV DOTNET_SYSTEM_IO_DISABLEFILEWATCHING=true

ENTRYPOINT ["dotnet", "edumisbackend.dll"]

