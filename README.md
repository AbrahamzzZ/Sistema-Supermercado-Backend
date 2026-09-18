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

2. Descargar el modelo usado en el proyecto o cualquier otro modelo:

ollama pull qwen3:8b

3. Editar OllamaClient.cs

cd Backend\Utilities\IA

string model = "TU_MODELO"

---

## Instalación y ejecución

**Clonar repositorio**
git clone <https://github.com/AbrahamzzZ/Sistema-Supermercado-Backend.git>

**Entrar a la carpeta del backend**
cd backend

**Ejecutar el script de la DB**
cd backend/Db

Importante si va a usar la autenticacion de Windows en vez de un usuario de la base de datos especificar eso en la cadena de conexión.

**Editar appsettings.json**

  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BASE;User Id=USUARIO;Password=CLAVE;Integrated Security=True;TrustServerCertificate=True;"
  }

**Restaurar dependencias**
dotnet restore

**Ejecutar**
dotnet run

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