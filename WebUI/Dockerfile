FROM microsoft/dotnet:2.2-aspnetcore-runtime
ARG source
WORKDIR /app
EXPOSE 4123
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "LOGA-WebUI.dll"]
