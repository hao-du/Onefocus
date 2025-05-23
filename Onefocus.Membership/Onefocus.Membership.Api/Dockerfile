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
COPY ["Onefocus.Membership/Onefocus.Membership.Api/Onefocus.Membership.Api.csproj", "Onefocus.Membership/Onefocus.Membership.Api/"]
COPY ["Onefocus.Membership/Onefocus.Membership.Application/Onefocus.Membership.Application.csproj", "Onefocus.Membership/Onefocus.Membership.Application/"]
COPY ["Onefocus.Membership/Onefocus.Membership.Infrastructure/Onefocus.Membership.Infrastructure.csproj", "Onefocus.Membership/Onefocus.Membership.Infrastructure/"]
COPY ["Onefocus.Membership/Onefocus.Membership.Domain/Onefocus.Membership.Domain.csproj", "Onefocus.Membership/Onefocus.Membership.Domain/"]
COPY ["Onefocus.Common/Onefocus.Common.csproj", "Onefocus.Common/"]
RUN dotnet restore "./Onefocus.Membership/Onefocus.Membership.Api/Onefocus.Membership.Api.csproj"
COPY . .
WORKDIR "/src/Onefocus.Membership/Onefocus.Membership.Api"
RUN dotnet build "./Onefocus.Membership.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Onefocus.Membership.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Onefocus.Membership.Api.dll"]