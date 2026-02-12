FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/InventoryService/InventoryService.API/InventoryService.API.csproj", "Services/InventoryService/InventoryService.API/"]
COPY ["src/Services/InventoryService/InventoryService.Infrastructure/InventoryService.Infrastructure.csproj", "Services/InventoryService/InventoryService.Infrastructure/"]
COPY ["src/Services/InventoryService/InventoryService.Domain/InventoryService.Domain.csproj", "Services/InventoryService/InventoryService.Domain/"]
COPY ["src/Services/InventoryService/InventoryService.Application/InventoryService.Application.csproj", "Services/InventoryService/InventoryService.Application/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["src/Shared/Shared.Utils/Shared.Utils.csproj", "Shared/Shared.Utils/"]

RUN dotnet restore "Services/InventoryService/InventoryService.API/InventoryService.API.csproj"
COPY ["src/", "."]
WORKDIR "/src/Services/InventoryService/InventoryService.API/"
RUN dotnet build "./InventoryService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InventoryService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "InventoryService.API.dll"]
