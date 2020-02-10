# Base image for build
FROM node:13-alpine AS build

# Working directory
WORKDIR /app

# Coping configuration files and seting up cache for packages
COPY package.json .
#COPY .npmrc .
COPY yarn.lock .
#RUN npm set progress=false && npm config set depth 0 && npm cache clean --force

# Updating and installing dependencies, coping cache modules 
#RUN npm install -g @angular/cli@latest
#RUN npm install -g npm-check-updates 
#RUN ncu -u
#RUN npm i 
RUN yarn instakll -g @angular/cli@latest
RUN yarn install

COPY . .
# Build the angular app in production mode and store the artifacts in dist folder
RUN yarn build --prod

EXPOSE 8080
CMD [ "yarn", "start" ]
