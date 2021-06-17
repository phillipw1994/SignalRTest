#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Paws/Prototype/Race.Paws.Core.Api.SignalR.Web/Race.Paws.Core.Api.SignalR.Web.csproj", "Paws/Prototype/Race.Paws.Core.Api.SignalR.Web/"]
RUN dotnet restore "Paws/Prototype/Race.Paws.Core.Api.SignalR.Web/Race.Paws.Core.Api.SignalR.Web.csproj"
COPY . .
WORKDIR "/src/Paws/Prototype/Race.Paws.Core.Api.SignalR.Web"
RUN dotnet build "Race.Paws.Core.Api.SignalR.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Race.Paws.Core.Api.SignalR.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Race.Paws.Core.Api.SignalR.Web.dll"]