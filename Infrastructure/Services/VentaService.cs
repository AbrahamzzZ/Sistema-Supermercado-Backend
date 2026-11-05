using Domain.Models.Dto.Response.Venta;
using FluentValidation;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IValidator<Ventas> _validator;
        private readonly IAuditoriaService _auditoriaService;
        private readonly ICurrentUser _currentUser;

        public VentaService(IVentaRepository ventaRepository, IValidator<Ventas> validator, IAuditoriaService auditoriaService, ICurrentUser currentUser)
        {
            _ventaRepository = ventaRepository;
            _validator = validator;
            _auditoriaService = auditoriaService;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync()
        {
            try
            {
                var numero = await _ventaRepository.ObtenerNumeroDocumentoAsync();

                await _auditoriaService.RegistrarExitoAsync("Obtener Número Documento Venta", $"Número generado: {numero}");

                return new ApiResponse<string>{ IsSuccess = true, Message = "Número de documento generado correctamente.", Data = numero };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Número Documento Venta", ex);
                throw;
            }
        }

        public async Task<ApiResponse<VentaResponse>> ObtenerVentaAsync(string numeroDocumento)
        {
            try
            {
                var numero = await _ventaRepository.ObtenerVentaAsync(numeroDocumento);

                if (numero == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Venta", $"Venta con número {numeroDocumento} no encontrada");

                    return new ApiResponse<VentaResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Venta", $"Venta {numeroDocumento} obtenida correctamente");

                return new ApiResponse<VentaResponse>{ IsSuccess = true,  Message = Mensajes.MESSAGE_QUERY, Data = numero };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Venta", ex);
                throw;
            }
        }

        public async Task<ApiResponse<List<DetalleVentaReponse>>> ObtenerDetallesVentaAsync(int idVenta)
        {
            try
            {
                var detalleVenta = await _ventaRepository.ObtenerDetallesVentaAsync(idVenta);

                if (detalleVenta == null || detalleVenta.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Detalles Venta", $"No hay detalles para la venta ID {idVenta}");

                    return new ApiResponse<List<DetalleVentaReponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = detalleVenta };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Detalles Venta", $"Se obtuvieron {detalleVenta.Count} detalles para venta ID {idVenta}");

                return new ApiResponse<List<DetalleVentaReponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = detalleVenta };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Detalles Venta", ex);
                throw;
            }
        }

        public async Task<ApiResponse<VentaRegistradaResponse>> RegistrarVentaAsync(Ventas ventaDto)
        {
            try
            {
                if (ventaDto == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Venta","Venta nula" );

                    return new ApiResponse<VentaRegistradaResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(ventaDto);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync( "Registrar Venta", $"Validación fallida: {errores}");

                    return new ApiResponse<VentaRegistradaResponse> { IsSuccess = false, Message = errores };
                }

                var idUsuario = _currentUser.GetUserId();
                if (idUsuario <= 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Venta", "No se pudo identificar al usuario del token");

                    return new ApiResponse<VentaRegistradaResponse>{ IsSuccess = false, Message = "No se pudo identificar al usuario que registra la venta." };
                }

                var venta = await _ventaRepository.RegistrarVentaAsync(ventaDto, idUsuario);

                await _auditoriaService.RegistrarExitoAsync("Registrar Venta", $"Venta {venta.Numero_Documento} registrada. Total: {venta.Monto_Total:0.00}, descuento: {venta.Descuento:0.00}");

                return new ApiResponse<VentaRegistradaResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER, Data = venta };
            }
            catch (ReglaNegocioException ex)
            {
                await _auditoriaService.RegistrarFalloAsync("Registrar Venta", ex.Message);

                return new ApiResponse<VentaRegistradaResponse>{ IsSuccess = false, Message = ex.Message };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Venta", ex);
                throw;
            }
        }
    }
}
