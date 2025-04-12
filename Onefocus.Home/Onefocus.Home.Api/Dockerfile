# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Onefocus.Home/Onefocus.Home.Api/Onefocus.Home.Api.csproj", "Onefocus.Home/Onefocus.Home.Api/"]
COPY ["Onefocus.Common/Onefocus.Common.csproj", "Onefocus.Common/"]
COPY ["Onefocus.Home/Onefocus.Home.Application/Onefocus.Home.Application.csproj", "Onefocus.Home/Onefocus.Home.Application/"]
COPY ["Onefocus.Home/Onefocus.Home.Infrastructure/Onefocus.Home.Infrastructure.csproj", "Onefocus.Home/Onefocus.Home.Infrastructure/"]
RUN dotnet restore "./Onefocus.Home/Onefocus.Home.Api/Onefocus.Home.Api.csproj"
COPY . .
WORKDIR "/src/Onefocus.Home/Onefocus.Home.Api"
RUN dotnet build "./Onefocus.Home.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Onefocus.Home.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Onefocus.Home.Api.dll"]