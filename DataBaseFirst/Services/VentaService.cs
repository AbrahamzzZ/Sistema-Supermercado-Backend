using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using FluentValidation;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class VentaService : IVentaService
    {
        private readonly VentaRepository _ventaRepository;
        private readonly IValidator<Ventas> _validator;

        public VentaService(VentaRepository ventaRepository, IValidator<Ventas> validator)
        {
            _ventaRepository = ventaRepository;
            _validator = validator;
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

        public async Task<ApiResponse<List<DetalleVentasRepuesta>>> ObtenerDetallesVentaAsync(int idVenta)
        {
            var detalleVenta = await _ventaRepository.ObtenerDetallesVentaAsync(idVenta);

            if(detalleVenta == null || detalleVenta.Count == 0)
                return new ApiResponse<List<DetalleVentasRepuesta>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = detalleVenta };

            return new ApiResponse<List<DetalleVentasRepuesta>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = detalleVenta };
        }

        public async Task<ApiResponse<object>> RegistrarVentaAsync(Ventas ventaDto)
        {
            var validationResult = await _validator.ValidateAsync(ventaDto);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var registrado = await _ventaRepository.RegistrarVentaAsync(ventaDto);
            if (registrado)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }
    }
}
