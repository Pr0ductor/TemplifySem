using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Templify.mvc.Models;
using Templify.Domain.Enums;
using System.ComponentModel;
using Templify.Application.Common.DTOs;
using System;
using System.Security.Claims;
using Templify.Application.Features.Users.Queries;
using Templify.Application.Features.ProductPurchases.Queries;
using Microsoft.AspNetCore.Authorization;
using Templify.Application.Features.Products.Queries;
using Templify.Application.Features.ProductPurchases.Commands;

namespace Templify.mvc.Controllers
{
    public class PurchaseRequest
    {
        public int ProductId { get; set; }
    }

    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;
        
        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? category, string? search)
        {
            ViewData["ActiveTab"] = "products";
            
            var searchQuery = new SearchProductsQuery
            {
                SearchTerm = search,
                CategoryType = !string.IsNullOrEmpty(category) ? GetCategoryTypeByDisplayName(category) : null
            };
            
            var allProducts = await _mediator.Send(searchQuery);
            var purchasedProducts = new List<ProductDto>();
            
            // Получаем купленные продукты для аутентифицированного пользователя
            if (User.Identity?.IsAuthenticated == true)
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(identityId))
                {
                    var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
                    if (appUserId.HasValue)
                    {
                        var purchasedProductIds = await _mediator.Send(new Application.Features.ProductPurchases.Queries.GetPurchasedProductIdsQuery { AppUserId = appUserId.Value });
                        
                        purchasedProducts = allProducts
                            .Where(p => purchasedProductIds.Contains(p.Id))
                            .ToList();
                        
                        // Убираем купленные продукты из общего списка
                        allProducts = allProducts
                            .Where(p => !purchasedProductIds.Contains(p.Id))
                            .ToList();
                    }
                }
            }
            
            var vm = new ProductListViewModel
            {
                Products = allProducts,
                PurchasedProducts = purchasedProducts
            };
            return View(vm);
        }

        public async Task<IActionResult> Product(int id)
        {
            ViewData["ActiveTab"] = "product";
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null)
                return NotFound();
            var vm = new ProductViewModel
            {
                Product = product
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search, string? category)
        {
            var searchQuery = new SearchProductsQuery
            {
                SearchTerm = search,
                CategoryType = !string.IsNullOrEmpty(category) ? GetCategoryTypeByDisplayName(category) : null
            };
            
            var allProducts = await _mediator.Send(searchQuery);
            var purchasedProducts = new List<ProductDto>();
            
            // Получаем купленные продукты для аутентифицированного пользователя
            if (User.Identity?.IsAuthenticated == true)
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(identityId))
                {
                    var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
                    if (appUserId.HasValue)
                    {
                        var purchasedProductIds = await _mediator.Send(new Application.Features.ProductPurchases.Queries.GetPurchasedProductIdsQuery { AppUserId = appUserId.Value });
                        
                        purchasedProducts = allProducts
                            .Where(p => purchasedProductIds.Contains(p.Id))
                            .ToList();
                        
                        // Убираем купленные продукты из общего списка
                        allProducts = allProducts
                            .Where(p => !purchasedProductIds.Contains(p.Id))
                            .ToList();
                    }
                }
            }
            
            return PartialView("_ProductListPartial", new ProductListViewModel
            {
                Products = allProducts,
                PurchasedProducts = purchasedProducts
            });
        }

        [HttpPost]
        public async Task<IActionResult> Purchase([FromBody] PurchaseRequest request)
        {
            try
            {
                Console.WriteLine($"Purchase request received: {request?.ProductId}");
                Console.WriteLine($"Request object: {request != null}");
                
                if (request == null)
                {
                    Console.WriteLine("Request is null");
                    return Json(new { success = false, error = "Invalid request data" });
                }
                
                Console.WriteLine($"Purchase attempt for product {request.ProductId}");
                
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"IdentityId: {identityId}");
                
                if (string.IsNullOrEmpty(identityId)) 
                {
                    Console.WriteLine("User not authorized - no identity ID");
                    return Json(new { success = false, error = "User not authorized" });
                }
                
                var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
                Console.WriteLine($"AppUser found: {appUserId != null}, AppUserId: {appUserId}");
                
                if (appUserId == null) 
                {
                    Console.WriteLine("AppUser not found");
                    return Json(new { success = false, error = "AppUser not found" });
                }
                
                var product = await _mediator.Send(new GetProductByIdQuery { Id = request.ProductId });
                Console.WriteLine($"Product found: {product != null}, ProductId: {product?.Id}");
                
                if (product == null) 
                {
                    Console.WriteLine("Product not found");
                    return Json(new { success = false, error = "Product not found" });
                }
                
                var command = new Templify.Application.Features.ProductPurchases.Commands.PurchaseProductCommand
                {
                    ProductId = request.ProductId,
                    AppUserId = appUserId.Value
                };
                
                Console.WriteLine($"Sending command: ProductId={command.ProductId}, AppUserId={command.AppUserId}");
                
                var result = await _mediator.Send(command);
                Console.WriteLine($"MediatR result: {result}");
                
                if (result)
                {
                    Console.WriteLine("Purchase successful");
                    return Json(new { success = true, message = "Product purchased successfully!" });
                }
                else
                {
                    Console.WriteLine("Purchase failed - result is false");
                    return Json(new { success = false, error = "Product already purchased or purchase failed" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Purchase: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return Json(new { success = false, error = $"Exception: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelPurchase(int productId)
        {
            try
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(identityId)) 
                {
                    return Json(new { success = false, error = "User not authorized" });
                }
                
                var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
                if (appUserId == null) 
                {
                    return Json(new { success = false, error = "AppUser not found" });
                }
                
                var result = await _mediator.Send(new CancelPurchaseCommand
                {
                    ProductId = productId,
                    AppUserId = appUserId.Value
                });
                
                if (result)
                {
                    return Json(new { success = true, message = "Purchase cancelled successfully!" });
                }
                else
                {
                    return Json(new { success = false, error = "Purchase not found or cancellation failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"Exception: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> IsAuthorOfProduct(int productId)
        {
            try
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(identityId)) 
                {
                    return Json(new { success = false, isAuthor = false });
                }
                
                var product = await _mediator.Send(new GetProductByIdQuery { Id = productId });
                if (product == null) 
                {
                    return Json(new { success = false, isAuthor = false });
                }
                
                // Проверяем, является ли текущий пользователь автором продукта
                var isAuthor = product.AuthorId > 0 && await _mediator.Send(new Application.Features.Authors.Queries.IsUserAuthorOfProductQuery 
                { 
                    UserId = identityId, 
                    ProductId = productId 
                });
                
                return Json(new { success = true, isAuthor });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"Exception: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> HasPurchased(int productId)
        {
            try
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(identityId)) 
                {
                    return Json(new { success = false, error = "User not authorized" });
                }
                
                var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
                if (appUserId == null) 
                {
                    return Json(new { success = false, error = "AppUser not found" });
                }
                
                var hasPurchased = await _mediator.Send(new HasPurchasedQuery
                {
                    ProductId = productId,
                    AppUserId = appUserId.Value
                });
                
                return Json(new { success = true, hasPurchased });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"Exception: {ex.Message}" });
            }
        }

        private CategoryType? GetCategoryTypeByDisplayName(string displayName)
        {
            // Маппинг Display Names к enum значениям
            var categoryMapping = new Dictionary<string, CategoryType>(StringComparer.OrdinalIgnoreCase)
            {
                { "Business", CategoryType.Business },
                { "3D Web", CategoryType.ThreeDWeb },
                { "Saas Platforms", CategoryType.SaasPlatforms },
                { "Agency", CategoryType.Agency },
                { "Portfolio Design", CategoryType.PortfolioDesign },
                { "Ecommerce", CategoryType.Ecommerce },
                { "Education", CategoryType.Education },
                { "Health", CategoryType.Health },
                { "Marketing", CategoryType.Marketing },
                { "Restaurant & Food", CategoryType.RestaurantAndFood },
                { "Gaming & Entertainment", CategoryType.GamingAndEntertainment },
                { "Real Estate", CategoryType.RealEstate }
            };

            return categoryMapping.TryGetValue(displayName, out var categoryType) ? categoryType : null;
        }
    }
}

              
