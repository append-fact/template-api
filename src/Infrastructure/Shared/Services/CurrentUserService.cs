using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            // Probablemente se está inicializando la aplicación.
            if (_httpContextAccessor is null || _httpContextAccessor.HttpContext is null)
            {
                User = new CurrentUser(Guid.Empty.ToString(), string.Empty, false, new List<Claim>());

                return;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext!.User!.Identity!.IsAuthenticated == false)
            {
                User = new CurrentUser(Guid.Empty.ToString(), string.Empty, false, new List<Claim>());

                return;
            }

            var id = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? string.Empty;

            var userName = httpContext.User!.Identity!.Name ?? "Unknown";
            var claims = httpContext.User.Claims.ToList();

            User = new CurrentUser(id, userName, true, claims);
        }

        public CurrentUser User { get; }

        public bool IsInRole(string roleName) =>
            _httpContextAccessor!.HttpContext!.User.IsInRole(roleName);
    }
}
