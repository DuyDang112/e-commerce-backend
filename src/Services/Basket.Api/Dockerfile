#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Basket.Api/Basket.Api.csproj", "Services/Basket.Api/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Contract/Contract.csproj", "BuildingBlocks/Contract/"]
COPY ["BuildingBlocks/Infrastructures/Infrastructures.csproj", "BuildingBlocks/Infrastructures/"]
COPY ["BuildingBlocks/EventBus/EvenBus.Messages/EvenBus.Messages.csproj", "BuildingBlocks/EventBus/EvenBus.Messages/"]
RUN dotnet restore "Services/Basket.Api/Basket.Api.csproj"

COPY . .
WORKDIR "/src/Services/Basket.Api"
RUN dotnet build "Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.Api.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]