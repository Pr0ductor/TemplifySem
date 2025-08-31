using Templify.Application.Common.DTOs;
using Templify.Domain.Enums;

namespace Templify.mvc.Models
{
    public class HomeViewModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public List<CategoryType> Categories { get; set; } = new();
        public List<AuthorDto> Authors { get; set; } = new();
    }
}