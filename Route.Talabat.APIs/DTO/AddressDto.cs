using System.ComponentModel.DataAnnotations;

namespace Route.Talabat.APIs.DTO
{
	public class AddressDto
	{
		[Required]
		public required string FirstName { get; set; }

		[Required]
		public string LastName { get; set; } = null!;

		[Required]
		public string Street { get; set; } = null!;

		[Required]
		public string City { get; set; } = null!;

		[Required]
		public string Country { get; set; } = null!;
	}
}