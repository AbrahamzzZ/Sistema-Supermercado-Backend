using DataBaseFirst.Models;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly ProveedorRepository _proveedorRepository;
        private readonly IValidator<Proveedor> _validator;

        public ProveedorService(ProveedorRepository proveedorRepository, IValidator<Proveedor> validator)
        {
            _proveedorRepository = proveedorRepository;
            _validator = validator;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IProveedorRepository _proveedorRepository;

        public ProveedorService(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }*/

        public async Task<ApiResponse<List<Proveedor>>> ListarProveedoresAsync()
        {
            var listaProveedores = await _proveedorRepository.ListarProveedoresAsync();

            if (listaProveedores == null || listaProveedores.Count == 0)
                return new ApiResponse<List<Proveedor>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaProveedores };

            return new ApiResponse<List<Proveedor>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaProveedores };
        }

        public async Task<ApiResponse<Paginacion<Proveedor>>> ListarProveedoresPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _proveedorRepository.ListarProveedoresPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<Proveedor>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<Proveedor>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Proveedor>> ObtenerProveedorAsync(int idProveedor)
        {
            var proveedor = await _proveedorRepository.ObtenerProveedorAsync(idProveedor);

            if (proveedor == null)
            {
                return new ApiResponse<Proveedor> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Proveedor> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = proveedor };
        }

        public async Task<ApiResponse<object>> RegistrarProveedorAsync(Proveedor proveedor)
        {
            var validationResult = await _validator.ValidateAsync(proveedor);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var categorias = await _proveedorRepository.ListarProveedoresAsync();
            if (categorias.Any(c => c.Codigo == proveedor.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (categorias.Any(c => c.Cedula == proveedor.Cedula))
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe" };

            if (categorias.Any(c => c.Telefono == proveedor.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "El télefono ya existe" };

            var result = await _proveedorRepository.RegistrarProveedorAsync(proveedor);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };

        }

        public async Task<ApiResponse<object>> EditarProveedorAsync(Proveedor proveedor)
        {
            if (proveedor == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(proveedor);
            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var proveedorExistente = await _proveedorRepository.ObtenerProveedorAsync(proveedor.Id_Proveedor);
            if (proveedorExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var clientes = await _proveedorRepository.ListarProveedoresAsync();
            if (clientes.Any(c => c.Cedula == proveedor.Cedula && c.Id_Proveedor != proveedor.Id_Proveedor))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe." };
            }
            else if (clientes.Any(c => c.Telefono == proveedor.Telefono && c.Id_Proveedor != proveedor.Id_Proveedor))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El teléfono ya existe." };
            }

            var result = await _proveedorRepository.EditarProveedorAsync(proveedor);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarProveedorAsync(int id)
        {
            try
            {
                var existe = await _proveedorRepository.ObtenerProveedorAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _proveedorRepository.EliminarProveedorAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar al proveedor porque tiene compras asociadas." };
            }
        }
    }
}
