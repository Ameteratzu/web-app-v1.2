#!/bin/bash
set -e

# Contador de ejecuciones
EXECUTION_COUNT_FILE="/app/execution_count.txt"

# Inicializar o incrementar el contador de ejecuciones
if [ ! -f "$EXECUTION_COUNT_FILE" ]; then
    echo "1" > "$EXECUTION_COUNT_FILE"
else
    current_count=$(cat "$EXECUTION_COUNT_FILE")
    new_count=$((current_count + 1))
    echo "$new_count" > "$EXECUTION_COUNT_FILE"
fi

# Mostrar el número de ejecuciones
echo "Número de ejecuciones del script: $(cat "$EXECUTION_COUNT_FILE")"

# Configuración de variables
DB_SERVER="sqlserver.ns-sigemad.svc.cluster.local,1433"
DB_USER="sa"
DB_PASSWORD='P@s$w0rd'
DLL_FOLDER="/app/DLL"
DATOS_FOLDER="/app/Datos"
DB_NAME="Sigemad"

echo "Eliminando base de datos $DB_NAME"
/opt/mssql-tools18/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$DB_PASSWORD" -Q "IF EXISTS (SELECT name FROM sys.databases WHERE name = '$DB_NAME') BEGIN ALTER DATABASE [$DB_NAME] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$DB_NAME]; END" -C

echo "Creando base de datos $DB_NAME"
/opt/mssql-tools18/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$DB_PASSWORD" -Q "CREATE DATABASE [$DB_NAME]" -C

# Función para ejecutar scripts en una carpeta
execute_scripts_in_folder() {
    local folder=$1
    for file in "$folder"/*.sql; do
        if [ -f "$file" ]; then
            echo "Ejecutando $file"
            /opt/mssql-tools18/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$DB_PASSWORD" -d "$DB_NAME" -i "$file" -C
        fi
    done
}

# Ejecutar scripts en las carpetas DLL y Datos
execute_scripts_in_folder "$DLL_FOLDER"
execute_scripts_in_folder "$DATOS_FOLDER"

echo "Proceso de inicialización de base de datos completado"