/*using MediatR;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Products.Queries;

public class GetPurchasedProductIdsQueryHandler : IRequestHandler<GetPurchasedProductIdsQuery, List<int>>
{
    private readonly IGenericRepository<TemplateSub> _templateSubRepository;

    public GetPurchasedProductIdsQueryHandler(IGenericRepository<TemplateSub> templateSubRepository)
    {
        _templateSubRepository = templateSubRepository;
    }

    public async Task<List<int>> Handle(GetPurchasedProductIdsQuery request, CancellationToken cancellationToken)
    {
        return await _templateSubRepository.Entities
            .Where(ts => ts.AppUserId == request.AppUserId)
            .Select(ts => ts.ProductId)
            .ToListAsync(cancellationToken);
    }
}
*/