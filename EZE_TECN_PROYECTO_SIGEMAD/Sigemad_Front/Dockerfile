# Etapa 1: Construcción de la aplicación Angular
FROM node:18 AS build
WORKDIR /app

# Actualizar npm a la última versión
RUN npm install -g npm@latest

# Copiar únicamente package.json
COPY package.json ./

# Limpiar caché y eliminar node_modules (por si acaso)
RUN npm cache clean --force

# Instalar dependencias sin dependencias opcionales
RUN npm install --no-optional

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
