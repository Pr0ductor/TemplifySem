using MediatR;
using Microsoft.AspNetCore.Mvc;
using Templify.mvc.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Templify.Application.Common.DTOs;
using Templify.Application.Features.Users.Queries;
using Templify.Application.Features.Authors.Queries;

namespace Templify.mvc.Controllers;

[Authorize]
public class AuthorsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(IMediator mediator, ILogger<AuthorsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["ActiveTab"] = "authors";
        var allAuthors = await _mediator.Send(new Templify.Application.Features.Authors.Queries.GetAllAuthorsQuery());
        
        // Применяем поиск если есть параметр search
        if (!string.IsNullOrWhiteSpace(search))
        {
            allAuthors = allAuthors
                .Where(a => a.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int? appUserId = null;
        if (!string.IsNullOrEmpty(identityId))
            appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
        List<AuthorDto> subs = new();
        if (appUserId != null)
        {
            var subscribedAuthorIds = await _mediator.Send(new Templify.Application.Features.AuthorSubscriptions.Queries.GetSubscribedAuthorIdsQuery { AppUserId = appUserId.Value });
            var subIds = subscribedAuthorIds.ToHashSet();
            subs = allAuthors.Where(a => subIds.Contains(a.Id)).ToList();
            allAuthors = allAuthors.Where(a => !subIds.Contains(a.Id)).ToList();
        }
        return View((new AuthorsListViewModel { Authors = subs }, new AuthorsListViewModel { Authors = allAuthors }));
    }

    public async Task<IActionResult> Author(int id)
    {
        ViewData["ActiveTab"] = "authors";
        var authorWithProducts = await _mediator.Send(new GetAuthorWithProductsQuery { Id = id });
        if (authorWithProducts == null) return NotFound();
        
        // Временное логирование для отладки
        _logger.LogInformation($"Author data - ID: {authorWithProducts.Author.Id}, Name: {authorWithProducts.Author.Name}, Email: {authorWithProducts.Author.Email}, UserId: {authorWithProducts.Author.UserId}");
        
        var vm = new AuthorViewModel { AuthorWithProducts = authorWithProducts };
        return View(vm);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Subscribe(int authorId)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId)) return Unauthorized();
        var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
        if (appUserId == null) return Unauthorized();
        var result = await _mediator.Send(new Templify.Application.Features.AuthorSubscriptions.Commands.SubscribeToAuthorCommand { AuthorId = authorId, AppUserId = appUserId.Value });
        return Json(new { success = result });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Unsubscribe(int authorId)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId)) return Unauthorized();
        var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
        if (appUserId == null) return Unauthorized();
        var result = await _mediator.Send(new Templify.Application.Features.AuthorSubscriptions.Commands.UnsubscribeFromAuthorCommand { AuthorId = authorId, AppUserId = appUserId.Value });
        return Json(new { success = result });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> IsSubscribed(int authorId)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId)) return Json(new { subscribed = false });
        var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
        if (appUserId == null) return Json(new { subscribed = false });
        var result = await _mediator.Send(new Templify.Application.Features.AuthorSubscriptions.Queries.IsSubscribedQuery { AuthorId = authorId, AppUserId = appUserId.Value });
        return Json(new { subscribed = result });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserSubscriptions()
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId)) return Content("");
        var appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
        if (appUserId == null) return Content("");
        // var subs = await _mediator.Send(new GetUserSubscriptionsQuery { UserId = appUserId.Value.ToString() }); // [Временно закомментировано: отсутствует GetUserSubscriptionsQuery]
        var subs = new List<AuthorDto>();
        if (subs == null || !subs.Any()) return Content("");
        return PartialView("_AuthorsListPartial", new AuthorsListViewModel { Authors = subs });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> IsCurrentUserAuthor(int authorId)
    {
        try
        {
            var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identityId)) 
            {
                return Json(new { success = false, isCurrentUser = false });
            }
            
            var author = await _mediator.Send(new GetAuthorByIdQuery { Id = authorId });
            if (author == null) 
            {
                return Json(new { success = false, isCurrentUser = false });
            }
            
            var isCurrentUser = author.UserId == identityId;
            
            return Json(new { success = true, isCurrentUser });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = $"Exception: {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> SearchAuthors(string? search)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int? appUserId = null;
        if (!string.IsNullOrEmpty(identityId))
            appUserId = await _mediator.Send(new GetAppUserIdByIdentityIdQuery { IdentityId = identityId });
        var allAuthors = await _mediator.Send(new Templify.Application.Features.Authors.Queries.SearchAuthorsQuery { SearchTerm = search });
        List<AuthorDto> subs = new();
        if (appUserId != null)
        {
            var subscribedAuthorIds = await _mediator.Send(new Templify.Application.Features.AuthorSubscriptions.Queries.GetSubscribedAuthorIdsQuery { AppUserId = appUserId.Value });
            var subIds = subscribedAuthorIds.ToHashSet();
            subs = allAuthors.Where(a => subIds.Contains(a.Id)).ToList();
            allAuthors = allAuthors.Where(a => !subIds.Contains(a.Id)).ToList();
        }
        return PartialView("_AuthorsBlocksPartial", (new AuthorsListViewModel { Authors = subs }, new AuthorsListViewModel { Authors = allAuthors }));
    }

}