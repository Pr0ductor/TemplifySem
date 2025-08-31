using Microsoft.AspNetCore.Identity;
using Templify.Domain.Entities;
using Templify.Domain.Enums;

namespace Templify.Persistence.Data
{
    /// <summary>
    /// Инициализатор администраторов и ролей
    /// 
    /// После запуска приложения автоматически создаются следующие учетные записи:
    /// 
    /// 🔐 Администратор (Admin)
    /// Email: admin@templify.com
    /// Пароль: Admin123!
    /// Роль: Admin
    /// Возможности: Полный доступ к системе (создание, редактирование, удаление)
    /// 
    /// 👨‍💼 Менеджер 1 (Manager)
    /// Email: manager1@templify.com
    /// Пароль: Manager1!
    /// Роль: Manager
    /// Возможности: Ограниченный доступ (создание, редактирование, НЕ удаление)
    /// 
    /// 👩‍💼 Менеджер 2 (Manager)
    /// Email: manager2@templify.com
    /// Пароль: Manager2!
    /// Роль: Manager
    /// Возможности: Ограниченный доступ (создание, редактирование, НЕ удаление)
    /// 
    /// Для доступа к админ-панели используйте любой из этих аккаунтов.
    /// 
    /// 🔒 Безопасность: Админ-панель скрыта от неавторизованных пользователей.
    /// При попытке доступа без нужных ролей возвращается 404 ошибка.
    /// 
    /// 👥 Разграничение прав:
    /// - Admin: полный доступ ко всем функциям
    /// - Manager: может создавать и редактировать, но НЕ удалять записи
    /// </summary>
    public static class AdminInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            // Создаем роли если их нет
            string[] roles = { "Admin", "Manager", "User" };
            
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            // Создаем администратора
            var adminEmail = "admin@templify.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Создаем менеджера 1
            var manager1Email = "manager1@templify.com";
            var manager1User = await userManager.FindByEmailAsync(manager1Email);
            
            if (manager1User == null)
            {
                manager1User = new ApplicationUser
                {
                    UserName = "manager1",
                    Email = manager1Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                var result = await userManager.CreateAsync(manager1User, "Manager1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager1User, "Manager");
                }
            }

            // Создаем менеджера 2
            var manager2Email = "manager2@templify.com";
            var manager2User = await userManager.FindByEmailAsync(manager2Email);
            
            if (manager2User == null)
            {
                manager2User = new ApplicationUser
                {
                    UserName = "manager2",
                    Email = manager2Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                var result = await userManager.CreateAsync(manager2User, "Manager2!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager2User, "Manager");
                }
            }
        }
    }
}
