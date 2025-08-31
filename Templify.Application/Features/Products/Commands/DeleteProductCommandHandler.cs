using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Products.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductService _productService;
    private readonly IAuthorService _authorService;

    public DeleteProductCommandHandler(IProductService productService, IAuthorService authorService)
    {
        _productService = productService;
        _authorService = authorService;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Если это администратор, пропускаем проверку автора
            if (!request.IsAdmin)
            {
                // Verify that the user is the author of this product
                var isAuthor = await _authorService.IsUserAuthorOfProductAsync(request.UserId, request.ProductId);
                if (!isAuthor)
                {
                    return false;
                }
            }

            // Delete the product
            var result = await _productService.DeleteProductAsync(request.ProductId);
            
            if (result && !request.IsAdmin)
            {
                // Update author stats after deletion (только для обычных пользователей)
                var author = await _authorService.GetAuthorByUserIdAsync(request.UserId);
                if (author != null)
                {
                    await _authorService.UpdateAuthorStatsAsync(author.Id);
                }
            }

            return result;
        }
        catch
        {
            return false;
        }
    }
}

