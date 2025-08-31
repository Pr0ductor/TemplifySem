using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Templify.Application.Features.Products.Commands;
using Templify.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Templify.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Templify.mvc.Controllers;

[Authorize]
public class SubmitYourTemplateController : Controller
{
    private readonly IMediator _mediator;
    private readonly IAuthorService _authorService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<SubmitYourTemplateController> _logger;

    public SubmitYourTemplateController(
        IMediator mediator, 
        IAuthorService authorService,
        IWebHostEnvironment webHostEnvironment,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<SubmitYourTemplateController> logger)
    {
        _mediator = mediator;
        _authorService = authorService;
        _webHostEnvironment = webHostEnvironment;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CreateProductCommand command, IFormFile? imageFile)
    {
        try
        {
            // Get current user ID
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User not authenticated");
                ModelState.AddModelError("", "User not authenticated");
                return View(command);
            }

            // Handle image upload
            string imageUrl = "/src/img/tem1.png"; // Default image
            if (imageFile != null && imageFile.Length > 0)
            {
                // Validate file size (5MB max)
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Image file size must be less than 5MB");
                    return View(command);
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("", "Only JPG, PNG and GIF files are allowed");
                    return View(command);
                }

                _logger.LogInformation("Processing image upload: {FileName}, Size: {Size} bytes", imageFile.FileName, imageFile.Length);
                
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Created uploads directory: {Path}", uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                imageUrl = "/uploads/products/" + uniqueFileName;
                _logger.LogInformation("Image uploaded successfully: {ImageUrl}", imageUrl);
            }

            // Set the user ID and image URL for the command
            command = command with { UserId = userId, ImageUrl = imageUrl };
            _logger.LogInformation("Command prepared: Name={Name}, Category={Category}, Price={Price}", command.Name, command.Category, command.Price);

            // Validate command using DataAnnotations
            var validationContext = new ValidationContext(command);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(command, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                    }
                }
                return View(command);
            }

            // Create the product (this will also create author if needed)
            var productId = await _mediator.Send(command);
            _logger.LogInformation("Product created successfully with ID: {ProductId}", productId);

            // Update user claims in current session to include Author role
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Refresh the user's sign-in to update claims
                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation("User sign-in refreshed for role update");
            }

            TempData["SuccessMessage"] = "Product submitted successfully! You are now an author!";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting product for user {UserId}: {Message}", User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, ex.Message);
            ModelState.AddModelError("", $"Error submitting product: {ex.Message}");
            return View(command);
        }
    }
}
