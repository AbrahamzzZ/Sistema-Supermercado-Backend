using Domain.Models.Dto.Compra;
using Infrastructure.Repository.InterfacesRepository;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;


namespace Infrastructure.Services
{
    public class CompraService : ICompraService
    {
        private readonly CompraRepository _compraRepository;
        private readonly IValidator<Compras> _validator;


        public CompraService(CompraRepository compraRepository, IValidator<Compras> validator)
        {
            _compraRepository = compraRepository;
            _validator = validator;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly ICompraRepository _compraRepository;
        private readonly IValidator<Compras> _validator;

        public CompraService(ICompraRepository compraRepository)
        {
            _compraRepository = compraRepository;
        }*/

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

        public async Task<ApiResponse<List<DetalleComprasRepuesta>>> ObtenerDetallesCompraAsync(int idCompra)
        {
            var detalleCompra = await _compraRepository.ObtenerDetallesCompraAsync(idCompra);

            if (detalleCompra == null || detalleCompra.Count == 0)
                return new ApiResponse<List<DetalleComprasRepuesta>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = detalleCompra };

            return new ApiResponse<List<DetalleComprasRepuesta>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = detalleCompra };
        }

        public async Task<ApiResponse<object>> RegistrarCompraAsync(Compras compraDto)
        {
            var validationResult = await _validator.ValidateAsync(compraDto);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var registrado = await _compraRepository.RegistrarCompraAsync(compraDto);
            if (registrado)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }
    }
}
