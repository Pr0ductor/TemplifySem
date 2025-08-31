using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Templify.Application.Interfaces.Services;
using Templify.mvc.Models;
using Microsoft.Extensions.Configuration;

namespace Templify.mvc.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContactController> _logger;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ContactController(IMediator mediator, ILogger<ContactController> logger, IEmailService emailService, IConfiguration configuration)
        {
            _mediator = mediator;
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["ActiveTab"] = "contact";
            return View(new ContactModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactModel model)
        {
            ViewData["ActiveTab"] = "contact";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var subject = $"Support request from {model.FullName}";
                var html = $@"<div style='font-family: Inter, Arial, sans-serif;'>
<p><strong>Name:</strong> {System.Net.WebUtility.HtmlEncode(model.FullName)}</p>
<p><strong>Email:</strong> {System.Net.WebUtility.HtmlEncode(model.Email)}</p>
<p><strong>Message:</strong></p>
<p>{System.Net.WebUtility.HtmlEncode(model.Message).Replace("\n","<br/>")}</p>
</div>";
                var recipient = _configuration["Support:RecipientEmail"] ?? "shidomaeki@mail.ru";
                await _emailService.SendEmailAsync(recipient, subject, html);

                TempData["SuccessMessage"] = "Your request has been sent. We'll get back to you soon.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact form message");
                ModelState.AddModelError(string.Empty, "Failed to send your request. Please try again later.");
                return View(model);
            }
        }
    }
}
