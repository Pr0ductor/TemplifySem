using Templify.Application.Common.DTOs;
using System.Collections.Generic;

namespace Templify.mvc.Models;

public class ProductListViewModel
{
    public List<ProductDto> Products { get; set; } = new();
    public List<ProductDto> PurchasedProducts { get; set; } = new();
}


