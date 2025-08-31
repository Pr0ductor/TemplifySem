using Templify.Application.Common.DTOs;

namespace Templify.mvc.Models;

public class MyTemplatesViewModel
{
    public AuthorDto Author { get; set; } = new();
    public List<ProductDto> Products { get; set; } = new();
}
