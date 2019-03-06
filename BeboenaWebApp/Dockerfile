FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 4123

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY BeboenaWebApp/BeboenaWebApp.csproj BeboenaWebApp/
RUN dotnet restore BeboenaWebApp/BeboenaWebApp.csproj
COPY . .
WORKDIR /src/BeboenaWebApp
RUN dotnet build BeboenaWebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BeboenaWebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

HEALTHCHECK --interval=1m --timeout=3s CMD curl --fail http://localhost:4123/healthcheck || exit 1

ENTRYPOINT ["dotnet", "BeboenaWebApp.dll"]
