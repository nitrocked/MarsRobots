#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["MarsRobots.REST/MarsRobots.REST.csproj", "MarsRobots.REST/"]
COPY ["MarsRobots.Entities/MarsRobots.Entities.csproj", "MarsRobots.Entities/"]
COPY ["MarsRobots.Business/MarsRobots.Business.csproj", "MarsRobots.Business/"]
COPY ["MarsRobots.Persistence/MarsRobots.Persistence.csproj", "MarsRobots.Persistence/"]
RUN dotnet restore "MarsRobots.REST/MarsRobots.REST.csproj"
COPY . .
WORKDIR "/src/MarsRobots.REST"
RUN dotnet build "MarsRobots.REST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarsRobots.REST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarsRobots.REST.dll"]