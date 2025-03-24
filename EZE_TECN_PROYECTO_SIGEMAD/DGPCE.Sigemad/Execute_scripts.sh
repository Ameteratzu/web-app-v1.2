#!/bin/bash
set -e

# Configuración de variables
DB_SERVER="sqlserver.ns-sigemad.svc.cluster.local,1433"
DB_USER="sa"
DB_PASSWORD='P@s$w0rd'
DLL_FOLDER="/app/DLL"
DATOS_FOLDER="/app/Datos"
DB_NAME="Sigemad"

echo "DB_SERVER: $DB_SERVER"
echo "Base de datos a actualizar: $DB_NAME"

# Esperar a que el servidor de base de datos esté disponible
max_retries=1
attempt=1
until /opt/mssql-tools18/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$DB_PASSWORD" -Q "SELECT 1" -C; do
    if [ $attempt -ge $max_retries ]; then
        echo "No se pudo conectar al servidor de base de datos después de $max_retries intentos."
        exit 1
    fi
    echo "Intento $attempt: Servidor no disponible, esperando 5 segundos..."
    sleep 5
    attempt=$((attempt+1))
done
echo "Conexión al servidor de base de datos verificada correctamente."

# Eliminar la base de datos si existe (forzando cierre de conexiones)
echo "Eliminando la base de datos $DB_NAME si existe..."
/opt/mssql-tools18/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$DB_PASSWORD" -Q "IF EXISTS (SELECT name FROM sys.databases WHERE name = '$DB_NAME') BEGIN ALTER DATABASE [$DB_NAME] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$DB_NAME]; END" -C
echo "Base de datos eliminada (si existía)."

# Crear la base de datos
echo "Creando la base de datos $DB_NAME..."
/opt/mssql-tools18/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$DB_PASSWORD" -Q "CREATE DATABASE [$DB_NAME]" -C
echo "Base de datos creada."

# Función para ejecutar scripts en una carpeta para la base de datos específica
execute_scripts_in_folder() {
    local folder=$1
    echo "Ejecutando scripts en la carpeta: $folder para la base de datos: $DB_NAME"
    for file in "$folder"/*.sql; do
        if [ -f "$file" ]; then
            echo "Ejecutando $file en la base de datos $DB_NAME..."
            /opt/mssql-tools18/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$DB_PASSWORD" -d "$DB_NAME" -i "$file" -C
            echo "Script $file ejecutado correctamente."
        else
            echo "No se encontraron archivos SQL en $folder."
        fi
    done
}

# Ejecutar scripts en la carpeta DLL para la base de datos
execute_scripts_in_folder "$DLL_FOLDER"

# Ejecutar scripts en la carpeta Datos para la base de datos
execute_scripts_in_folder "$DATOS_FOLDER"

echo "Todos los scripts se ejecutaron correctamente para la base de datos $DB_NAME."
echo "Proceso completado con éxito."
