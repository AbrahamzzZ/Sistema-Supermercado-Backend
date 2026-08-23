using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No se pudo obtener el ID del usuario autenticado.");
            }

            return userId;
        }
    }
}
