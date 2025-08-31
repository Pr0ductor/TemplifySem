using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Templify.Domain.Entities;

namespace Templify.mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRoleAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public RequireRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.GetUserAsync(context.HttpContext.User);

            if (user == null)
            {
                // Если пользователь не авторизован - возвращаем 404
                context.Result = new NotFoundResult();
                return;
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var hasRequiredRole = _roles.Any(role => userRoles.Contains(role));

            if (!hasRequiredRole)
            {
                // Если у пользователя нет нужных ролей - возвращаем 404
                context.Result = new NotFoundResult();
                return;
            }
        }
    }
}
