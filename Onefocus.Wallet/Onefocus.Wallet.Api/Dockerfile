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
COPY ["Onefocus.Wallet/Onefocus.Wallet.Api/Onefocus.Wallet.Api.csproj", "Onefocus.Wallet/Onefocus.Wallet.Api/"]
COPY ["Onefocus.Wallet/Onefocus.Wallet.Application/Onefocus.Wallet.Application.csproj", "Onefocus.Wallet/Onefocus.Wallet.Application/"]
COPY ["Onefocus.Wallet/Onefocus.Wallet.Infrastructure/Onefocus.Wallet.Infrastructure.csproj", "Onefocus.Wallet/Onefocus.Wallet.Infrastructure/"]
COPY ["Onefocus.Wallet/Onefocus.Wallet.Domain/Onefocus.Wallet.Domain.csproj", "Onefocus.Wallet/Onefocus.Wallet.Domain/"]
COPY ["Onefocus.Common/Onefocus.Common.csproj", "Onefocus.Common/"]
RUN dotnet restore "./Onefocus.Wallet/Onefocus.Wallet.Api/Onefocus.Wallet.Api.csproj"
COPY . .
WORKDIR "/src/Onefocus.Wallet/Onefocus.Wallet.Api"
RUN dotnet build "./Onefocus.Wallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Onefocus.Wallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Onefocus.Wallet.Api.dll"]