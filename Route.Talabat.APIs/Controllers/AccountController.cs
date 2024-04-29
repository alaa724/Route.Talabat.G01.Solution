using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.DTO.IdentityDto;
using Route.Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;

namespace Route.Talabat.APIs.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly SignInManager<ApplicationUsers> _signInManager;

		public AccountController(
			UserManager<ApplicationUsers> userManager,
			SignInManager<ApplicationUsers> signInManager) 
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("login")] // POST : /api/Account/login
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null) return Unauthorized(new ApiResponse(401 , "Invalid Login"));

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (result.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = "This will be token"
			});
		}
	}
}
