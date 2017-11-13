FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 4123
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "LOGA_WebUI_core.dll"]
