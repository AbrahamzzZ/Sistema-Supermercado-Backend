# deploy.ps1 - Versión simple
Write-Host "Desplegando API..."

$origen = "ApiRestSistemaVentas\publish"
$destino = "C:\SistemaVentas\API"

# Crear carpeta si no existe
New-Item -ItemType Directory -Path $destino -Force | Out-Null

# Copiar archivos
Copy-Item -Path "$origen\*" -Destination $destino -Recurse -Force

Write-Host "Despliegue completado en: $destino"