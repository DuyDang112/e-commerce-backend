#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/SchelduledJob/Hangfire.Api/Hangfire.Api.csproj", "Services/SchelduledJob/Hangfire.Api/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Contract/Contract.csproj", "BuildingBlocks/Contract/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Infrastructures/Infrastructures.csproj", "BuildingBlocks/Infrastructures/"]
RUN dotnet restore "Services/SchelduledJob/Hangfire.Api/Hangfire.Api.csproj"
COPY . .
WORKDIR "/src/Services/SchelduledJob/Hangfire.Api"
RUN dotnet build "Hangfire.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hangfire.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hangfire.Api.dll"]