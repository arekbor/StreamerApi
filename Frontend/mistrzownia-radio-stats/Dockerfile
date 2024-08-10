FROM node:20.14.0-alpine3.20 AS build
WORKDIR /usr/local/app
RUN npm cache clean --force
COPY . .
RUN npm install
RUN npm run prod

FROM nginx:latest AS ngi

ARG USERNAME=streamer_stats_client

RUN useradd -m $USERNAME

RUN chown -R $USERNAME:$USERNAME /usr/share/nginx/html && \
    chmod -R 755 /usr/share/nginx/html && \
    chown -R $USERNAME:$USERNAME /var/cache/nginx && \
    chown -R $USERNAME:$USERNAME /var/log/nginx && \
    chown -R $USERNAME:$USERNAME /etc/nginx/conf.d

RUN touch /var/run/nginx.pid && \
    chown -R $USERNAME:$USERNAME /var/run/nginx.pid

RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /usr/local/app/dist/mistrzownia-radio-stats/browser /usr/share/nginx/html

WORKDIR /etc/nginx/template
COPY default.conf.template /etc/nginx/templates/default.conf.template

USER $USERNAME

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
