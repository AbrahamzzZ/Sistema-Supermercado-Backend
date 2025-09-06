using DataBaseFirst.Models;
using DataBaseFirst.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Utilities.Shared;

namespace APIRestSistemaVentas.Controllers
{
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
        public async Task<ActionResult<ApiResponse<Categorium>>> GetCategorias()
        {
            var categorias = await _categoriaService.ListarCategoriasAsync();
            return Ok(categorias);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<Categorium>>>> GetCategoriasPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _categoriaService.ListarCategoriasPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categorium>> GetCategoria(int id)
        {
            var response = await _categoriaService.ObtenerCategoriaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/categoria
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarCategoria([FromBody] Categorium categoria)
        {
            var response = await _categoriaService.RegistrarCategoriaAsync(categoria);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/categoria/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarCategoria(int id, [FromBody] Categorium categoria)
        {
            var response = await _categoriaService.EditarCategoriaAsync(categoria);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/categoria/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarCategoria(int id)
        {
            var response = await _categoriaService.EliminarCategoriaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
