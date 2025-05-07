FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
EXPOSE 8080

WORKDIR /app

COPY . .

WORKDIR /app/Frontend/MyRecipeBook.Site/

RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

FROM nginx:stable-alpine AS runtime
COPY --from=build-env /app/out/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

ENTRYPOINT ["nginx", "-g", "daemon off;"]