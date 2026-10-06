# Sistema de Ventas - Backend API (.NET 8)

API REST desarrollada en **.NET 8** para la gestión integral de un Sistema de Ventas.  
Permite administrar usuarios, ventas, compras, inventario, clientes, proveedores y reportes.

---

## Tecnologías utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (Database First)
- SQL Server
- JWT Authentication
- SHA-256 (Cifrado de contraseñas)
- Auditoría de operaciones por módulo
- Logging persistente con niveles `INFO`, `WARNING` y `ERROR`
- Ollama (Integración IA local)

---

## Arquitectura

La API está organizada en una arquitectura en capas:

### Controllers
- Reciben solicitudes HTTP.
- Validan parámetros.
- Retornan respuestas estándar (`ApiResponse`).

### Services
- Contienen la lógica de negocio.
- Usan inyección de dependencias.
- Separación por módulos (Ventas, Compras, Usuarios, etc.).

### Repository
- Acceso a datos mediante Entity Framework.
- Implementación de interfaces por módulo.

### Models
- Clases generadas por EF (Database First).
- DTOs para transferencia de datos.

### Utilities / Shared
- `ApiResponse.cs` → Respuesta estándar de la API.
- `Mensajes.cs` → Mensajes reutilizables.
- `Paginacion.cs` → Soporte para paginación.
- `Encriptacion.cs` → Cifrado SHA-256.
- `Token.cs` → Generación y validación JWT.

---

## Seguridad

- Autenticación basada en **JWT**
- Contraseñas cifradas con **SHA-256**
- Protección de endpoints mediante `[Authorize]`

---

## Auditoría y logging

La API registra las operaciones de los módulos mediante `IAuditoriaService`. La auditoría está integrada en los servicios de negocio y permite conservar la trazabilidad de las operaciones exitosas, las validaciones fallidas y los errores inesperados.

Cada registro almacena:

- Código de seguimiento de la operación.
- Mensaje y detalle de lo ocurrido.
- Usuario autenticado que ejecutó la operación, cuando está disponible.
- Endpoint y método HTTP utilizados.
- Fecha y hora del evento.
- Nivel del registro.

### Niveles de auditoría

| Nivel | Uso |
|-------|-----|
| `INFO` | Operaciones ejecutadas correctamente. |
| `WARNING` | Validaciones fallidas, datos no encontrados o reglas de negocio no cumplidas. |
| `ERROR` | Excepciones y errores inesperados del sistema. |

Los errores no controlados se registran automáticamente mediante `ErrorHandlerMiddleware` antes de devolver la respuesta HTTP `500` al cliente. Los registros se persisten en la tabla `LOG` de SQL Server a través del procedimiento `PA_REGISTRAR_LOG`.

---

## Integración con IA (Ollama)

El sistema incluye integración con IA local usando **Ollama**.

### Requisitos:

1. Instalar Ollama desde:
https://ollama.com

2. Asegurarse de que Ollama esté iniciado y descargar el modelo (por defecto `phi3.5`):

```bash
ollama pull phi3.5
```

En Windows, Ollama normalmente queda ejecutándose en segundo plano después de abrir la aplicación. Si no está iniciado, ejecutar `ollama serve` en otra terminal y dejarla abierta mientras se usa la API.

3. Si usa otro modelo, cambie solo la configuración (no el código):
   - En local: `Ollama:Model` en `appsettings.Development.json`.
   - En Docker: `OLLAMA_MODEL` en el archivo `.env`.

---

## Configuración

La API lee siempre las mismas claves; solo cambia de dónde salen los valores:

| Clave | Local (`dotnet run`) | Docker (`docker compose up`) |
|-------|----------------------|------------------------------|
| `ConnectionStrings:CadenaSQL` | `appsettings.Development.json` | `DB_CONNECTION_STRING` en `.env` |
| `Jwt:Key` | User Secrets | `JWT_KEY` en `.env` |
| `Ollama:BaseUrl` | `http://localhost:11434` | `http://ollama:11434` (fijo en `docker-compose.yml`) |
| `Ollama:Model` | `appsettings.Development.json` | `OLLAMA_MODEL` en `.env` |

`appsettings.json` contiene solo valores por defecto y no lleva secretos. Si falta la cadena de conexión o la clave JWT, la API no arranca y el mensaje indica dónde configurarla.

---

## Instalación y ejecución

**Clonar repositorio**
git clone <https://github.com/AbrahamzzZ/Sistema-Supermercado-Backend.git>

**Ejecutar el script de la DB**
Ejecutar `Db/DB_Sistema_Supermercado.sql` en SQL Server.

### Opción 1: local

Los comandos siguientes se ejecutan desde la raíz del repositorio. Se necesita tener instalado el SDK de .NET 8, SQL Server con la base de datos creada y Ollama iniciado con el modelo configurado.

1. Revisar `APIRestSistemaVentas/appsettings.Development.json` y ajustar `ConnectionStrings:CadenaSQL` para que apunte a la instancia de SQL Server donde se ejecutó el script de la base de datos.
2. Guardar una clave JWT propia de al menos 32 caracteres en User Secrets. Se hace una vez por usuario/proyecto; reemplazar el texto de ejemplo por una clave aleatoria:

```powershell
dotnet user-secrets set "Jwt:Key" "<clave-aleatoria-de-al-menos-32-caracteres>" --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj
```

El proyecto ya tiene configurado su `UserSecretsId`; ese identificador permite guardar y recuperar secretos, pero no crea la clave automáticamente. Para comprobar si quedó guardada, se puede ejecutar:

```powershell
dotnet user-secrets list --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj
```

Este comando muestra los valores de los secretos; no se debe compartir su salida.

3. Restaurar dependencias e iniciar la API con el perfil local `http`:

```powershell
dotnet restore .\APIRestSistemaVentas\APIRestSistemaVentas.csproj
dotnet run --project .\APIRestSistemaVentas\APIRestSistemaVentas.csproj --launch-profile http
```

La API local queda disponible en `http://localhost:5299/swagger`. Para detenerla, presionar `Ctrl+C` en la terminal.

### Opción 2: Docker

1. Copiar `.env.example` como `.env` y completar `DB_CONNECTION_STRING`, `JWT_KEY` y `OLLAMA_MODEL`. La base de datos debe ser accesible desde el contenedor (IP o nombre del servidor, no `localhost`).
2. Levantar la API y Ollama:

```bash
docker compose up -d --build
```

La API queda en `http://localhost:8081/swagger` y el contenedor de Ollama descarga el modelo indicado en `OLLAMA_MODEL`.

---

## Pruebas unitarias y GitHub Actions

El proyecto cuenta con pruebas unitarias desarrolladas con **MSTest** y **Moq**. Las pruebas están organizadas por responsabilidad:

- `UnitTests/Services`: valida reglas de negocio, validaciones, casos exitosos, errores, excepciones y registro de auditoría.
- `UnitTests/Controller`: valida las respuestas HTTP de los controladores y la comunicación con los servicios mockeados.

Actualmente existen **244 pruebas unitarias**.

### Flujo de integración continua

El workflow `.github/workflows/pruebas.yml` se ejecuta automáticamente cuando:

- Se crea o actualiza un Pull Request hacia `main`.
- Se realiza un push directo a `main`.

El workflow realiza las siguientes etapas:

| Etapa | Descripción |
|-------|-------------|
| **Restauración** | Restaura las dependencias de la solución. |
| **Compilación** | Compila `Backend.sln` en configuración `Release`. |
| **Pruebas** | Ejecuta todos los tests del proyecto `UnitTests`. |

Si la compilación o alguna prueba falla, GitHub Actions marca el workflow como fallido y muestra los detalles en los logs del Pull Request.

### Protección de la rama principal

La rama `main` debe utilizar una regla de protección que requiera:

- Un Pull Request para integrar cambios.
- La aprobación de los checks de GitHub Actions antes del merge.
- La resolución de las conversaciones del Pull Request.

De esta forma, los cambios no se integran a `main` mientras la solución no compile o alguna prueba unitaria falle.

### Ejecución local

Para restaurar, compilar y ejecutar las pruebas localmente:

```bash
dotnet restore Backend.sln
dotnet build Backend.sln --configuration Release --no-restore
dotnet test UnitTests/UnitTests.csproj --configuration Release --no-build
```