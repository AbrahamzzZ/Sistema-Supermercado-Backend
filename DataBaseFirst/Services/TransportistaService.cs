using DataBaseFirst.Models;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class TransportistaService : ITransportistaService
    {
        private readonly TransportistaRepository _transportistaRepository;
        private readonly IValidator<Transportistum> _validator;

        public TransportistaService(TransportistaRepository transportistaRepository, IValidator<Transportistum> validator)
        {
            _transportistaRepository = transportistaRepository;
            _validator = validator;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly ITransportistaRepository _transportistaRepository;

        public TransportistaService(ITransportistaRepository transportistaRepository)
        {
            _transportistaRepository = transportistaRepository;
        }*/

        public async Task<ApiResponse<List<Transportistum>>> ListarTransportistasAsync()
        {
            var listaTransportistas = await _transportistaRepository.ListarTransportistasAsync();

            if (listaTransportistas == null || listaTransportistas.Count == 0)
                return new ApiResponse<List<Transportistum>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaTransportistas };

            return new ApiResponse<List<Transportistum>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaTransportistas };
        }

        public async Task<ApiResponse<Paginacion<Transportistum>>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _transportistaRepository.ListarTransportistasPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return new ApiResponse<Paginacion<Transportistum>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<Transportistum>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Transportistum>> ObtenerTransportistaAsync(int idTranportista)
        {
            var transportista = await _transportistaRepository.ObtenerTransportistaAsync(idTranportista);

            if (transportista == null)
            {
                return new ApiResponse<Transportistum> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Transportistum> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = transportista };
        }

        public async Task<ApiResponse<object>> RegistrarTransportistaAsync(Transportistum transportista)
        {
            var validationResult = await _validator.ValidateAsync(transportista);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var transportistas = await _transportistaRepository.ListarTransportistasAsync();
            if (transportistas.Any(c => c.Codigo == transportista.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (transportistas.Any(c => c.Cedula == transportista.Cedula))
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe" };

            if (transportistas.Any(c => c.Telefono == transportista.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "El télefono ya existe" };

            var result = await _transportistaRepository.RegistrarTransportistaAsync(transportista);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarTransportistaAsync(Transportistum transportista)
        {
            if (transportista == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(transportista);
            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var transportistaExistente = await _transportistaRepository.ObtenerTransportistaAsync(transportista.Id_Transportista);
            if (transportistaExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var transportistas = await _transportistaRepository.ListarTransportistasAsync();
            if (transportistas.Any(c => c.Cedula == transportista.Cedula && c.Id_Transportista != transportista.Id_Transportista))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe." };
            }
            else if (transportistas.Any(c => c.Telefono == transportista.Telefono && c.Id_Transportista != transportista.Id_Transportista))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El teléfono ya existe." };
            }

            var result = await _transportistaRepository.EditarTransportistaAsync(transportista);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarTransportistaAsync(int id)
        {
            try
            {
                var existe = await _transportistaRepository.ObtenerTransportistaAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int>
                    { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _transportistaRepository.EliminarTransportistaAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar al transportista porque tiene compras asociadas." };
            }
        }

    }
}
