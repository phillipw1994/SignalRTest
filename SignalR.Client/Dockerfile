#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Race.Paws.Core.Api.SignalR.Client/Race.Paws.Core.Api.SignalR.Client.csproj", "Race.Paws.Core.Api.SignalR.Client/"]
RUN dotnet restore "Race.Paws.Core.Api.SignalR.Client/Race.Paws.Core.Api.SignalR.Client.csproj"
COPY . .
WORKDIR "/src/Race.Paws.Core.Api.SignalR.Client"
RUN dotnet build "Race.Paws.Core.Api.SignalR.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Race.Paws.Core.Api.SignalR.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Race.Paws.Core.Api.SignalR.Client.dll"]