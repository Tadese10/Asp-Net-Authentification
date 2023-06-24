FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroServices/OtherService/OtherService.csproj", "OtherService/"]
RUN dotnet restore "MicroServices/OtherService/OtherService.csproj"
COPY . .
WORKDIR "/src/MicroServices/OtherService"
RUN dotnet build "OtherService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OtherService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OtherService.dll"]
# ENTRYPOINT ["ls", "."]