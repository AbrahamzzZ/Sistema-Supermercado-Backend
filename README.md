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

## Integración con IA (Ollama)

El sistema incluye integración con IA local usando **Ollama**.

### Requisitos:

1. Instalar Ollama desde:
https://ollama.com

2. Descargar el modelo usado en el proyecto o cualquier otro modelo:

ollama pull qwen3:8b

**Editar OllamaClient.cs**

cd Backend\Utilities\IA

string model = "TU_MODELO"

---

## Instalación y ejecución

**Clonar repositorio**
git clone <https://github.com/AbrahamzzZ/Sistema-Supermercado-Backend.git>

**Entrar a la carpeta del backend**
cd backend

**Editar appsettings.json**

  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BASE;User Id=USUARIO;Password=CLAVE;Integrated Security=True;TrustServerCertificate=True;"
  }

**Restaurar dependencias**
dotnet restore

**Ejecutar**
dotnet run