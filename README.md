# Sistema de Ventas - Backend

API REST para las operaciones del Sistema de Ventas, desarrollada con ASP.NET Core .NET 8 y SQL Server.

## Documentación

- [Manual técnico](./Docs/Manual%20Tecnico.docx): arquitectura, configuración y operación.
- [Especificación de requerimientos](./Docs/Especificacion%20Requerimientos.docx).
- [Colección Postman](./Sistema%20Ventas.postman_collection.json): ejemplos para probar los endpoints.

## Requisitos

- .NET 8 SDK y SQL Server.
- Ollama, opcional, para las funciones de análisis con IA.

## Inicio rápido local

Desde la raíz del repositorio:

1. Crear la base de datos ejecutando [`Db/DB_Sistema_Supermercado.sql`](./Db/DB_Sistema_Supermercado.sql) y configurar `ConnectionStrings:CadenaSQL` en `APIRestSistemaVentas/appsettings.Development.json`.
2. Este proyecto ya tiene configurado `UserSecretsId` en el archivo `.csproj`, así que normalmente no es necesario inicializar User Secrets. Si trabajas con una copia donde no esté configurado, inicialízalo una sola vez:

   ```powershell
   dotnet user-secrets init --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj
   ```

3. Guardar una clave JWT propia de al menos 32 caracteres:

   ```powershell
   dotnet user-secrets set "Jwt:Key" "<clave-aleatoria-de-al-menos-32-caracteres>" --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj
   ```

4. Restaurar e iniciar la API:

   ```powershell
   dotnet run --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj --launch-profile http
   ```

Swagger: <http://localhost:5299/swagger>. Para habilitar el asistente de IA, inicia Ollama y descarga el modelo configurado (`ollama pull phi3.5`).

## Docker

Completar `.env` a partir de `.env.example` y ejecutar desde la raíz:

```powershell
docker compose up -d --build
```

Swagger: <http://localhost:8081/swagger>. La cadena de conexión debe apuntar a una instancia SQL Server accesible desde los contenedores.
