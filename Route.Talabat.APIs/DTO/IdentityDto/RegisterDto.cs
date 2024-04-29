using System.ComponentModel.DataAnnotations;

namespace Route.Talabat.APIs.DTO.IdentityDto
{
	public class RegisterDto
	{
        [Required]
        public string DisplayName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
		public string Phone { get; set; } = null!;

		[Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Should have at least 1 lowercase , 1 UpperCase , 1 number , 1  special char and 8 characters")]
        public string Password { get; set; } = null!;
    }
}
