using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class OfertaService : IOfertaService
    {
        private readonly OfertaRepository _ofertaRepository;

        public OfertaService(OfertaRepository ofertaRepository)
        {
            _ofertaRepository = ofertaRepository;
        }

        public async Task<ApiResponse<List<OfertaProducto>>> ListarOfertasAsync()
        {
            var listaOfertas = await _ofertaRepository.ListarOfertasAsync();

            if (listaOfertas == null || listaOfertas.Count == 0)
                return new ApiResponse<List<OfertaProducto>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaOfertas };

            return new ApiResponse<List<OfertaProducto>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaOfertas };
        }

        public async Task<ApiResponse<Paginacion<OfertaProducto>>> ListarOfertasPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _ofertaRepository.ListarOfertasPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<OfertaProducto>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<OfertaProducto>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Ofertum>> ObtenerOfertaAsync(int idOferta)
        {
            var oferta = await _ofertaRepository.ObtenerOfertaAsync(idOferta);

            if (oferta == null)
            {
                return new ApiResponse<Ofertum> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Ofertum> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = oferta };
        }

        public async Task<ApiResponse<object>> RegistrarOfertaAsync(Ofertum oferta)
        {
            if (oferta == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (oferta.Id_Producto <= 0 || string.IsNullOrWhiteSpace(oferta.Codigo) || string.IsNullOrWhiteSpace(oferta.Nombre_Oferta) || string.IsNullOrWhiteSpace(oferta.Descripcion))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            if (oferta.Descuento < 0 || oferta.Descuento > 100)
                return new ApiResponse<object> { IsSuccess = false, Message = "El descuento debe estar entre 0 y 100" };

            if (oferta.Fecha_Fin < DateOnly.FromDateTime(DateTime.Now))
                return new ApiResponse<object> { IsSuccess = false, Message = "La fecha de fin debe ser una fecha futura" };

            var ofertas = await _ofertaRepository.ListarOfertasAsync();
            if (ofertas.Any(c => c.Codigo == oferta.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (ofertas.Any(c => c.Nombre_Oferta?.ToLower() == oferta.Nombre_Oferta.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe" };

            var result = await _ofertaRepository.RegistrarOfertaAsync(oferta);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarOfertaAsync(Ofertum oferta)
        {
            if (oferta == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (oferta.Id_Producto <= 0 || string.IsNullOrWhiteSpace(oferta.Nombre_Oferta) || string.IsNullOrWhiteSpace(oferta.Descripcion))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var ofertaExistente = await _ofertaRepository.ObtenerOfertaAsync(oferta.Id_Oferta);
            if (ofertaExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            if (oferta.Descuento < 0 || oferta.Descuento > 100)
                return new ApiResponse<object> { IsSuccess = false, Message = "El descuento debe estar entre 0 y 100" };

            if (oferta.Fecha_Fin < DateOnly.FromDateTime(DateTime.Now))
                return new ApiResponse<object> { IsSuccess = false, Message = "La fecha de fin debe ser una fecha futura" };

            var ofertas = await _ofertaRepository.ListarOfertasAsync();
            if (ofertas.Any(c => c.Nombre_Oferta == oferta.Nombre_Oferta && c.Id_Oferta != oferta.Id_Oferta))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe." };
            }

            var result = await _ofertaRepository.EditarOfertaAsync(oferta);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarOfertaAsync(int id)
        {
            try
            {
                var existe = await _ofertaRepository.ObtenerOfertaAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _ofertaRepository.EliminarOfertaAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar la oferta porque tiene ventas asociadas." };
            }
        }
    }
}
