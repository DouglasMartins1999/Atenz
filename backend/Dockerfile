FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY ./build/ .

CMD cp /settings/storageSettings.json . && ls && dotnet Atenz.API.dll