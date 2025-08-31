using Templify.Application.Common.DTOs;

namespace Templify.mvc.Models
{
    public class AuthorsListViewModel
    {
        public List<AuthorDto> Authors { get; set; } = new();
    }
}