using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _requiredPermission;

        public PermissionAuthorizationAttribute(string requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var rolesService = context.HttpContext.RequestServices.GetRequiredService<IRolesService>();

            var hasPermission = await rolesService.UserHasPermissionAsync(_requiredPermission);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
