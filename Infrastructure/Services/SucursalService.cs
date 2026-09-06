using Domain.Models;
using Domain.Models.Dto.Response.Sucursal;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services.business;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class SucursalService : ISucursalService
    {
        private readonly SucursalRepository _sucursalRepository;
        private readonly IValidator<Sucursal> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;

        public SucursalService(SucursalRepository sucursalRepository, IValidator<Sucursal> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _sucursalRepository = sucursalRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;
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
            try
            {
                var listaSucursales = await _sucursalRepository.ListarSucursalesAsync();

                if (listaSucursales == null || listaSucursales.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Sucursales", "No hay sucursales registradas");

                    return new ApiResponse<List<SucursalResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaSucursales };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Sucursales", $"Se obtuvieron {listaSucursales.Count} sucursales");

                return new ApiResponse<List<SucursalResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaSucursales };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Sucursales", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<SucursalResponse>>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _sucursalRepository.ListarSucursalesPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || pagedResult.Items.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Sucursales Paginación", $"No hay resultados para el filtro: {filtro}");
                    
                    return new ApiResponse<Paginacion<SucursalResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Sucursales Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} sucursales. Filtro: {filtro}");

                return new ApiResponse<Paginacion<SucursalResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Sucursales Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<SucursalResponse>> ObtenerSucursalAsync(int idSucursal)
        {
            try
            {
                var sucursal = await _sucursalRepository.ObtenerSucursalAsync(idSucursal);

                if (sucursal == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Sucursal", $"Sucursal con ID {idSucursal} no encontrada");

                    return new ApiResponse<SucursalResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Sucursal", $"Sucursal {sucursal.Codigo} obtenida correctamente");

                return new ApiResponse<SucursalResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = sucursal };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Sucursal", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarSucursalAsync(Sucursal sucursal)
        {
            try
            {
                if (sucursal == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Sucursal", "Sucursal nula");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(sucursal);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Registrar Sucursal", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var sucursales = await _sucursalRepository.ListarSucursalesAsync();

                if (sucursales.Any(c => c.Codigo == sucursal.Codigo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Sucursal", $"Código {sucursal.Codigo} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };
                }

                if (sucursales.Any(c => c.Nombre_Sucursal?.ToLower() == sucursal.Nombre_Sucursal?.ToLower()))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Sucursal", $"Nombre {sucursal.Nombre_Sucursal} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe" };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _sucursalRepository.RegistrarSucursalAsync(sucursal, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Registrar Sucursal", $"Sucursal {sucursal.Codigo} registrada exitosamente");
                    
                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Sucursal", $"No se pudo guardar la sucursal {sucursal.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Sucursal", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarSucursalAsync(Sucursal sucursal)
        {
            try
            {
                if (sucursal == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Sucursal", "Sucursal nula");
                    
                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(sucursal);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Sucursal", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var sucursalExistente = await _sucursalRepository.ObtenerSucursalAsync(sucursal.Id_Sucursal);
                if (sucursalExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Sucursal", $"Sucursal con ID {sucursal.Id_Sucursal} no encontrada");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var sucursales = await _sucursalRepository.ListarSucursalesAsync();
                if (sucursales.Any(c => c.Nombre_Sucursal?.ToLower() == sucursal.Nombre_Sucursal?.ToLower() && c.Id_Sucursal != sucursal.Id_Sucursal))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Sucursal", $"Nombre {sucursal.Nombre_Sucursal} ya existe en otra sucursal");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe." };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _sucursalRepository.EditarSucursalAsync(sucursal, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Sucursal", $"Sucursal {sucursal.Codigo} actualizada exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync( "Editar Sucursal", $"No se pudo actualizar la sucursal {sucursal.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Sucursal", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarSucursalAsync(int id)
        {
            try
            {
                var existe = await _sucursalRepository.ObtenerSucursalAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Sucursal", $"Sucursal con ID {id} no encontrada");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _sucursalRepository.EliminarSucursalAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Sucursal", $"Sucursal {existe.Codigo} eliminada exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };
                }

                await _auditoriaService.RegistrarFalloAsync("Eliminar Sucursal", $"No se pudo eliminar la sucursal {existe.Codigo}");

                return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Sucursal", ex);
                throw;
            }
        }
    }
}
