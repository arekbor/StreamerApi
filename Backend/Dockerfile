# Dotnet build image
FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy AS build

WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Dotnet runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-jammy AS runtime
WORKDIR /app

ARG USERNAME=streamer_api_server

# Create a new user and group
RUN useradd -m ${USERNAME}

# Set the owner and group to the newly created user for the directories to be used
RUN mkdir /var/log/streamer && chown -R ${USERNAME}:${USERNAME} /var/log/streamer
RUN mkdir /media/streamer/ && chown -R ${USERNAME}:${USERNAME} /media/streamer/
RUN chown -R ${USERNAME}:${USERNAME} /app

# Copying application files from the build stage
COPY --from=build --chown=${USERNAME}:${USERNAME} /app/out .

# Setting the environment variable
ENV ASPNETCORE_ENVIRONMENT=Production

# Exposing the port
EXPOSE 80

# Switching to a new user
USER ${USERNAME}

# Setting the entry point for the application
ENTRYPOINT [ "dotnet", "StreamerApi.dll" ]