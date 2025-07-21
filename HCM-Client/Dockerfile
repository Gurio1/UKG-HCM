FROM node:latest AS build

WORKDIR /app

COPY package.json /app/
COPY package-lock.json /app/

RUN npm install
COPY . /app/

RUN npm run build --prod

# Step 2: Serve the Angular app
FROM nginx:latest
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/hcm-client/browser /usr/share/nginx/html
EXPOSE 80