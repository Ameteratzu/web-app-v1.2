# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos de solución y de proyecto
COPY *.sln ./
COPY DGPCE.Sigemad.API/*.csproj ./DGPCE.Sigemad.API/
COPY DGPCE.Sigemad.Application/*.csproj ./DGPCE.Sigemad.Application/
COPY DGPCE.Sigemad.Domain/*.csproj ./DGPCE.Sigemad.Domain/
COPY DGPCE.Sigemad.Identity/*.csproj ./DGPCE.Sigemad.Identity/
COPY DGPCE.Sigemad.Infrastructure/*.csproj ./DGPCE.Sigemad.Infrastructure/
COPY tests/DGPCE.Sigemad.Application.Tests/*.csproj ./tests/DGPCE.Sigemad.Application.Tests/

# Restaurar las dependencias
RUN dotnet restore

# Copiar el resto del código fuente
COPY . .

# Publicar la aplicación en modo Release
RUN dotnet publish DGPCE.Sigemad.API/DGPCE.Sigemad.API.csproj -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Instalar sqlcmd y sus dependencias
RUN apt-get update && \
    apt-get install -y curl apt-transport-https gnupg2 && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql18 mssql-tools18 unixodbc-dev && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Añadir sqlcmd al PATH
ENV PATH="$PATH:/opt/mssql-tools18/bin"

# Copiar los archivos publicados desde la etapa de construcción
COPY --from=build /app/out ./

# Copiar los scripts
COPY Execute_scripts.sh /app/Execute_scripts.sh
COPY ../BasesDatos/DLL/ /app/DLL/
COPY ../BasesDatos/Datos/ /app/Datos/

# Dar permisos de ejecución al script
RUN chmod +x /app/Execute_scripts.sh

# Exponer el puerto utilizado por la API
EXPOSE 8080

# Establecer variables de entorno para producción
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Modificar el comando de inicio para ejecutar el script antes de iniciar la API
CMD /app/Execute_scripts.sh && dotnet DGPCE.Sigemad.API.dll
