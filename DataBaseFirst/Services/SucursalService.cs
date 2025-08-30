using DataBaseFirst.Models;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using FluentValidation;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class SucursalService : ISucursalService
    {
        private readonly SucursalRepository _sucursalRepository;
        private readonly IValidator<Sucursal> _validator;

        public SucursalService(SucursalRepository sucursalRepository, IValidator<Sucursal> validator)
        {
            _sucursalRepository = sucursalRepository;
            _validator = validator;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly ISucursalRepository _sucursalRepository;

        public SucursalService(ISucursalRepository sucursalRepository)
        {
            _sucursalRepository = sucursalRepository;
        }*/

        public async Task<ApiResponse<List<Sucursal>>> ListarSucursalesAsync()
        {
            var listaSucursales = await _sucursalRepository.ListarSucursalesAsync();

            if (listaSucursales == null || listaSucursales.Count == 0)
                return new ApiResponse<List<Sucursal>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaSucursales };

            return new ApiResponse<List<Sucursal>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaSucursales };
        }

        public async Task<ApiResponse<Paginacion<Sucursal>>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _sucursalRepository.ListarSucursalesPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<Sucursal>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<Sucursal>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Sucursal>> ObtenerSucursalAsync(int idSucursal)
        {
            var sucursal = await _sucursalRepository.ObtenerSucursalAsync(idSucursal); 

            if (sucursal == null)
            {
                return new ApiResponse<Sucursal> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Sucursal> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = sucursal };
        }

        public async Task<ApiResponse<object>> RegistrarSucursalAsync(Sucursal sucursal)
        {
            var validationResult = await _validator.ValidateAsync(sucursal);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var sucursales = await _sucursalRepository.ListarSucursalesAsync();
            if (sucursales.Any(c => c.Codigo == sucursal.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (sucursales.Any(c => c.Nombre_Sucursal?.ToLower() == sucursal.Nombre_Sucursal?.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe" };

            var result = await _sucursalRepository.RegistrarSucursalAsync(sucursal);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarSucursalAsync(Sucursal sucursal)
        {
            if (sucursal == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(sucursal);
            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var sucursalExistente = await _sucursalRepository.ObtenerSucursalAsync(sucursal.Id_Sucursal);
            if (sucursalExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var categorias = await _sucursalRepository.ListarSucursalesAsync();
            if (categorias.Any(c =>
                c.Nombre_Sucursal?.ToLower() == sucursal.Nombre_Sucursal?.ToLower()
                && c.Id_Sucursal != sucursal.Id_Sucursal))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe." };
            }

            var result = await _sucursalRepository.EditarSucursalAsync(sucursal);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarSucursalAsync(int id)
        {
            var existe = await _sucursalRepository.ObtenerSucursalAsync(id);
            if (existe == null)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            var result = await _sucursalRepository.EliminarSucursalAsync(id);

            if (result > 0)
                return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

            return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
        }
    }
}
