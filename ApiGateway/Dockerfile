FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApiGateway/ApiGatewayService/ApiGatewayService.csproj", "ApiGatewayService/"]
RUN dotnet restore "ApiGatewayService/ApiGatewayService.csproj"
COPY . .
WORKDIR "/src/ApiGateway/ApiGatewayService"
RUN dotnet build "ApiGatewayService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGatewayService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGatewayService.dll"]
