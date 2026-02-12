FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/ProductService/ProductService.API/ProductService.API.csproj", "Services/ProductService/ProductService.API/"]
COPY ["src/Services/ProductService/ProductService.Infrastructure/ProductService.Infrastructure.csproj", "Services/ProductService/ProductService.Infrastructure/"]
COPY ["src/Services/ProductService/ProductService.Domain/ProductService.Domain.csproj", "Services/ProductService/ProductService.Domain/"]
COPY ["src/Services/ProductService/ProductService.Application/ProductService.Application.csproj", "Services/ProductService/ProductService.Application/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["src/Shared/Shared.Utils/Shared.Utils.csproj", "Shared/Shared.Utils/"]

RUN dotnet restore "Services/ProductService/ProductService.API/ProductService.API.csproj"
COPY ["src/", "."]
WORKDIR "/src/Services/ProductService/ProductService.API"
RUN dotnet build "./ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "ProductService.API.dll"]
