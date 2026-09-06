using Domain.Models;
using Domain.Models.Dto.Response.Provedor;
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
    public class ProveedorService : IProveedorService
    {
        private readonly ProveedorRepository _proveedorRepository;
        private readonly IValidator<Proveedor> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;

        public ProveedorService(ProveedorRepository proveedorRepository, IValidator<Proveedor> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _proveedorRepository = proveedorRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IProveedorRepository _proveedorRepository;
        private readonly IValidator<Proveedor> _validator;

        public ProveedorService(IProveedorRepository proveedorRepository, IValidator<Proveedor> validator)
        {
            _proveedorRepository = proveedorRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<ProveedorResponse>>> ListarProveedoresAsync()
        {
            try
            {
                var listaProveedores = await _proveedorRepository.ListarProveedoresAsync();

                if (listaProveedores == null || listaProveedores.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Proveedores", "No hay proveedores registrados");

                    return new ApiResponse<List<ProveedorResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaProveedores };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Proveedores", $"Se obtuvieron {listaProveedores.Count} proveedores");

                return new ApiResponse<List<ProveedorResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaProveedores };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Proveedores", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<ProveedorResponse>>> ListarProveedoresPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _proveedorRepository.ListarProveedoresPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || pagedResult.Items.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Proveedores Paginación", $"No hay resultados para el filtro: {filtro}");

                    return new ApiResponse<Paginacion<ProveedorResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Proveedores Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} proveedores. Filtro: {filtro}");

                return new ApiResponse<Paginacion<ProveedorResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Proveedores Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<ProveedorResponse>> ObtenerProveedorAsync(int idProveedor)
        {
            try
            {
                var proveedor = await _proveedorRepository.ObtenerProveedorAsync(idProveedor);

                if (proveedor == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Proveedor", $"Proveedor con ID {idProveedor} no encontrado");

                    return new ApiResponse<ProveedorResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Proveedor", $"Proveedor {proveedor.Codigo} obtenido correctamente");

                return new ApiResponse<ProveedorResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = proveedor };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Proveedor", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarProveedorAsync(Proveedor proveedor)
        {
            try
            {
                if (proveedor == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Proveedor", "Proveedor nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(proveedor);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Registrar Proveedor", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var proveedores = await _proveedorRepository.ListarProveedoresAsync();

                if (proveedores.Any(c => c.Codigo == proveedor.Codigo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Proveedor", $"Código {proveedor.Codigo} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };
                }

                if (proveedores.Any(c => c.Cedula == proveedor.Cedula))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Proveedor", $"Cédula {proveedor.Cedula} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CEDULA_EXITS };
                }

                if (proveedores.Any(c => c.Telefono == proveedor.Telefono))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Proveedor", $"Teléfono {proveedor.Telefono} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_PHONE_EXITS };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _proveedorRepository.RegistrarProveedorAsync(proveedor, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Registrar Proveedor", $"Proveedor {proveedor.Codigo} registrado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Proveedor", $"No se pudo guardar el proveedor {proveedor.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Proveedor", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarProveedorAsync(Proveedor proveedor)
        {
            try
            {
                if (proveedor == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Proveedor", "Proveedor nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(proveedor);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Proveedor", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var proveedorExistente = await _proveedorRepository.ObtenerProveedorAsync(proveedor.Id_Proveedor);
                if (proveedorExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Proveedor", $"Proveedor con ID {proveedor.Id_Proveedor} no encontrado");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var proveedores = await _proveedorRepository.ListarProveedoresAsync();
                if (proveedores.Any(c => c.Cedula == proveedor.Cedula && c.Id_Proveedor != proveedor.Id_Proveedor))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Proveedor", $"Cédula {proveedor.Cedula} ya existe en otro proveedor");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CEDULA_EXITS };
                }
                else if (proveedores.Any(c => c.Telefono == proveedor.Telefono && c.Id_Proveedor != proveedor.Id_Proveedor))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Proveedor", $"Teléfono {proveedor.Telefono} ya existe en otro proveedor");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_PHONE_EXITS };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _proveedorRepository.EditarProveedorAsync(proveedor, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Proveedor", $"Proveedor {proveedor.Codigo} actualizado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Proveedor", $"No se pudo actualizar el proveedor {proveedor.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Proveedor", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarProveedorAsync(int id)
        {
            try
            {
                var existe = await _proveedorRepository.ObtenerProveedorAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Proveedor", $"Proveedor con ID {id} no encontrado");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _proveedorRepository.EliminarProveedorAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Proveedor", $"Proveedor {existe.Codigo} eliminado exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };
                }

                await _auditoriaService.RegistrarFalloAsync( "Eliminar Proveedor", $"No se pudo eliminar el proveedor {existe.Codigo}");

                return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Proveedor", new Exception("No se puede eliminar: proveedor tiene compras asociadas"));

                return new ApiResponse<int>{ IsSuccess = false, Message = "No se puede eliminar al proveedor porque tiene compras asociadas." };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Proveedor", ex);
                throw;
            }
        }
    }
}
