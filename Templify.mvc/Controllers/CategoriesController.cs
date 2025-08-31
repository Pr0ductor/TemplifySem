using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Templify.mvc.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediatr;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMediator mediatr, ILogger<CategoriesController> logger)
        {
            _mediatr = mediatr;
            _logger = logger;
        }


        public IActionResult Index()
        {
            ViewData["ActiveTab"] = "categories";
            return View();
        }

        public IActionResult Category(string categoryName)
        {
            ViewData["ActiveTab"] = "categories";
            ViewData["CategoryName"] = categoryName;
            
            // Перенаправляем на страницу продуктов с фильтром по категории
            return RedirectToAction("Index", "Products", new { category = categoryName });
        }
    }
}
           
