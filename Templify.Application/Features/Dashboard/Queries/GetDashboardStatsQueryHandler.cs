using MediatR;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Dashboard.Queries;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IGenericRepository<AppUser> _userRepository;
    private readonly IGenericRepository<Author> _authorRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductPurchase> _purchaseRepository;

    public GetDashboardStatsQueryHandler(
        IGenericRepository<AppUser> userRepository,
        IGenericRepository<Author> authorRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<ProductPurchase> purchaseRepository)
    {
        _userRepository = userRepository;
        _authorRepository = authorRepository;
        _productRepository = productRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var stats = new DashboardStatsDto();

        // Получаем базовую статистику
        stats.TotalUsers = await _userRepository.Entities.CountAsync(cancellationToken);
        stats.TotalAuthors = await _authorRepository.Entities.CountAsync(cancellationToken);
        stats.TotalProducts = await _productRepository.Entities.CountAsync(cancellationToken);
        stats.TotalSales = await _purchaseRepository.Entities.CountAsync(cancellationToken);

        // Получаем общую выручку
        var products = await _productRepository.Entities.ToListAsync(cancellationToken);
        var purchases = await _purchaseRepository.Entities.ToListAsync(cancellationToken);
        
        stats.TotalRevenue = purchases.Sum(p => 
            products.FirstOrDefault(prod => prod.Id == p.ProductId)?.Price ?? 0);

        // Получаем продажи за последние 7 дней
        var weekAgo = DateTime.UtcNow.AddDays(-7);
        var weeklyPurchases = await _purchaseRepository.Entities
            .Where(p => p.PurchasedAt >= weekAgo)
            .ToListAsync(cancellationToken);

        var daysOfWeek = new[] { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };
        for (int i = 0; i < 7; i++)
        {
            var dayStart = weekAgo.AddDays(i);
            var dayEnd = dayStart.AddDays(1);
            
            var dayPurchases = weeklyPurchases.Where(p => 
                p.PurchasedAt >= dayStart && p.PurchasedAt < dayEnd).ToList();
            
            stats.WeeklySales.Add(new WeeklySalesDto
            {
                Day = daysOfWeek[i],
                Sales = dayPurchases.Count,
                Revenue = dayPurchases.Sum(p => 
                    products.FirstOrDefault(prod => prod.Id == p.ProductId)?.Price ?? 0)
            });
        }

        // Получаем топ категории
        var categoryStats = await _productRepository.Entities
            .GroupBy(p => p.Category)
            .Select(g => new CategoryStatsDto
            {
                Category = g.Key,
                ProductCount = g.Count(),
                Percentage = 0 // Будет рассчитано ниже
            })
            .OrderByDescending(c => c.ProductCount)
            .Take(3)
            .ToListAsync(cancellationToken);

        // Рассчитываем проценты
        if (stats.TotalProducts > 0)
        {
            foreach (var category in categoryStats)
            {
                category.Percentage = Math.Round((decimal)category.ProductCount / stats.TotalProducts * 100, 1);
            }
        }

        stats.TopCategories = categoryStats;

        // Получаем последние действия (заглушка для демонстрации)
        stats.RecentActivities = new List<RecentActivityDto>
        {
            new RecentActivityDto
            {
                Type = "new-user",
                Description = "Новый пользователь зарегистрировался",
                UserName = "john_doe",
                Timestamp = DateTime.UtcNow.AddMinutes(-2),
                IconClass = "new-user"
            },
            new RecentActivityDto
            {
                Type = "new-product",
                Description = "Автор опубликовал новый продукт",
                UserName = "design_master",
                Timestamp = DateTime.UtcNow.AddMinutes(-15),
                IconClass = "new-product"
            },
            new RecentActivityDto
            {
                Type = "new-sale",
                Description = "Продажа продукта за $29",
                UserName = "Premium Template",
                Timestamp = DateTime.UtcNow.AddHours(-1),
                IconClass = "new-sale"
            },
            new RecentActivityDto
            {
                Type = "new-author",
                Description = "Новый автор присоединился к платформе",
                UserName = "code_wizard",
                Timestamp = DateTime.UtcNow.AddHours(-3),
                IconClass = "new-author"
            }
        };

        return stats;
    }
}
