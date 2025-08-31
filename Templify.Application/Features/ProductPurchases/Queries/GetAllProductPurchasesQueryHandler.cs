using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.ProductPurchases.Queries
{
    public class GetAllProductPurchasesQueryHandler : IRequestHandler<GetAllProductPurchasesQuery, List<ProductPurchaseDto>>
    {
        private readonly IProductPurchaseRepository _purchaseRepository;

        public GetAllProductPurchasesQueryHandler(IProductPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<List<ProductPurchaseDto>> Handle(GetAllProductPurchasesQuery request, CancellationToken cancellationToken)
        {
            return await _purchaseRepository.GetAllAsync();
        }
    }
}
