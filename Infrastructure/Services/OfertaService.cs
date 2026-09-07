using Domain.Models;
using Domain.Models.Dto.Response.Oferta;
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
    public class OfertaService : IOfertaService
    {
        private readonly OfertaRepository _ofertaRepository;
        private readonly IValidator<Ofertum> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;

        public OfertaService(OfertaRepository ofertaRepository, IValidator<Ofertum> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _ofertaRepository = ofertaRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IOfertaRepository _ofertaRepository;
        private readonly IValidator<Ofertum> _validator;

        public OfertaService(IOfertaRepository ofertaRepository, IValidator<Ofertum> validator)
        {
            _ofertaRepository = ofertaRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<OfertaProductoResponse>>> ListarOfertasAsync()
        {
            try
            {
                var listaOfertas = await _ofertaRepository.ListarOfertasAsync();

                if (listaOfertas == null || listaOfertas.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Ofertas", "No hay ofertas registradas");

                    return new ApiResponse<List<OfertaProductoResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaOfertas };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Ofertas", $"Se obtuvieron {listaOfertas.Count} ofertas");

                return new ApiResponse<List<OfertaProductoResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaOfertas };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Ofertas", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<OfertaProductoResponse>>> ListarOfertasPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _ofertaRepository.ListarOfertasPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || pagedResult.Items.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Ofertas Paginación", $"No hay resultados para el filtro: {filtro}");

                    return new ApiResponse<Paginacion<OfertaProductoResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Ofertas Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} ofertas. Filtro: {filtro}");

                return new ApiResponse<Paginacion<OfertaProductoResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Ofertas Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<OfertaProductoResponse>> ObtenerOfertaAsync(int idOferta)
        {
            try
            {
                var oferta = await _ofertaRepository.ObtenerOfertaAsync(idOferta);

                if (oferta == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Oferta", $"Oferta con ID {idOferta} no encontrada");
                    
                    return new ApiResponse<OfertaProductoResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Oferta", $"Oferta {oferta.Codigo} obtenida correctamente");

                return new ApiResponse<OfertaProductoResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = oferta };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Oferta", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarOfertaAsync(Ofertum oferta)
        {
            try
            {
                if (oferta == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Oferta", "Oferta nula");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(oferta);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync( "Registrar Oferta", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var ofertas = await _ofertaRepository.ListarOfertasAsync();

                if (ofertas.Any(c => c.Codigo == oferta.Codigo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Oferta", $"Código {oferta.Codigo} ya existe");
                   
                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };
                }

                if (ofertas.Any(c => c.Nombre_Oferta?.ToLower() == oferta.Nombre_Oferta?.ToLower()))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Oferta", $"Nombre {oferta.Nombre_Oferta} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe" };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _ofertaRepository.RegistrarOfertaAsync(oferta, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Registrar Oferta", $"Oferta {oferta.Codigo} registrada exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Oferta", $"No se pudo guardar la oferta {oferta.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Oferta", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarOfertaAsync(Ofertum oferta)
        {
            try
            {
                if (oferta == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Oferta", "Oferta nula");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(oferta);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Oferta", $"Validación fallida: {errores}");

                    return new ApiResponse<object>
                    { IsSuccess = false, Message = errores };
                }

                var ofertaExistente = await _ofertaRepository.ObtenerOfertaAsync(oferta.Id_Oferta);
                if (ofertaExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Oferta", $"Oferta con ID {oferta.Id_Oferta} no encontrada");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var ofertas = await _ofertaRepository.ListarOfertasAsync();
                if (ofertas.Any(c => c.Nombre_Oferta?.ToLower() == oferta.Nombre_Oferta?.ToLower() && c.Id_Oferta != oferta.Id_Oferta))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Oferta", $"Nombre {oferta.Nombre_Oferta} ya existe en otra oferta");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe." };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _ofertaRepository.EditarOfertaAsync(oferta, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Oferta", $"Oferta {oferta.Codigo} actualizada exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Oferta", $"No se pudo actualizar la oferta {oferta.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Oferta", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarOfertaAsync(int id)
        {
            try
            {
                var existe = await _ofertaRepository.ObtenerOfertaAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Oferta", $"Oferta con ID {id} no encontrada");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _ofertaRepository.EliminarOfertaAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Oferta", $"Oferta {existe.Codigo} eliminada exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE};
                }

                await _auditoriaService.RegistrarFalloAsync("Eliminar Oferta", $"No se pudo eliminar la oferta {existe.Codigo}");

                return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Oferta", new Exception("No se puede eliminar: oferta tiene ventas asociadas"));

                return new ApiResponse<int>{ IsSuccess = false, Message = "No se puede eliminar la oferta porque tiene ventas asociadas." };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Oferta", ex);
                throw;
            }
        }
    }
}
