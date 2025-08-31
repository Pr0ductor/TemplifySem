using Microsoft.AspNetCore.Mvc;
using MediatR;
using Templify.Application.Features.Dashboard.Queries;
using Templify.Application.Common.DTOs;
using Templify.mvc.Attributes;

namespace Templify.mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RequirePermission("dashboard.view")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IMediator mediator, ILogger<DashboardController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var stats = await _mediator.Send(new GetDashboardStatsQuery());
                
                ViewBag.TotalUsers = stats.TotalUsers;
                ViewBag.TotalAuthors = stats.TotalAuthors;
                ViewBag.TotalProducts = stats.TotalProducts;
                ViewBag.TotalSales = stats.TotalSales;
                ViewBag.TotalRevenue = stats.TotalRevenue;
                ViewBag.WeeklySales = stats.WeeklySales;
                ViewBag.TopCategories = stats.TopCategories;
                ViewBag.RecentActivities = stats.RecentActivities;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard statistics");
                
                // Возвращаем заглушки в случае ошибки
                ViewBag.TotalUsers = 0;
                ViewBag.TotalAuthors = 0;
                ViewBag.TotalProducts = 0;
                ViewBag.TotalSales = 0;
                ViewBag.TotalRevenue = 0;
                ViewBag.WeeklySales = new List<WeeklySalesDto>();
                ViewBag.TopCategories = new List<CategoryStatsDto>();
                ViewBag.RecentActivities = new List<RecentActivityDto>();

                return View();
            }
        }
    }
}
