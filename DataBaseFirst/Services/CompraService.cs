using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesServices;
using System.Text.RegularExpressions;
using Utilities.Shared;


namespace DataBaseFirst.Services
{
    public class CompraService : ICompraService
    {
        private readonly CompraRepository _compraRepository;
        

        public CompraService(CompraRepository compraRepository)
        {
            _compraRepository = compraRepository;
        }

        public async Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync()
        {
            var numero = await _compraRepository.ObtenerNumeroDocumentoAsync();
            return new ApiResponse<string> { IsSuccess = true, Message = "Número de documento generado correctamente.", Data = numero };
        }

        public async Task<ApiResponse<CompraRespuesta>> ObtenerCompraAsync(string numeroDocumento)
        {
            var numero = await _compraRepository.ObtenerCompraAsync(numeroDocumento);
            if (numero == null)
                return new ApiResponse<CompraRespuesta> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<CompraRespuesta> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = numero };
        }

        public async Task<List<DetalleComprasRepuesta>> ObtenerDetallesCompraAsync(int idCompra)
        {
            return await _compraRepository.ObtenerDetallesCompraAsync(idCompra);
        }

        public async Task<ApiResponse<object>> RegistrarCompraAsync(Compras compraDto)
        {
            if (compraDto == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (compraDto.Id_Usuario <= 0 || compraDto.Id_Proveedor <= 0 || compraDto.Id_Transportista <= 0 || string.IsNullOrWhiteSpace(compraDto.Tipo_Documento) || string.IsNullOrWhiteSpace(compraDto.Numero_Documento) || compraDto.Monto_Total <= 0 || compraDto.Detalles == null || !compraDto.Detalles.Any())
            {
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };
            }

            if (compraDto.Monto_Total <= 0)
                return new ApiResponse<object> { IsSuccess = false, Message = "El monto total debe ser un número positivo." };

            var regexSoloLetras = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regexSoloLetras.IsMatch(compraDto.Tipo_Documento))
                return new ApiResponse<object> { IsSuccess = false, Message = "El tipo de documento solo pueden contener letras." };

            /*var usuarioExiste = await _context.Usuario.AnyAsync(u => u.Id_Usuario == compraDto.Id_Usuario);
            if (!usuarioExiste)
                return new ApiResponse { IsSuccess = false, Message = "El usuario no existe." };

            var proveedorExiste = await _context.Proveedor.AnyAsync(p => p.Id_Proveedor == compraDto.Id_Proveedor);
            if (!proveedorExiste)
                return new ApiResponse { IsSuccess = false, Message = "El proveedor no existe." };

            var transportistaExiste = await _context.Transportista.AnyAsync(t => t.Id_Transportista == compraDto.Id_Transportista);
            if (!transportistaExiste)
                return new ApiResponse { IsSuccess = false, Message = "El transportista no existe." };*/

            var registrado = await _compraRepository.RegistrarCompraAsync(compraDto);
            if (registrado)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }
    }
}
