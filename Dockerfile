FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY src/ .

WORKDIR /app/Backend/MyRecipeBook.API/

RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app    

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "MyRecipeBook.API.dll"]