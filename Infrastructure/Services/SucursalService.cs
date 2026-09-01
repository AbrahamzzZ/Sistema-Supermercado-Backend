using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;
using Domain.Models.Dto.Response.Sucursal;

namespace Infrastructure.Services
{
    public class SucursalService : ISucursalService
    {
        private readonly SucursalRepository _sucursalRepository;
        private readonly IValidator<Sucursal> _validator;
        private readonly ICurrentUserService _currentUserService;

        public SucursalService(SucursalRepository sucursalRepository, IValidator<Sucursal> validator, ICurrentUserService currentUserService)
        {
            _sucursalRepository = sucursalRepository;
            _validator = validator;
            _currentUserService = currentUserService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly ISucursalRepository _sucursalRepository;
        private readonly IValidator<Sucursal> _validator;

        public SucursalService(ISucursalRepository sucursalRepository, IValidator<Sucursal> validator)
        {
            _sucursalRepository = sucursalRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<SucursalResponse>>> ListarSucursalesAsync()
        {
            var listaSucursales = await _sucursalRepository.ListarSucursalesAsync();

            if (listaSucursales == null || listaSucursales.Count == 0)
                return new ApiResponse<List<SucursalResponse>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaSucursales };

            return new ApiResponse<List<SucursalResponse>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaSucursales };
        }

        public async Task<ApiResponse<Paginacion<SucursalResponse>>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            var pagedResult = await _sucursalRepository.ListarSucursalesPaginacionAsync(pageNumber, pageSize, filtro);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<SucursalResponse>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<SucursalResponse>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<SucursalResponse>> ObtenerSucursalAsync(int idSucursal)
        {
            var sucursal = await _sucursalRepository.ObtenerSucursalAsync(idSucursal); 

            if (sucursal == null)
            {
                return new ApiResponse<SucursalResponse> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<SucursalResponse> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = sucursal };
        }

        public async Task<ApiResponse<object>> RegistrarSucursalAsync(Sucursal sucursal)
        {

            if (sucursal == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(sucursal);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var sucursales = await _sucursalRepository.ListarSucursalesAsync();
            if (sucursales.Any(c => c.Codigo == sucursal.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };

            if (sucursales.Any(c => c.Nombre_Sucursal?.ToLower() == sucursal.Nombre_Sucursal?.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe" };

            var idUsuario = _currentUserService.GetUserId();

            var result = await _sucursalRepository.RegistrarSucursalAsync(sucursal, idUsuario);
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

            var sucursales = await _sucursalRepository.ListarSucursalesAsync();
            if (sucursales.Any(c =>
                c.Nombre_Sucursal?.ToLower() == sucursal.Nombre_Sucursal?.ToLower()
                && c.Id_Sucursal != sucursal.Id_Sucursal))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe." };
            }

            var idUsuario = _currentUserService.GetUserId();

            var result = await _sucursalRepository.EditarSucursalAsync(sucursal, idUsuario);
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
