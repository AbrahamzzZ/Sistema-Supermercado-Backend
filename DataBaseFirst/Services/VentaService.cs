using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using System.Text.RegularExpressions;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class VentaService : IVentaService
    {
        private readonly VentaRepository _ventaRepository;

        public VentaService(VentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IVentaRepository _ventaRepository;

        public VentaService(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }*/

        public async Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync()
        {
            var numero = await _ventaRepository.ObtenerNumeroDocumentoAsync();
            return new ApiResponse<string> { IsSuccess = true, Message = "Número de documento generado correctamente.", Data = numero };
        }

        public async Task<ApiResponse<VentaRespuesta>> ObtenerVentaAsync(string numeroDocumento)
        {
            var numero = await _ventaRepository.ObtenerVentaAsync(numeroDocumento);
            if(numero == null) 
                return new ApiResponse<VentaRespuesta> {IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<VentaRespuesta> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = numero };
        }

        public async Task<List<DetalleVentasRepuesta>> ObtenerDetallesVentaAsync(int idVenta)
        {
            return await _ventaRepository.ObtenerDetallesVentaAsync(idVenta);
        }

        public async Task<ApiResponse<object>> RegistrarVentaAsync(Ventas ventaDto)
        {
            if (ventaDto == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (ventaDto.Id_Usuario <= 0 || ventaDto.Id_Cliente <= 0 || string.IsNullOrWhiteSpace(ventaDto.Tipo_Documento) || string.IsNullOrWhiteSpace(ventaDto.Numero_Documento) || ventaDto.Monto_Total <= 0 || ventaDto.Monto_Pago <= 0 || ventaDto.Detalles == null || !ventaDto.Detalles.Any())
            {
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };
            }

            if (ventaDto.Monto_Total <= 0)
                return new ApiResponse<object> { IsSuccess = false, Message = "El monto total debe ser un número positivo." };

            if (ventaDto.Monto_Pago <= 0)
                return new ApiResponse<object> { IsSuccess = false, Message = "El monto pago debe ser un número positivo." };

            if (ventaDto.Monto_Cambio < 0)
                return new ApiResponse<object> { IsSuccess = false, Message = "El monto cambio debe ser un número positivo." };

            var regexSoloLetras = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regexSoloLetras.IsMatch(ventaDto.Tipo_Documento))
                return new ApiResponse<object> { IsSuccess = false, Message = "El tipo de documento solo pueden contener letras." };

            var registrado = await _ventaRepository.RegistrarVentaAsync(ventaDto);
            if (registrado)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }
    }
}
