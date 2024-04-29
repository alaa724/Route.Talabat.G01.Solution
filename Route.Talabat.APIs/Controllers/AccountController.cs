using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.DTO.IdentityDto;
using Route.Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Route.Talabat.APIs.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly SignInManager<ApplicationUsers> _signInManager;
		private readonly IAuthService _authService;

		public AccountController(
			UserManager<ApplicationUsers> userManager,
			SignInManager<ApplicationUsers> signInManager,
			IAuthService authService) 
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_authService = authService;
		}

		[HttpPost("login")] // POST : /api/Account/login
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null) return Unauthorized(new ApiResponse(401 , "Invalid Login"));

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (!result.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authService.CreateTokenAsync(user, _userManager)
			});
		}


		[HttpPost("register")] // POST : /api/Account/register
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var user = new ApplicationUsers()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split("@")[0],
				PhoneNumber = model.Phone
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded) return Unauthorized(new ApiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authService.CreateTokenAsync(user, _userManager)
			});
		}

	}
}
