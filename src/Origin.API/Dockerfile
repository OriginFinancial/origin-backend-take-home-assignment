FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src 
COPY ["Origin.API/Origin.API.csproj", "Origin.API/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"] 
COPY ["Application/Application.csproj", "Application/"] 
RUN dotnet restore "Origin.API/Origin.API.csproj"
COPY . .
WORKDIR "/src/Origin.API"
RUN dotnet build "Origin.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Origin.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Origin.API.dll"]
