FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
COPY ./webapp/ App/
WORKDIR /App
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Ianf.Fittrack.Webapi.dll"]
