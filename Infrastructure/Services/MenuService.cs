using Domain.Models;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class MenuService : IMenuService
    {
        private readonly MenuRepository _menusRepository;

        public MenuService(MenuRepository menuRepository)
        {
            _menusRepository = menuRepository;
        }

        public async Task<ApiResponse<List<Menu>>> ObtenerMenusAsync(int idUsuario)
        {
            var obtenerMenus = await _menusRepository.ObtenerMenusAsync(idUsuario);

            if (obtenerMenus == null || obtenerMenus.Count == 0)
                return new ApiResponse<List<Menu>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = obtenerMenus};

            return new ApiResponse<List<Menu>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = obtenerMenus };
        }
    }
}
