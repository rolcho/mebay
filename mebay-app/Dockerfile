FROM node:18-alpine AS build

# Install Python
RUN apk add --no-cache python3 make g++

WORKDIR /usr/local/app
COPY ./ /usr/local/app

RUN npm install
RUN npm run build

FROM nginx:alpine AS ngi
COPY --from=build /usr/local/app/www /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
