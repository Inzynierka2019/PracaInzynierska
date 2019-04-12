FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-stretch AS build
WORKDIR /src
COPY ["Inzynierka/Inzynierka.csproj", "Inzynierka/"]
RUN dotnet restore "Inzynierka/Inzynierka.csproj"
COPY . .
WORKDIR "/src/Inzynierka"
RUN dotnet build "Inzynierka.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Inzynierka.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Inzynierka.dll"]