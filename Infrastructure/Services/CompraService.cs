using Domain.Models;
using Domain.Models.Dto.Response.Compra;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;


namespace Infrastructure.Services
{
    public class CompraService : ICompraService
    {
        private readonly CompraRepository _compraRepository;
        private readonly IValidator<Compras> _validator;
        private readonly IAuditoriaService _auditoriaService;

        public CompraService(CompraRepository compraRepository, IValidator<Compras> validator, IAuditoriaService auditoriaService)
        {
            _compraRepository = compraRepository;
            _validator = validator;
            _auditoriaService = auditoriaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly ICompraRepository _compraRepository;
        private readonly IValidator<Compras> _validator;

        public CompraService(ICompraRepository compraRepository, IValidator<Compras> validator)
        {
            _compraRepository = compraRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync()
        {
            try
            {
                var numero = await _compraRepository.ObtenerNumeroDocumentoAsync();

                await _auditoriaService.RegistrarExitoAsync("Obtener Número Documento Compra", $"Número generado: {numero}");

                return new ApiResponse<string>{ IsSuccess = true, Message = "Número de documento generado correctamente.", Data = numero };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Número Documento Compra", ex);
                throw;
            }
        }

        public async Task<ApiResponse<CompraResponse>> ObtenerCompraAsync(string numeroDocumento)
        {
            try
            {
                var numero = await _compraRepository.ObtenerCompraAsync(numeroDocumento);

                if (numero == null)
                {
                    await _auditoriaService.RegistrarFalloAsync( "Obtener Compra", $"Compra con número {numeroDocumento} no encontrada");
                    return new ApiResponse<CompraResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Compra", $"Compra {numeroDocumento} obtenida correctamente");

                return new ApiResponse<CompraResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = numero };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Compra", ex);
                throw;
            }
        }

        public async Task<ApiResponse<List<DetalleCompraReponse>>> ObtenerDetallesCompraAsync(int idCompra)
        {
            try
            {
                var detalleCompra = await _compraRepository.ObtenerDetallesCompraAsync(idCompra);

                if (detalleCompra == null || detalleCompra.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync( "Obtener Detalles Compra", $"No hay detalles para la compra ID {idCompra}");
                    
                    return new ApiResponse<List<DetalleCompraReponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = detalleCompra };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Detalles Compra", $"Se obtuvieron {detalleCompra.Count} detalles para compra ID {idCompra}");

                return new ApiResponse<List<DetalleCompraReponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = detalleCompra };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Detalles Compra", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarCompraAsync(Compras compraDto)
        {
            try
            {
                if (compraDto == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Compra", "Compra nula");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(compraDto);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Registrar Compra", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var registrado = await _compraRepository.RegistrarCompraAsync(compraDto);

                if (registrado)
                {
                    await _auditoriaService.RegistrarExitoAsync("Registrar Compra", $"Compra registrada exitosamente. Número documento: {compraDto.Numero_Documento}");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Compra", $"No se pudo guardar la compra {compraDto.Numero_Documento}");
                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Compra", ex);
                throw;
            }
        }
    }
}
