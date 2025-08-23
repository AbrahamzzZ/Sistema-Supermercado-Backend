using DataBaseFirst.Models;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly ProveedorRepository _proveedorRepository;

        public ProveedorService(ProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
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
            if (proveedor == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(proveedor.Codigo) || string.IsNullOrWhiteSpace(proveedor.Nombres) || string.IsNullOrWhiteSpace(proveedor.Apellidos) || string.IsNullOrWhiteSpace(proveedor.Cedula) || string.IsNullOrWhiteSpace(proveedor.Telefono) || string.IsNullOrWhiteSpace(proveedor.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(proveedor.Nombres) || !regex.IsMatch(proveedor.Apellidos))
                return new ApiResponse<object> { IsSuccess = false, Message = "Los nombres y apellidos solo puede contener letras y espacios" };

            var regexCedula = new Regex(@"^\d{10}$");
            if (!regexCedula.IsMatch(proveedor.Cedula) || !regexCedula.IsMatch(proveedor.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "La cédula y el teléfono deben contener exactamente 10 dígitos numéricos" };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(proveedor.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var categorias = await _proveedorRepository.ListarProveedoresAsync();
            if (categorias.Any(c => c.Codigo == proveedor.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (categorias.Any(c => c.Cedula?.ToLower() == proveedor.Cedula.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe" };

            if (categorias.Any(c => c.Telefono?.ToLower() == proveedor.Telefono.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El télefono ya existe" };

            var result = await _proveedorRepository.RegistrarProveedorAsync(proveedor);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };

        }

        public async Task<ApiResponse<object>> EditarProveedorAsync(Proveedor proveedor)
        {
            if (proveedor == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(proveedor.Nombres) || string.IsNullOrWhiteSpace(proveedor.Apellidos) || string.IsNullOrWhiteSpace(proveedor.Cedula) || string.IsNullOrWhiteSpace(proveedor.Telefono) || string.IsNullOrWhiteSpace(proveedor.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var clienteExistente = await _proveedorRepository.ObtenerProveedorAsync(proveedor.Id_Proveedor);
            if (clienteExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(proveedor.Nombres) || !regex.IsMatch(proveedor.Apellidos))
                return new ApiResponse<object> { IsSuccess = false, Message = "Los nombres y apellidos solo puede contener letras y espacios" };

            var regexCedula = new Regex(@"^\d{10}$");
            if (!regexCedula.IsMatch(proveedor.Cedula) || !regexCedula.IsMatch(proveedor.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "La cédula y el teléfono deben contener exactamente 10 dígitos numéricos" };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(proveedor.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var clientes = await _proveedorRepository.ListarProveedoresAsync();
            if (clientes.Any(c => c.Cedula == proveedor.Cedula && c.Id_Proveedor != proveedor.Id_Proveedor) || clientes.Any(c => c.Telefono == proveedor.Telefono && c.Id_Proveedor != proveedor.Id_Proveedor))
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
