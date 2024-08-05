FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy AS build

WORKDIR /app
COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-jammy AS runtime
WORKDIR /app

COPY --from=build /app/out .
ENV ASPNETCORE_ENVIRONMENT=Production

RUN mkdir /var/log/streamer
RUN mkdir /media/streamer/

EXPOSE 80

ENTRYPOINT [ "dotnet", "StreamerApi.dll" ]