# Etapa 1: Construcción de la aplicación Angular
FROM node:18.18.0 AS build
WORKDIR /app

# Instalar npm@10.1.0
RUN npm install -g npm@10.1.0

# Establecer variable de entorno para incluir dependencias opcionales
ENV NPM_OPTIONAL=1

# Copiar package.json y package-lock.json
COPY package.json package-lock.json ./

# Limpiar caché de npm
RUN npm cache clean --force

# Instalar dependencias usando npm install
RUN npm install

# Copiar el resto del código fuente al contenedor
COPY ./ ./

# Ejecutar el build en modo producción
RUN npm run build --prod

# Etapa 2: Servir la aplicación con Nginx
FROM nginx:latest
WORKDIR /usr/share/nginx/html

# Eliminar archivos predeterminados de Nginx
RUN rm -rf ./*

# Copiar los archivos compilados desde la etapa de construcción
COPY --from=build /app/dist/sigemad/browser/ .

# Copiar configuración personalizada para Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Exponer el puerto necesario para Kubernetes
EXPOSE 80

# Comando por defecto para iniciar Nginx
CMD ["nginx", "-g", "daemon off;"]
