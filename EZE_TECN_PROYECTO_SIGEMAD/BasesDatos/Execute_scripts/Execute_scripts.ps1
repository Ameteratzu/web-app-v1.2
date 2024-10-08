# Función para leer archivos .ini
function Get-IniContent {
    param(
        [string]$iniPath
    )

    $iniContent = @{}
    $currentSection = ""
    
    foreach ($line in Get-Content $iniPath) {
        # Ignorar comentarios y líneas vacías
        if ($line -match "^\s*;") { continue }
        if ($line -match "^\s*$") { continue }
        
        # Detectar nuevas secciones
        if ($line -match "^\[(.+)\]$") {
            $currentSection = $matches[1]
            $iniContent[$currentSection] = @{}
        } elseif ($line -match "^\s*(.+?)\s*=\s*(.+?)\s*$") {
            # Añadir claves y valores a la sección actual
            $key = $matches[1]
            $value = $matches[2]
            $iniContent[$currentSection][$key] = $value
        }
    }

    return $iniContent
}

# Función para ejecutar los scripts en una carpeta
function Execute-Scripts {
    param(
        [string]$folder,
        [string]$dbServer,
        [string]$dbName
    )

    # Obtener todos los archivos .sql en la carpeta y ordenarlos por nombre
    $files = Get-ChildItem -Path $folder -Filter *.sql | Sort-Object Name

    # Iterar sobre cada archivo SQL y ejecutarlo
    foreach ($file in $files) {
        $scriptPath = $file.FullName
        Write-Host "Ejecutando script: $scriptPath"

        # Ejecutar el script usando sqlcmd con codificación UTF-8
        $sqlcmd = "sqlcmd -S $dbServer -d $dbName -i `"$scriptPath`" -f 65001 -E"
        Write-Host "Comando: $sqlcmd"  # Para depuración
        $result = Invoke-Expression $sqlcmd

        # Verificar si hubo un error
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Error ejecutando el script: $scriptPath" -ForegroundColor Red
            return $false  # Salir si hay un error
        }
    }

    return $true  # Todos los scripts se ejecutaron correctamente
}

# Ruta al archivo de configuración
$configFile = "config.ini"

# Leer contenido del archivo ini
$config = Get-IniContent $configFile

# Parámetros de conexión obtenidos desde el archivo de configuración
$DB_SERVER = $config['DatabaseSettings']['DB_SERVER']
$DB_NAME = $config['DatabaseSettings']['DB_NAME']
$DLL_FOLDER = $config['DatabaseSettings']['DLL_FOLDER']  # Ruta a los scripts DLL
$DATOS_FOLDER = $config['DatabaseSettings']['DATOS_FOLDER']  # Ruta a los scripts de Datos

# Ejecutar scripts en la carpeta DLL
Write-Host "Ejecutando scripts en la carpeta DLL..."
$dllExecutionResult = Execute-Scripts -folder $DLL_FOLDER -dbServer $DB_SERVER -dbName $DB_NAME

# Verificar si los scripts de DLL se ejecutaron correctamente
if ($dllExecutionResult) {
    Write-Host "Scripts de DLL ejecutados correctamente. Ejecutando scripts en la carpeta Datos..."
    
    # Ejecutar scripts en la carpeta Datos
    $datosExecutionResult = Execute-Scripts -folder $DATOS_FOLDER -dbServer $DB_SERVER -dbName $DB_NAME

    if ($datosExecutionResult) {
        Write-Host "Todos los scripts de Datos se ejecutaron correctamente."
    } else {
        Write-Host "Error al ejecutar los scripts en la carpeta Datos." -ForegroundColor Red
        exit 1  # Salir si hay un error en los scripts de Datos
    }
} else {
    Write-Host "Error al ejecutar los scripts en la carpeta DLL. No se ejecutarán los scripts de Datos." -ForegroundColor Red
    exit 1  # Salir si hay un error en los scripts de DLL
}

Write-Host "Proceso completado con éxito."
