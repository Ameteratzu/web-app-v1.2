#!/bin/bash

# Configuración de variables
DB_SERVER=sqlserver.ns-sigemad.svc.cluster.local,1433
DB_NAME=Sigemad
DB_USER=sa
DB_PASSWORD='P@s$w0rd'
DLL_FOLDER=/app/DLL
DATOS_FOLDER=/app/Datos

echo "DB_SERVER: $DB_SERVER"
echo "DB_NAME: $DB_NAME"

# Función para ejecutar scripts en una carpeta
execute_scripts_in_folder() {
    local folder=$1
    echo "Ejecutando scripts en la carpeta: $folder"
    for file in $folder/*.sql; do
        echo "Ejecutando $file..."
        /opt/mssql-tools18/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$file" -C
        if [ $? -ne 0 ]; then
            echo "Error al ejecutar $file"
            exit 1
        fi
    done
}

# Probar conexión a la base de datos
echo "Verificando conexión a la base de datos..."
/opt/mssql-tools18/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -Q "SELECT 1" -C
if [ $? -ne 0 ]; then
    echo "No se puede conectar a la base de datos $DB_NAME en el servidor $DB_SERVER"
    exit 1
else
    echo "Conexión a la base de datos verificada correctamente."
fi

# Verificar si la base de datos ya está inicializada
echo "Comprobando si la base de datos ya está inicializada..."
TABLE_CHECK=$(/opt/mssql-tools18/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -Q "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Alertas') SELECT 1 ELSE SELECT 0" -h -1 | tr -d ' \r')

if [ "$TABLE_CHECK" == "1" ]; then
    echo "La base de datos ya está inicializada. No se ejecutarán los scripts."
else
    echo "La base de datos no está inicializada. Ejecutando scripts..."

    # Ejecutar scripts en la carpeta DLL
    execute_scripts_in_folder $DLL_FOLDER

    # Ejecutar scripts en la carpeta Datos
    execute_scripts_in_folder $DATOS_FOLDER

    echo "Todos los scripts se ejecutaron correctamente."
fi

echo "Proceso completado con éxito."
