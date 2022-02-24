#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/SnookerBet.Web/SnookerBet.Web.csproj", "src/SnookerBet.Web/"]
RUN dotnet restore "src/SnookerBet.Web/SnookerBet.Web.csproj"
COPY . .
WORKDIR "/src/src/SnookerBet.Web"
RUN dotnet build "SnookerBet.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SnookerBet.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SnookerBet.Web.dll"]