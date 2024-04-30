using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Route.Talabat.APIs.Errors;
using Route.Talabat.APIs.Helpers;
using System.Text;
using Talabat.Application.AuthService;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure;

namespace Route.Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(typeof(IBasketRepository) , typeof(BasketRepository));

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.AddAutoMapper(typeof(MappingProfiles));

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
															.SelectMany(P => P.Value.Errors)
															.Select(E => E.ErrorMessage)
															.ToList();

					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);

				};
			});

			return services;

		}

		public static IServiceCollection AddAuthServices(this IServiceCollection services , IConfiguration configuration)
		{
			services.AddAuthentication().AddJwtBearer("Bearer", options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = configuration["JWT:ValidIssure"],
					ValidateAudience = true,
					ValidAudience = configuration["JWT:ValidAudiance"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
				};
			});

			services.AddScoped(typeof(IAuthService), typeof(AuthService));

			return services;
		}
	}
}
