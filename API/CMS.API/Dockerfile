# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/CMS.API/CMS.API.csproj", "API/CMS.API/"]
COPY ["CMS.Infrastructure/CMS.Infrastructure.csproj", "CMS.Infrastructure/"]
COPY ["Core/CMS.Application/CMS.Application.csproj", "Core/CMS.Application/"]
COPY ["Core/CMS.Domain/CMS.Domain.csproj", "Core/CMS.Domain/"]
COPY ["Infrastructure/CMS.Persistence/CMS.Persistence.csproj", "Infrastructure/CMS.Persistence/"]
RUN dotnet restore "./API/CMS.API/CMS.API.csproj"
COPY . .
WORKDIR "/src/API/CMS.API"
RUN dotnet build "./CMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMS.API.dll"]