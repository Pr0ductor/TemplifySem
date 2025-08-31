using MediatR;

namespace Templify.Application.Features.ProductPurchases.Commands
{
    public class UpdateProductPurchaseCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        // public DateTime PurchaseDate { get; set; } // Удалено, теперь дата не редактируется вручную
    }
}
