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

## CI/CD Pipeline con Jenkins

El proyecto cuenta con un pipeline de Integración Continua y Despliegue Continuo configurado en **Jenkins** que automatiza el proceso de compilación, prueba y despliegue.

### Importante

**El pipeline SOLO funciona en la rama `5-test`.** Esta rama está preparada exclusivamente para la ejecución de pruebas unitarias y despliegue automatizado.

### Flujo del Pipeline

Cada vez que se realiza un `git push` a la rama `5-test`, Jenkins ejecuta automáticamente las siguientes etapas:

| Etapa | Descripción |
|-------|-------------|
| **Build** | Restaura paquetes NuGet y compila la solución |
| **Test** | Ejecuta las 233 pruebas unitarias usando MSTest |
| **Publish** | Genera los archivos ejecutables de la API |
| **Deploy** | Copia los archivos a `C:\SistemaVentas\API` |

### Notificaciones

Al finalizar el pipeline, se envía un correo electrónico al equipo de desarrollo indicando:
- ✅ **Éxito**: Todas las pruebas pasaron correctamente
- ❌ **Fallo**: Se requiere revisar los logs del pipeline

### Archivos de configuración

- `Jenkinsfile` → Define las etapas del pipeline
- `deploy.ps1` → Script de despliegue en entorno local

### Requisitos para ejecutar el pipeline

- Jenkins instalado (v2.541.3+)
- Java 21 (LTS)
- .NET SDK 8.0
- ngrok (para webhook local)

### Rama de pruebas

```bash
# Cambiar a la rama de pruebas
git checkout 5-test

# Subir cambios para activar el pipeline
git push origin 5-test