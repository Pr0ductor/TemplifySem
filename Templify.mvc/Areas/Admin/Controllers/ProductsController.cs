using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using Templify.Application.Features.Products.Queries;
using Templify.Application.Features.Products.Commands;
using Templify.Application.Features.Authors.Queries;
using Templify.Application.Common.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Templify.mvc.Attributes;

namespace Templify.mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RequirePermission("products.view", "products.edit", "products.create")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string search = "", string category = "")
        {
            try
            {
                var products = await _mediator.Send(new GetAllProductsQuery());
                var sorted = products.OrderBy(p => p.Id).ToList();
                
                // Передаем параметры фильтров в ViewBag для восстановления состояния
                ViewBag.SearchTerm = search;
                ViewBag.CategoryFilter = category;
                
                return View(sorted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products list");
                return View(new List<ProductDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Получаем список авторов для выпадающего списка
            try
            {
                var authors = await _mediator.Send(new GetAllAuthorsQuery());
                ViewBag.Authors = authors.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting authors list");
                ViewBag.Authors = new List<SelectListItem>();
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command, IFormFile? productImage)
        {
            _logger.LogInformation("Create POST received - Name: {Name}, Category: {Category}, AuthorId: {AuthorId}", 
                command.Name, command.Category, command.AuthorId);
            
            if (!ModelState.IsValid)
            {
                // Повторно получаем список авторов при ошибке валидации
                try
                {
                    var authors = await _mediator.Send(new GetAllAuthorsQuery());
                    ViewBag.Authors = authors.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting authors list");
                    ViewBag.Authors = new List<SelectListItem>();
                }
                
                return View(command);
            }
            
            try
            {
                // Обработка загрузки изображения
                if (productImage != null && productImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productImage.CopyToAsync(fileStream);
                    }

                    command = command with { ImageUrl = "/uploads/products/" + uniqueFileName };
                    _logger.LogInformation("Image uploaded: {ImageUrl}", command.ImageUrl);
                }
                else if (string.IsNullOrEmpty(command.ImageUrl))
                {
                    // Оставляем ImageUrl пустым, если не загружено изображение и не указан URL
                    command = command with { ImageUrl = string.Empty };
                    _logger.LogInformation("No image provided, ImageUrl set to empty");
                }
                
                // Для админки используем AuthorId как UserId
                // Это позволит системе найти или создать автора для этого "пользователя"
                command = command with { UserId = command.AuthorId.ToString() };
                
                var productId = await _mediator.Send(command);
                _logger.LogInformation("Product created successfully with ID: {ProductId}", productId);
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError("", "Ошибка при создании продукта: " + ex.Message);
                
                // Повторно получаем список авторов при ошибке
                try
                {
                    var authors = await _mediator.Send(new GetAllAuthorsQuery());
                    ViewBag.Authors = authors.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    }).ToList();
                }
                catch (Exception authEx)
                {
                    _logger.LogError(authEx, "Error getting authors list");
                    ViewBag.Authors = new List<SelectListItem>();
                }
                
                return View(command);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
                if (product == null) return NotFound();
                
                // Получаем список авторов для выпадающего списка
                var authors = await _mediator.Send(new GetAllAuthorsQuery());
                ViewBag.Authors = authors.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();
                
                var command = new UpdateProductCommand
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    AuthorId = product.AuthorId,
                    Category = product.Category,
                    Tags = product.Tags ?? string.Empty,
                    UserId = "admin", // TODO: Получить реального пользователя
                    IsAdmin = true
                };
                
                return View(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product for edit");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductCommand command, IFormFile? newImage)
        {
            _logger.LogInformation("Edit POST received - ProductId: {ProductId}, Name: {Name}, Category: {Category}, AuthorId: {AuthorId}", 
                command.Id, command.Name, command.Category, command.AuthorId);
            
            if (!ModelState.IsValid)
            {
                // Повторно получаем список авторов при ошибке валидации
                try
                {
                    var authors = await _mediator.Send(new GetAllAuthorsQuery());
                    ViewBag.Authors = authors.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting authors list");
                    ViewBag.Authors = new List<SelectListItem>();
                }
                
                return View(command);
            }
            
            try
            {
                // Если загружено новое изображение, сохраняем его и обновляем ImageUrl
                if (newImage != null && newImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newImage.CopyToAsync(fileStream);
                    }
                    command = command with { ImageUrl = "/uploads/products/" + uniqueFileName };
                    _logger.LogInformation("New image uploaded: {ImageUrl}", "/uploads/products/" + uniqueFileName);
                }
                // Создаем новую команду с флагом IsAdmin = true
                var adminCommand = command with { IsAdmin = true };
                _logger.LogInformation("Sending admin command with IsAdmin = true");
                
                var result = await _mediator.Send(adminCommand);
                _logger.LogInformation("Update result: {Result}", result);
                
                if (result)
                {
                    _logger.LogInformation("Product updated successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogWarning("Product update failed");
                    ModelState.AddModelError("", "Не удалось обновить продукт");
                    
                    // Повторно получаем список авторов при ошибке
                    try
                    {
                        var authors = await _mediator.Send(new GetAllAuthorsQuery());
                        ViewBag.Authors = authors.Select(a => new SelectListItem
                        {
                            Value = a.Id.ToString(),
                            Text = a.Name
                        }).ToList();
                    }
                    catch (Exception authEx)
                    {
                        _logger.LogError(authEx, "Error getting authors list");
                        ViewBag.Authors = new List<SelectListItem>();
                    }
                    
                    return View(command);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                
                // Повторно получаем список авторов при ошибке
                try
                {
                    var authors = await _mediator.Send(new GetAllAuthorsQuery());
                    ViewBag.Authors = authors.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    }).ToList();
                }
                catch (Exception authEx)
                {
                    _logger.LogError(authEx, "Error getting authors list");
                    ViewBag.Authors = new List<SelectListItem>();
                }
                
                return View(command);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteProductCommand { ProductId = id, UserId = "admin", IsAdmin = true });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return RedirectToAction("Index");
            }
        }
    }
}
