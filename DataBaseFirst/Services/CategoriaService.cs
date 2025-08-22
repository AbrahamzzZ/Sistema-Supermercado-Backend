using DataBaseFirst.Models;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly CategoriaRepository _categoriaRepository;

        public CategoriaService(CategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.
        
        /*readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }*/

        public async Task<ApiResponse<List<Categorium>>> ListarCategoriasAsync()
        {
            var listaCategorias = await _categoriaRepository.ListarCategoriasAsync();

            if (listaCategorias == null || listaCategorias.Count == 0)
                return new ApiResponse<List<Categorium>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaCategorias };

            return new ApiResponse<List<Categorium>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaCategorias };
        }

        public async Task<ApiResponse<Paginacion<Categorium>>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _categoriaRepository.ListarCategoriasPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<Categorium>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<Categorium>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Categorium>> ObtenerCategoriaAsync(int idCategoria)
        {
            var categoria = await _categoriaRepository.ObtenerCategoriaAsync(idCategoria);

            if (categoria == null)
            {
                return new ApiResponse<Categorium> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Categorium> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = categoria };
        }

        public async Task<ApiResponse<object>> RegistrarCategoriaAsync(Categorium categoria)
        {
            if (categoria == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(categoria.Codigo) || string.IsNullOrWhiteSpace(categoria.Nombre_Categoria))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(categoria.Nombre_Categoria))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre solo puede contener letras y espacios" };

            var categorias = await _categoriaRepository.ListarCategoriasAsync();
            if (categorias.Any(c => c.Codigo == categoria.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (categorias.Any(c => c.Nombre_Categoria?.ToLower() == categoria.Nombre_Categoria.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe" };

            var result = await _categoriaRepository.RegistrarCategoriaAsync(categoria);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarCategoriaAsync(Categorium categoria)
        {
            if (categoria == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(categoria.Nombre_Categoria))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var categoriaExistente = await _categoriaRepository.ObtenerCategoriaAsync(categoria.Id_Categoria);
            if (categoriaExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(categoria.Nombre_Categoria))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre solo puede contener letras y espacios." };

            var categorias = await _categoriaRepository.ListarCategoriasAsync();
            if (categorias.Any(c =>
                c.Nombre_Categoria?.ToLower() == categoria.Nombre_Categoria.ToLower()
                && c.Id_Categoria != categoria.Id_Categoria))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe." };
            }

            var result = await _categoriaRepository.EditarCategoriaAsync(categoria);
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
