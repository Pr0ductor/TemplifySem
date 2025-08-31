using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.Extensions.Logging;

using Templify.Domain.Enums;
using Templify.mvc.Models;
using Templify.Application.Features.Products.Queries;
using Templify.Application.Features.Authors.Queries;

namespace Templify.mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMediator mediator, ILogger<HomeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveTab"] = "home";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // Категории (6 первых)
                var categories = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().Take(6).ToList();

                // Продукты (12 первых)
                var products = await _mediator.Send(new GetAllProductsQuery());
                var topProducts = products.Take(12).ToList();

                // Авторы (6 первых)
                var authors = await _mediator.Send(new GetAllAuthorsQuery());
                var topAuthors = authors.Take(6).ToList();

                var vm = new HomeViewModel
                {
                    Categories = categories,
                    Products = topProducts,
                    Authors = topAuthors
                };

                stopwatch.Stop();
                
                // Логируем успешную загрузку главной страницы
                _logger.LogInformation("Home page loaded successfully in {LoadTime}ms. Categories: {CategoriesCount}, Products: {ProductsCount}, Authors: {AuthorsCount}", 
                    stopwatch.ElapsedMilliseconds, categories.Count, topProducts.Count, topAuthors.Count);

                return View(vm);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error loading home page: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}
