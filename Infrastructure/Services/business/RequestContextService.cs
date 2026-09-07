using Infrastructure.Repository.InterfacesBusiness;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.business
{
    public class RequestContextService : IRequestContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetEndpoint()
        {
            return _httpContextAccessor.HttpContext?.Request.Path.ToString() ?? "UNKNOWN";
        }

        public string GetMethod()
        {
            return _httpContextAccessor.HttpContext?.Request.Method ?? "UNKNOWN";
        }
    }
}
