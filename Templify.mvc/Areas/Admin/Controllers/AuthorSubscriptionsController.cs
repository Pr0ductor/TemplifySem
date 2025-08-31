using Microsoft.AspNetCore.Mvc;
using MediatR;
using Templify.Application.Features.AuthorSubscriptions.Queries;
using Templify.Application.Features.AuthorSubscriptions.Commands;
using Templify.Application.Common.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Templify.mvc.Attributes;

namespace Templify.mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RequireRole("Admin")]
    public class AuthorSubscriptionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IAuthorRepository _authorRepository;

        public AuthorSubscriptionsController(IMediator mediator, IGenericRepository<AppUser> userRepository, IAuthorRepository authorRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IActionResult> Index(string search = "")
        {
            var subscriptions = await _mediator.Send(new GetAllAuthorSubscriptionsQuery());
            
            // Передаем параметры фильтров в ViewBag для восстановления состояния
            ViewBag.SearchTerm = search;
            
            return View(subscriptions.OrderBy(s => s.Id).ToList());
        }

        // GET: Admin/AuthorSubscriptions/Create
        public async Task<IActionResult> Create()
        {
            // Загружаем пользователей для выпадающего списка
            var users = await _userRepository.Entities.Include(u => u.Identity).ToListAsync();
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})"
            }).ToList();

            // Загружаем авторов для выпадающего списка
            var authors = await _authorRepository.GetAllAsync();
            ViewBag.Authors = authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            return View();
        }

        // POST: Admin/AuthorSubscriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAuthorSubscriptionCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var subscriptionId = await _mediator.Send(command);
                    TempData["SuccessMessage"] = "Подписка успешно создана!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при создании подписки: " + ex.Message);
                }
            }

            // Если что-то пошло не так, загружаем данные для выпадающих списков снова
            var users = await _userRepository.Entities.Include(u => u.Identity).ToListAsync();
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})"
            }).ToList();

            var authors = await _authorRepository.GetAllAsync();
            ViewBag.Authors = authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            return View(command);
        }

        // GET: Admin/AuthorSubscriptions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var subscription = await _mediator.Send(new GetAuthorSubscriptionByIdQuery { Id = id });
            if (subscription == null)
            {
                return NotFound();
            }

            // Загружаем пользователей для выпадающего списка
            var users = await _userRepository.Entities.Include(u => u.Identity).ToListAsync();
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})",
                Selected = u.Id == subscription.UserId
            }).ToList();

            // Загружаем авторов для выпадающего списка
            var authors = await _authorRepository.GetAllAsync();
            ViewBag.Authors = authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = a.Id == subscription.AuthorId
            }).ToList();

            return View(subscription);
        }

        // POST: Admin/AuthorSubscriptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorSubscriptionDto dto)
        {
            if (id != dto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var command = new UpdateAuthorSubscriptionCommand
                {
                    Id = dto.Id,
                    UserId = dto.UserId,
                    AuthorId = dto.AuthorId
                };

                var result = await _mediator.Send(command);
                if (result)
                {
                    TempData["SuccessMessage"] = "Подписка успешно обновлена!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при обновлении подписки");
                }
            }

            // Если что-то пошло не так, загружаем данные для выпадающих списков снова
            var users = await _userRepository.Entities.Include(u => u.Identity).ToListAsync();
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})",
                Selected = u.Id == dto.UserId
            }).ToList();

            var authors = await _authorRepository.GetAllAsync();
            ViewBag.Authors = authors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = a.Id == dto.AuthorId
            }).ToList();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteAuthorSubscriptionCommand { Id = id });
            if (result)
            {
                TempData["SuccessMessage"] = "Подписка успешно удалена!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Ошибка при удалении подписки";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
