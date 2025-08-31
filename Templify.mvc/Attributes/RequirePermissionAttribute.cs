using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Templify.Domain.Entities;

namespace Templify.mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _permissions;

        public RequirePermissionAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.GetUserAsync(context.HttpContext.User);

            if (user == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            var userRoles = await userManager.GetRolesAsync(user);
            
            // Admin имеет все права
            if (userRoles.Contains("Admin"))
            {
                return;
            }

            // Проверяем права для Manager
            var hasPermission = false;
            foreach (var permission in _permissions)
            {
                if (HasManagerPermission(userRoles, permission))
                {
                    hasPermission = true;
                    break;
                }
            }

            if (!hasPermission)
            {
                context.Result = new NotFoundResult();
                return;
            }
        }

        private bool HasManagerPermission(IList<string> userRoles, string permission)
        {
            if (!userRoles.Contains("Manager"))
                return false;

            // Определяем права для Manager
            return permission switch
            {
                "users.view" => true,           // Менеджер может просматривать пользователей
                "users.edit" => false,          // Но не может редактировать
                "authors.view" => true,         // Менеджер может просматривать авторов
                "authors.edit" => true,         // И редактировать их
                "authors.create" => true,       // И создавать новых
                "authors.delete" => false,      // Но не может удалять
                "products.view" => true,        // Менеджер может просматривать продукты
                "products.edit" => true,        // И редактировать их
                "products.create" => true,      // И создавать новые
                "products.delete" => false,     // Но не может удалять
                "purchases.view" => true,       // Менеджер может просматривать покупки
                "purchases.edit" => true,       // И редактировать их
                "purchases.create" => true,     // И создавать новые
                "purchases.delete" => false,    // Но не может удалять
                "subscriptions.view" => true,   // Менеджер может просматривать подписки
                "subscriptions.edit" => true,   // И редактировать их
                "subscriptions.create" => true, // И создавать новые
                "subscriptions.delete" => false,// Но не может удалять
                "dashboard.view" => true,       // Менеджер может видеть дашборд
                _ => false
            };
        }
    }
}
