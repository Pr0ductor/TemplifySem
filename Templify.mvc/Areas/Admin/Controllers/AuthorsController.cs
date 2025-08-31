using Microsoft.AspNetCore.Mvc;
using MediatR;
using Templify.Application.Features.Authors.Queries;
using Templify.Application.Features.Authors.Commands;
using Templify.Application.Features.Users.Queries;
using Templify.Application.Common.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Templify.mvc.Attributes;

namespace Templify.mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(string search = "")
        {
            var authors = await _mediator.Send(new GetAllAuthorsQuery());
            
            // Передаем параметры фильтров в ViewBag для восстановления состояния
            ViewBag.SearchTerm = search;
            
            return View(authors.OrderBy(a => a.Id).ToList());
        }

        public async Task<IActionResult> Create()
        {
            // Получаем список пользователей для привязки
            var users = await _mediator.Send(new GetAllUsersQuery());
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Nickname} ({u.Email})"
            }).ToList();

            // Добавляем опцию "Без привязки"
            ViewBag.Users.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Без привязки к пользователю"
            });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAuthorCommand command, IFormFile? avatarFile)
        {
            if (ModelState.IsValid)
            {
                string avatarUrl = string.Empty;
                
                // Обработка загрузки файла
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "authors");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }

                    avatarUrl = "/uploads/authors/" + fileName;
                }

                // Создаем команду с AvatarUrl
                var commandWithAvatar = command with { AvatarUrl = avatarUrl };
                
                var result = await _mediator.Send(commandWithAvatar);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при создании автора");
                }
            }

            // Перезаполняем ViewBag в случае ошибки
            var users = await _mediator.Send(new GetAllUsersQuery());
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Nickname} ({u.Email})"
            }).ToList();

            ViewBag.Users.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Без привязки к пользователю"
            });

            return View(command);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery { Id = id });
            if (author == null)
            {
                return NotFound();
            }

            // Получаем список пользователей для привязки
            var users = await _mediator.Send(new GetAllUsersQuery());
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Nickname} ({u.Email})"
            }).ToList();

            ViewBag.Users.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Без привязки к пользователю"
            });

            // Находим AppUser по UserId (IdentityId)
            string? appUserId = null;
            if (!string.IsNullOrEmpty(author.UserId))
            {
                var allUsers = await _mediator.Send(new GetAllUsersQuery());
                var appUser = allUsers.FirstOrDefault(u => u.Email == author.Email);
                if (appUser != null)
                {
                    appUserId = appUser.Id.ToString();
                }
            }

            var command = new UpdateAuthorCommand
            {
                Id = author.Id,
                DisplayName = author.Name,
                Email = author.Email,
                AvatarUrl = author.AvatarUrl,
                Specialization = author.Specialization,
                Description = author.Bio,
                UserId = appUserId
            };

            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAuthorCommand command, IFormFile? avatarFile)
        {
            if (ModelState.IsValid)
            {
                // Обработка загрузки нового файла
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "authors");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }

                    command = command with { AvatarUrl = "/uploads/authors/" + fileName };
                }

                var result = await _mediator.Send(command);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при обновлении автора");
                }
            }

            // Перезаполняем ViewBag в случае ошибки
            var allUsers = await _mediator.Send(new GetAllUsersQuery());
            ViewBag.Users = allUsers.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Nickname} ({u.Email})"
            }).ToList();

            ViewBag.Users.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Без привязки к пользователю"
            });

            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("authors.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteAuthorCommand { Id = id });
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при удалении автора");
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
