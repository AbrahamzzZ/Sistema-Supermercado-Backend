using Domain.Models;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class MenuService : IMenuService
    {
        private readonly MenuRepository _menusRepository;
        private readonly IAuditoriaService _auditoriaService;

        public MenuService(MenuRepository menuRepository, IAuditoriaService auditoriaService)
        {
            _menusRepository = menuRepository;
            _auditoriaService = auditoriaService;
        }

        public async Task<ApiResponse<List<Menu>>> ObtenerMenusAsync(int idUsuario)
        {
            try
            {
                var obtenerMenus = await _menusRepository.ObtenerMenusAsync(idUsuario);

                if (obtenerMenus == null || obtenerMenus.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Menús", $"No hay menús disponibles para el usuario {idUsuario}");

                    return new ApiResponse<List<Menu>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = obtenerMenus  };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Menús", $"Se obtuvieron {obtenerMenus.Count} menús para el usuario {idUsuario}");

                return new ApiResponse<List<Menu>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = obtenerMenus };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Menús", ex);
                throw;
            }
        }
    }
}
