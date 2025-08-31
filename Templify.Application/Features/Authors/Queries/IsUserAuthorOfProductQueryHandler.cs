using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Authors.Queries;

public class IsUserAuthorOfProductQueryHandler : IRequestHandler<IsUserAuthorOfProductQuery, bool>
{
    private readonly IAuthorService _authorService;
    private readonly IProductService _productService;

    public IsUserAuthorOfProductQueryHandler(IAuthorService authorService, IProductService productService)
    {
        _authorService = authorService;
        _productService = productService;
    }

    public async Task<bool> Handle(IsUserAuthorOfProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Получаем продукт
            var product = await _productService.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                return false;
            }

            // Получаем автора продукта
            var author = await _authorService.GetAuthorByIdAsync(product.AuthorId);
            if (author == null)
            {
                return false;
            }

            // Проверяем, является ли пользователь автором
            return author.UserId == request.UserId;
        }
        catch
        {
            return false;
        }
    }
}
