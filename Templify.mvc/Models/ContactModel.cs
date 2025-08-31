using System.ComponentModel.DataAnnotations;

namespace Templify.mvc.Models
{
	public class ContactModel
	{
		[Required(ErrorMessage = "Full name is required")]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be 2-100 characters")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Message is required")]
		[StringLength(2000, MinimumLength = 10, ErrorMessage = "Message must be 10-2000 characters")]
		public string Message { get; set; } = string.Empty;
	}
}
