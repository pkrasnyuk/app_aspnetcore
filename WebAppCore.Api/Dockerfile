FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 32768
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "WebAppCore.Api.dll"]