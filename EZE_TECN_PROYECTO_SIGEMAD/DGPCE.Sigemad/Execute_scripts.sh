#!/bin/bash

# Configuración de variables
DB_SERVER=sqlserver.ns-sigemad.svc.cluster.local,1433
DB_USER=sa
DB_PASSWORD='P@s$w0rd'
DLL_FOLDER=/app/DLL
DATOS_FOLDER=/app/Datos
DB_NAME="Sigemad"

echo "DB_SERVER: $DB_SERVER"
echo "Base de datos a actualizar: $DB_NAME"

# Función para ejecutar scripts en una carpeta para la base de datos específica
execute_scripts_in_folder() {
    local folder=$1
    echo "Ejecutando scripts en la carpeta: $folder para la base de datos: $DB_NAME"
    for file in $folder/*.sql; do
        echo "Ejecutando $file en la base de datos $DB_NAME..."
        /opt/mssql-tools18/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$file" -C
        if [ $? -ne 0 ]; then
            echo "Error al ejecutar $file en la base de datos $DB_NAME"
            exit 1
        fi
    done
}

# Probar conexión al servidor de base de datos
echo "Verificando conexión al servidor de base de datos..."
/opt/mssql-tools18/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -Q "SELECT 1" -C
if [ $? -ne 0 ]; then
    echo "No se puede conectar al servidor de base de datos en $DB_SERVER"
    exit 1
else
    echo "Conexión al servidor de base de datos verificada correctamente."
fi

# Verificar si la base de datos existe
DB_CHECK=$(/opt/mssql-tools18/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -Q "SELECT name FROM sys.databases WHERE name = '$DB_NAME'" -h -1 | tr -d ' \r')

if [ -z "$DB_CHECK" ]; then
    echo "La base de datos $DB_NAME no existe. Creándola..."
    /opt/mssql-tools18/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -Q "CREATE DATABASE [$DB_NAME]" -C
    if [ $? -ne 0 ]; then
        echo "Error al crear la base de datos $DB_NAME"
        exit 1
    fi
else
    echo "La base de datos $DB_NAME ya existe."
fi

# Ejecutar scripts en la carpeta DLL para la base de datos
execute_scripts_in_folder $DLL_FOLDER

# Ejecutar scripts en la carpeta Datos para la base de datos
execute_scripts_in_folder $DATOS_FOLDER

echo "Todos los scripts se ejecutaron correctamente para la base de datos $DB_NAME."

echo "Proceso completado con éxito."
