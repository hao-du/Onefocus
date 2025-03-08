#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Onefocus.Wallet/Onefocus.Wallet.Api/Onefocus.Wallet.Api.csproj", "Onefocus.Wallet/Onefocus.Wallet.Api/"]
RUN dotnet restore "./Onefocus.Wallet/Onefocus.Wallet.Api/Onefocus.Wallet.Api.csproj"
COPY . .
WORKDIR "/src/Onefocus.Wallet/Onefocus.Wallet.Api"
RUN dotnet build "./Onefocus.Wallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Onefocus.Wallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Onefocus.Wallet.Api.dll"]