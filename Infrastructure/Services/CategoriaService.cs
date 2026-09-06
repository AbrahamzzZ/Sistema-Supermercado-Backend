using Domain.Models;
using Domain.Models.Dto.Response.Categoria;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services.business;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly CategoriaRepository _categoriaRepository;
        private readonly IValidator<Categorium> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;

        public CategoriaService(CategoriaRepository categoriaRepository, IValidator<Categorium> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _categoriaRepository = categoriaRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly ICategoriaRepository _categoriaRepository;
        private readonly IValidator<Categorium> _validator;

        public CategoriaService(ICategoriaRepository categoriaRepository, IValidator<Categorium> validator)
        {
            _categoriaRepository = categoriaRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<CategoriaResponse>>> ListarCategoriasAsync()
        {
            try
            {
                var listaCategorias = await _categoriaRepository.ListarCategoriasAsync();

                if (listaCategorias == null || listaCategorias.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Categorías", "No hay categorías registradas");

                    return new ApiResponse<List<CategoriaResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaCategorias };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Categorías", $"Se obtuvieron {listaCategorias.Count} categorías");

                return new ApiResponse<List<CategoriaResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaCategorias };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Categorías", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<CategoriaResponse>>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _categoriaRepository.ListarCategoriasPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || pagedResult.Items.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Categorías Paginación", $"No hay resultados para el filtro: {filtro}");

                    return new ApiResponse<Paginacion<CategoriaResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Categorías Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} categorías. Filtro: {filtro}");

                return new ApiResponse<Paginacion<CategoriaResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Categorías Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<CategoriaResponse>> ObtenerCategoriaAsync(int idCategoria)
        {
            try
            {
                var categoria = await _categoriaRepository.ObtenerCategoriaAsync(idCategoria);

                if (categoria == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Categoría", $"Categoría con ID {idCategoria} no encontrada");

                    return new ApiResponse<CategoriaResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Categoría", $"Categoría {categoria.Codigo} obtenida correctamente");

                return new ApiResponse<CategoriaResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = categoria };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Categoría", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarCategoriaAsync(Categorium categoria)
        {
            try
            {
                if (categoria == null)
                {
                    await _auditoriaService.RegistrarFalloAsync( "Registrar Categoría", "Categoría nula");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(categoria);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync( "Registrar Categoría", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var categorias = await _categoriaRepository.ListarCategoriasAsync();

                if (categorias.Any(c => c.Codigo == categoria.Codigo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Categoría", $"Código {categoria.Codigo} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };
                }

                if (categorias.Any(c => c.Nombre_Categoria?.ToLower() == categoria.Nombre_Categoria?.ToLower()))
                {
                    await _auditoriaService.RegistrarFalloAsync( "Registrar Categoría", $"Nombre {categoria.Nombre_Categoria} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false,  Message = "El nombre ya existe" };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _categoriaRepository.RegistrarCategoriaAsync(categoria, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync( "Registrar Categoría", $"Categoría {categoria.Codigo} registrada exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Categoría", $"No se pudo guardar la categoría {categoria.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Categoría", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarCategoriaAsync(Categorium categoria)
        {
            try
            {
                if (categoria == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Categoría", "Categoría nula");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(categoria);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Categoría", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var categoriaExistente = await _categoriaRepository.ObtenerCategoriaAsync(categoria.Id_Categoria);
                if (categoriaExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Categoría", $"Categoría con ID {categoria.Id_Categoria} no encontrada");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var categorias = await _categoriaRepository.ListarCategoriasAsync();
                if (categorias.Any(c => c.Nombre_Categoria?.ToLower() == categoria.Nombre_Categoria?.ToLower() && c.Id_Categoria != categoria.Id_Categoria))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Categoría", $"Nombre {categoria.Nombre_Categoria} ya existe en otra categoría");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe." };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _categoriaRepository.EditarCategoriaAsync(categoria, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Categoría", $"Categoría {categoria.Codigo} actualizada exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true,  Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Categoría", $"No se pudo actualizar la categoría {categoria.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Categoría", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarCategoriaAsync(int id)
        {
            try
            {
                var existe = await _categoriaRepository.ObtenerCategoriaAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Categoría", $"Categoría con ID {id} no encontrada");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _categoriaRepository.EliminarCategoriaAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Categoría", $"Categoría {existe.Codigo} eliminada exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };
                }

                await _auditoriaService.RegistrarFalloAsync("Eliminar Categoría",  $"No se pudo eliminar la categoría {existe.Codigo}");

                return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Categoría", new Exception("No se puede eliminar: categoría tiene productos asociados"));

                return new ApiResponse<int>{ IsSuccess = false,  Message = "No se puede eliminar la categoría porque tiene productos asociados." };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Categoría", ex);
                throw;
            }
        }
    }
}
