using Domain.Models;
using Domain.Models.Dto.Response.Transportista;
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
    public class TransportistaService : ITransportistaService
    {
        private readonly TransportistaRepository _transportistaRepository;
        private readonly IValidator<Transportistum> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;

        public TransportistaService(TransportistaRepository transportistaRepository, IValidator<Transportistum> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _transportistaRepository = transportistaRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly ITransportistaRepository _transportistaRepository;
        private readonly IValidator<Transportistum> _validator;
        public TransportistaService(ITransportistaRepository transportistaRepository, IValidator<Transportistum> validator)
        {
            _transportistaRepository = transportistaRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<TransportistaResponse>>> ListarTransportistasAsync()
        {
            try
            {
                var listaTransportistas = await _transportistaRepository.ListarTransportistasAsync();

                if (listaTransportistas == null || listaTransportistas.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Transportistas", "No hay transportistas registrados");

                    return new ApiResponse<List<TransportistaResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaTransportistas };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Transportistas", $"Se obtuvieron {listaTransportistas.Count} transportistas");

                return new ApiResponse<List<TransportistaResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaTransportistas };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Transportistas", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<TransportistaResponse>>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _transportistaRepository.ListarTransportistasPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || !pagedResult.Items.Any())
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Transportistas Paginación", $"No hay resultados para el filtro: {filtro}");

                    return new ApiResponse<Paginacion<TransportistaResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Transportistas Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} transportistas. Filtro: {filtro}");

                return new ApiResponse<Paginacion<TransportistaResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Transportistas Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<TransportistaResponse>> ObtenerTransportistaAsync(int idTranportista)
        {
            try
            {
                var transportista = await _transportistaRepository.ObtenerTransportistaAsync(idTranportista);

                if (transportista == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Transportista", $"Transportista con ID {idTranportista} no encontrado");

                    return new ApiResponse<TransportistaResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Transportista", $"Transportista {transportista.Codigo} obtenido correctamente");

                return new ApiResponse<TransportistaResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = transportista };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Transportista", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarTransportistaAsync(Transportistum transportista)
        {
            try
            {
                if (transportista == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Transportista", "Transportista nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(transportista);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Registrar Transportista", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var transportistas = await _transportistaRepository.ListarTransportistasAsync();

                if (transportistas.Any(c => c.Codigo == transportista.Codigo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Transportista", $"Código {transportista.Codigo} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };
                }

                if (transportistas.Any(c => c.Cedula == transportista.Cedula))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Transportista", $"Cédula {transportista.Cedula} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CEDULA_EXITS };
                }

                if (transportistas.Any(c => c.Telefono == transportista.Telefono))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Transportista", $"Teléfono {transportista.Telefono} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_PHONE_EXITS };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _transportistaRepository.RegistrarTransportistaAsync(transportista, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Registrar Transportista", $"Transportista {transportista.Codigo} registrado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Transportista", $"No se pudo guardar el transportista {transportista.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Transportista", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarTransportistaAsync(Transportistum transportista)
        {
            try
            {
                if (transportista == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Transportista", "Transportista nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL};
                }

                var validationResult = await _validator.ValidateAsync(transportista);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Transportista", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var transportistaExistente = await _transportistaRepository.ObtenerTransportistaAsync(transportista.Id_Transportista);
                if (transportistaExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Transportista",$"Transportista con ID {transportista.Id_Transportista} no encontrado");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var transportistas = await _transportistaRepository.ListarTransportistasAsync();
                if (transportistas.Any(c => c.Cedula == transportista.Cedula && c.Id_Transportista != transportista.Id_Transportista))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Transportista", $"Cédula {transportista.Cedula} ya existe en otro transportista");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CEDULA_EXITS };
                }
                else if (transportistas.Any(c => c.Telefono == transportista.Telefono && c.Id_Transportista != transportista.Id_Transportista))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Transportista", $"Teléfono {transportista.Telefono} ya existe en otro transportista");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_PHONE_EXITS };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _transportistaRepository.EditarTransportistaAsync(transportista, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Transportista", $"Transportista {transportista.Codigo} actualizado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Transportista", $"No se pudo actualizar el transportista {transportista.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED};
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Transportista", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarTransportistaAsync(int id)
        {
            try
            {
                var existe = await _transportistaRepository.ObtenerTransportistaAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Transportista", $"Transportista con ID {id} no encontrado");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _transportistaRepository.EliminarTransportistaAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Transportista", $"Transportista {existe.Codigo} eliminado exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };
                }

                await _auditoriaService.RegistrarFalloAsync("Eliminar Transportista", $"No se pudo eliminar el transportista {existe.Codigo}");

                return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Transportista", new Exception("No se puede eliminar: transportista tiene compras asociadas"));

                return new ApiResponse<int>{ IsSuccess = false, Message = "No se puede eliminar al transportista porque tiene compras asociadas." };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Transportista", ex);
                throw;
            }
        }

    }
}
