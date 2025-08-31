using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Security.Claims;
using Templify.Application.Features.Products.Commands;
using Templify.Application.Features.Products.Queries;
using Templify.Application.Features.Users.Queries;
using Templify.Application.Interfaces.Services;
using Templify.mvc.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace Templify.mvc.Controllers;

[Authorize]
public class MyTemplatesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IAuthorService _authorService;
    private readonly ILogger<MyTemplatesController> _logger;

    public MyTemplatesController(
        IMediator mediator,
        IAuthorService authorService,
        ILogger<MyTemplatesController> logger)
    {
        _mediator = mediator;
        _authorService = authorService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActiveTab"] = "profile";
        
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId))
        {
            return RedirectToAction("Index", "Auth");
        }

        // Check if user is an author
        var isAuthor = await _authorService.IsUserAuthorAsync(identityId);
        if (!isAuthor)
        {
            return RedirectToAction("Index", "Profile");
        }

        // Get author profile
        var author = await _authorService.GetAuthorByUserIdAsync(identityId);
        if (author == null)
        {
            return RedirectToAction("Index", "Profile");
        }

        // Get products created by this author
        var products = await _mediator.Send(new GetProductsByAuthorQuery { AuthorId = author.Id });

        var viewModel = new MyTemplatesViewModel
        {
            Author = author,
            Products = products.ToList()
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId))
        {
            return RedirectToAction("Index", "Auth");
        }

        // Verify user is author of this product
        var isAuthor = await _authorService.IsUserAuthorOfProductAsync(identityId, id);
        if (!isAuthor)
        {
            return RedirectToAction("Index");
        }

        // Get product details
        var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
        if (product == null)
        {
            return NotFound();
        }

        var editModel = new EditProductModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            Tags = product.Tags,
            ImageUrl = product.ImageUrl
        };

        return View(editModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditProductModel model, IFormFile? newImage)
    {
        try
        {
            _logger.LogInformation("Edit POST method called for product ID: {ProductId}", model.Id);
            
            var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identityId))
            {
                _logger.LogWarning("User not authenticated");
                return RedirectToAction("Index", "Auth");
            }

            _logger.LogInformation("User ID: {UserId}, ModelState valid: {IsValid}", identityId, ModelState.IsValid);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid: {Errors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return View(model);
            }

            // Handle new image upload if provided
            string imageUrl = model.ImageUrl; // Keep current image by default
            if (newImage != null && newImage.Length > 0)
            {
                _logger.LogInformation("Processing new image upload: {FileName}, Size: {Size} bytes", 
                    newImage.FileName, newImage.Length);
                
                // Validate file size (5MB max)
                if (newImage.Length > 5 * 1024 * 1024)
                {
                    _logger.LogWarning("Image file too large: {Size} bytes", newImage.Length);
                    ModelState.AddModelError("", "Image file size must be less than 5MB");
                    return View(model);
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(newImage.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    _logger.LogWarning("Invalid file type: {Extension}", fileExtension);
                    ModelState.AddModelError("", "Only JPG, PNG and GIF files are allowed");
                    return View(model);
                }

                // Get web host environment from HttpContext
                var webHostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Created uploads directory: {Path}", uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(fileStream);
                }

                imageUrl = "/uploads/products/" + uniqueFileName;
                _logger.LogInformation("New image uploaded successfully: {ImageUrl}", imageUrl);
            }

            var command = new UpdateProductCommand
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Category = model.Category,
                Tags = model.Tags,
                ImageUrl = imageUrl,
                UserId = identityId
            };

            _logger.LogInformation("Sending UpdateProductCommand: {Command}", 
                System.Text.Json.JsonSerializer.Serialize(command));

            var result = await _mediator.Send(command);
            _logger.LogInformation("UpdateProductCommand result: {Result}", result);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to update product. Please try again.");
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Edit POST method for product ID: {ProductId}", model.Id);
            ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId))
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        var command = new DeleteProductCommand
        {
            ProductId = id,
            UserId = identityId
        };

        var result = await _mediator.Send(command);
        if (result)
        {
            return Json(new { success = true, message = "Product deleted successfully" });
        }

        return Json(new { success = false, message = "Failed to delete product" });
    }
}
