#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Server/Jukebox.Server.csproj", "Server/"]
COPY ["Shared/Jukebox.Shared.csproj", "Shared/"]
COPY ["Jukebox.Player/Jukebox.Player.csproj", "Jukebox.Player/"]
COPY ["Jukebox.Player.YouTube/Jukebox.Player.YouTube.csproj", "Jukebox.Player.YouTube/"]
COPY ["Client/Jukebox.Client.csproj", "Client/"]
RUN dotnet restore "Server/Jukebox.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Jukebox.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jukebox.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jukebox.Server.dll"]
