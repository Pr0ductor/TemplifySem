using Microsoft.AspNetCore.Mvc;
using MediatR;
using Templify.Application.Features.ProductPurchases.Queries;
using Templify.Application.Features.ProductPurchases.Commands;
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
    public class ProductPurchasesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public ProductPurchasesController(
            IMediator mediator,
            IGenericRepository<AppUser> userRepository,
            IGenericRepository<Product> productRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(string search = "")
        {
            var purchases = await _mediator.Send(new GetAllProductPurchasesQuery());
            
            // Передаем параметры фильтров в ViewBag для восстановления состояния
            ViewBag.SearchTerm = search;
            
            return View(purchases.OrderBy(p => p.Id).ToList());
        }

        public async Task<IActionResult> Create()
        {
            // Загружаем списки пользователей и продуктов
            var users = await _userRepository.Entities
                .Include(u => u.Identity)
                .ToListAsync();
            
            var products = await _productRepository.Entities
                .Include(p => p.AuthorEntity)
                .ToListAsync();

            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})"
            }).ToList();

            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} - ${p.Price:F2} ({p.AuthorEntity?.Name ?? p.Author})"
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductPurchaseDto purchaseDto)
        {
            if (ModelState.IsValid)
            {
                var command = new PurchaseProductCommand
                {
                    AppUserId = purchaseDto.UserId,
                    ProductId = purchaseDto.ProductId
                };

                var result = await _mediator.Send(command);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при создании покупки");
                }
            }

            // Перезагружаем списки в случае ошибки
            var users = await _userRepository.Entities
                .Include(u => u.Identity)
                .ToListAsync();
            
            var products = await _productRepository.Entities
                .Include(p => p.AuthorEntity)
                .ToListAsync();

            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})"
            }).ToList();

            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} - ${p.Price:F2} ({p.AuthorEntity?.Name ?? p.Author})"
            }).ToList();

            return View(purchaseDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var purchase = await _mediator.Send(new GetProductPurchaseByIdQuery { Id = id });
            if (purchase == null)
            {
                return NotFound();
            }

            // Загружаем списки пользователей и продуктов для редактирования
            var users = await _userRepository.Entities
                .Include(u => u.Identity)
                .ToListAsync();
            
            var products = await _productRepository.Entities
                .Include(p => p.AuthorEntity)
                .ToListAsync();

            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})",
                Selected = u.Id == purchase.UserId
            }).ToList();

            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} - ${p.Price:F2} ({p.AuthorEntity?.Name ?? p.Author})",
                Selected = p.Id == purchase.ProductId
            }).ToList();

            return View(purchase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductPurchaseDto purchaseDto)
        {
            if (id != purchaseDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var command = new UpdateProductPurchaseCommand
                {
                    Id = purchaseDto.Id,
                    UserId = purchaseDto.UserId,
                    ProductId = purchaseDto.ProductId
                };

                var result = await _mediator.Send(command);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при обновлении покупки");
                }
            }

            // Перезагружаем списки в случае ошибки
            var users = await _userRepository.Entities
                .Include(u => u.Identity)
                .ToListAsync();
            
            var products = await _productRepository.Entities
                .Include(p => p.AuthorEntity)
                .ToListAsync();

            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Username} ({u.Identity?.Email})",
                Selected = u.Id == purchaseDto.UserId
            }).ToList();

            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} - ${p.Price:F2} ({p.AuthorEntity?.Name ?? p.Author})",
                Selected = p.Id == purchaseDto.ProductId
            }).ToList();

            return View(purchaseDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductPurchaseCommand { Id = id });
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при удалении покупки");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
