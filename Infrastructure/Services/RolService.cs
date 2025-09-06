using DataBaseFirst.Models;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class RolService : IRolService
    {
        private readonly RolRepository _rolesRepository;

        public RolService(RolRepository rolRepository)
        {
            _rolesRepository = rolRepository;
        }

        public async Task<ApiResponse<List<Rol>>> ListarRolesAsync()
        {
            var listaRoles = await _rolesRepository.ListarRolesAsync();

            if(listaRoles == null || listaRoles.Count == 0)
                return new ApiResponse<List<Rol>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaRoles };

            return new ApiResponse<List<Rol>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaRoles };
        }

    }
}
