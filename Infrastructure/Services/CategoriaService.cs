using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using FluentValidation;
using Domain.Models.Dto.Response.Categoria;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly CategoriaRepository _categoriaRepository;
        private readonly IValidator<Categorium> _validator;
        private readonly ICurrentUserService _currentUserService;

        public CategoriaService(CategoriaRepository categoriaRepository, IValidator<Categorium> validator, ICurrentUserService currentUserService)
        {
            _categoriaRepository = categoriaRepository;
            _validator = validator;
            _currentUserService = currentUserService;
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
            var listaCategorias = await _categoriaRepository.ListarCategoriasAsync();

            if (listaCategorias == null || listaCategorias.Count == 0)
                return new ApiResponse<List<CategoriaResponse>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaCategorias };

            return new ApiResponse<List<CategoriaResponse>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaCategorias };
        }

        public async Task<ApiResponse<Paginacion<CategoriaResponse>>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            var pagedResult = await _categoriaRepository.ListarCategoriasPaginacionAsync(pageNumber, pageSize, filtro);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<CategoriaResponse>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<CategoriaResponse>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<CategoriaResponse>> ObtenerCategoriaAsync(int idCategoria)
        {
            var categoria = await _categoriaRepository.ObtenerCategoriaAsync(idCategoria);

            if (categoria == null)
            {
                return new ApiResponse<CategoriaResponse> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<CategoriaResponse> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = categoria };
        }

        public async Task<ApiResponse<object>> RegistrarCategoriaAsync(Categorium categoria)
        {
            var validationResult = await _validator.ValidateAsync(categoria);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var categorias = await _categoriaRepository.ListarCategoriasAsync();
            if (categorias.Any(c => c.Codigo == categoria.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };

            if (categorias.Any(c => c.Nombre_Categoria?.ToLower() == categoria.Nombre_Categoria?.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe" };

            var idUsuario = _currentUserService.GetUserId();

            var result = await _categoriaRepository.RegistrarCategoriaAsync(categoria, idUsuario);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarCategoriaAsync(Categorium categoria)
        {
            if (categoria == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(categoria);
            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var categoriaExistente = await _categoriaRepository.ObtenerCategoriaAsync(categoria.Id_Categoria);
            if (categoriaExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var categorias = await _categoriaRepository.ListarCategoriasAsync();
            if (categorias.Any(c => c.Nombre_Categoria?.ToLower() == categoria.Nombre_Categoria?.ToLower() && c.Id_Categoria != categoria.Id_Categoria))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe." };
            }

            var idUsuario = _currentUserService.GetUserId();

            var result = await _categoriaRepository.EditarCategoriaAsync(categoria, idUsuario);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarCategoriaAsync(int id)
        {
            try
            {
                var existe = await _categoriaRepository.ObtenerCategoriaAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int>
                    { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _categoriaRepository.EliminarCategoriaAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547) 
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar la categoría porque tiene productos asociados." };
            }
        }
    }
}
