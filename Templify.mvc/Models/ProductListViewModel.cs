using Templify.Application.Common.DTOs;
using System.Collections.Generic;

namespace Templify.mvc.Models;

public class ProductListViewModel
{
    public List<ProductDto> Products { get; set; } = new();
    public List<ProductDto> PurchasedProducts { get; set; } = new();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalProducts { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalProducts / PageSize);
}


