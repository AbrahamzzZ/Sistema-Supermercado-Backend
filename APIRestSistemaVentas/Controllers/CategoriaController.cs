using Domain.Models;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Utilities.Shared;

namespace APIRestSistemaVentas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }*/

        // GET: api/categoria
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar categorías",
            Description = "Obtiene la lista completa de categorías registradas en el sistema."
        )]
        [SwaggerResponse(200, "Lista de categorías obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Categorium>>> GetCategorias()
        {
            var categorias = await _categoriaService.ListarCategoriasAsync();
            return Ok(categorias);
        }

        // GET: api/categoria/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar categorías con paginación",
            Description = "Obtiene una lista de categorías utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<Categorium>>>> GetCategoriasPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _categoriaService.ListarCategoriasPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/categoria/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener categoría por ID",
            Description = "Obtiene la información de una categoría específica mediante su identificador."
        )]
        [SwaggerResponse(200, "Categoría encontrada")]
        [SwaggerResponse(404, "Categoría no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Categorium>> GetCategoria(int id)
        {
            var response = await _categoriaService.ObtenerCategoriaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/categoria
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar categoría",
            Description = "Registra una nueva categoría en el sistema."
        )]
        [SwaggerResponse(200, "Categoría registrada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarCategoria([FromBody] Categorium categoria)
        {
            var response = await _categoriaService.RegistrarCategoriaAsync(categoria);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/categoria/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar categoría",
            Description = "Actualiza la información de una categoría existente."
        )]
        [SwaggerResponse(200, "Categoría actualizada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarCategoria(int id, [FromBody] Categorium categoria)
        {
            var response = await _categoriaService.EditarCategoriaAsync(categoria);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/categoria/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar categoría",
            Description = "Elimina una categoría del sistema mediante su identificador."
        )]
        [SwaggerResponse(200, "Categoría eliminada correctamente")]
        [SwaggerResponse(404, "Categoría no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarCategoria(int id)
        {
            var response = await _categoriaService.EliminarCategoriaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
