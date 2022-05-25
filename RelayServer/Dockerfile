FROM node:16

WORKDIR /app

COPY RelayServer/package.json package.json
COPY RelayServer/package-lock.json package-lock.json

RUN npm install

COPY . .

CMD ["node", "RelayServer/server.js"]