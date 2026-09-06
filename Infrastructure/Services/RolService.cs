using Domain.Models;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class RolService : IRolService
    {
        private readonly RolRepository _rolesRepository;
        private readonly IAuditoriaService _auditoriaService;

        public RolService(RolRepository rolRepository, IAuditoriaService auditoriaService)
        {
            _rolesRepository = rolRepository;
            _auditoriaService = auditoriaService;
        }

        public async Task<ApiResponse<List<Rol>>> ListarRolesAsync()
        {
            try
            {
                var listaRoles = await _rolesRepository.ListarRolesAsync();

                if (listaRoles == null || listaRoles.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Roles", "No hay roles disponibles");

                    return new ApiResponse<List<Rol>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaRoles };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Roles", $"Se obtuvieron {listaRoles.Count} roles");

                return new ApiResponse<List<Rol>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaRoles };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Roles", ex);
                throw;
            }
        }

    }
}
