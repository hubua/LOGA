#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 4123

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BeboenaWebApp/BeboenaWebApp.csproj", "BeboenaWebApp/"]
RUN dotnet restore "BeboenaWebApp/BeboenaWebApp.csproj"
COPY . .
WORKDIR "/src/BeboenaWebApp"
RUN dotnet build "BeboenaWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BeboenaWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK --interval=1m --timeout=3s CMD curl --fail http://localhost:4123/healthcheck || exit 1

ENTRYPOINT ["dotnet", "BeboenaWebApp.dll"]