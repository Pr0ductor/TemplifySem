using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.ProductPurchases.Queries
{
    public class GetProductPurchaseByIdQueryHandler : IRequestHandler<GetProductPurchaseByIdQuery, ProductPurchaseDto?>
    {
        private readonly IProductPurchaseRepository _purchaseRepository;

        public GetProductPurchaseByIdQueryHandler(IProductPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<ProductPurchaseDto?> Handle(GetProductPurchaseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _purchaseRepository.GetByIdAsync(request.Id);
        }
    }
}
