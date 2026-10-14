# Sistema de Ventas - Backend API

API REST para la gestión de ventas, compras, inventario, usuarios y reportes. Está desarrollada con ASP.NET Core .NET 8 y SQL Server.

## Documentación

- [Manual técnico](./Docs/Manual%20Tecnico.docx): arquitectura, módulos, base de datos, configuración y operación.
- [Especificación de requerimientos](./Docs/Especificacion%20Requerimientos.docx): requerimientos funcionales, reglas de negocio y requisitos no funcionales.
- [Colección Postman](./Sistema%20Ventas.postman_collection.json): solicitudes de ejemplo para probar los endpoints.

Al ejecutar la API en entorno Development, Swagger está disponible en `/swagger`.

## Requisitos

- .NET 8 SDK.
- SQL Server.
- Ollama, solo para las funciones de análisis con IA. En Docker, Ollama se levanta como servicio del `docker-compose`.

## Inicio rápido local

Desde la raíz del repositorio:

1. Crear la base de datos ejecutando [`Db/DB_Sistema_Supermercado.sql`](./Db/DB_Sistema_Supermercado.sql) en SQL Server.
2. Configurar `ConnectionStrings:CadenaSQL` en `APIRestSistemaVentas/appsettings.Development.json` para la instancia SQL Server local.
3. Guardar una clave JWT propia de al menos 32 caracteres en User Secrets:

```powershell
dotnet user-secrets set "Jwt:Key" "<clave-aleatoria-de-al-menos-32-caracteres>" --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj
```

4. Para las funciones con IA, iniciar Ollama y descargar el modelo configurado (por defecto `phi3.5`):

```powershell
ollama pull phi3.5
```

5. Restaurar e iniciar la API:

```powershell
dotnet restore .\APIRestSistemaVentas\APIRestSistemaVentas.csproj
dotnet run --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj --launch-profile http
```

Swagger local: <http://localhost:5299/swagger>. Para detener la API, presionar `Ctrl+C`.

## Inicio rápido con Docker

Copiar `.env.example` como `.env` en la raíz, completar la cadena de conexión y la clave JWT; luego ejecutar:

```powershell
docker compose up -d --build
```

La base de datos debe ser accesible desde los contenedores. No usar `localhost` en la cadena SQL para una instancia que está fuera del contenedor. Swagger: <http://localhost:8081/swagger>.

## Pruebas

Para ejecutar la compilación y las pruebas unitarias:

```powershell
dotnet restore .\Backend.sln
dotnet build .\Backend.sln --configuration Release --no-restore
dotnet test .\UnitTests\UnitTests.csproj --configuration Release --no-build
```

GitHub Actions también compila la solución y ejecuta las pruebas en los cambios dirigidos a `main`; el workflow está en `.github/workflows/pruebas.yml`.