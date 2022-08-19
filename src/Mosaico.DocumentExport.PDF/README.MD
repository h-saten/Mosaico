# Generator usage instructions

Thirdth and seventh line from code below are required in Dockerfile to be able to use pdf generator properly.
```
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /ui
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
EXPOSE 8080
COPY --from=buildnet /ui/Mosaico.WebUI/out .
COPY --from=buildnode /dist ./ClientApp/
RUN chmod 755 ./Tools/Linux/wkhtmltopdf
ENTRYPOINT ["dotnet", "Mosaico.WebUI.dll"]
```

#### Important!
Find out valid wkhtmltopdf file when package is compiled in core service.
