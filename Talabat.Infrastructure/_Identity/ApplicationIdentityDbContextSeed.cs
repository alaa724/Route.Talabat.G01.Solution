using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure._Identity
{
	public static class ApplicationIdentityDbContextSeed
	{
		public static async Task SeedUserAsync(UserManager<ApplicationUsers> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUsers()
				{
					DisplayName = "Alaa Hamdy",
					Email = "alaahamdy@gmail.com",
					UserName = "alaa.hamdy",
					PhoneNumber = "01142756524"
				};

				await userManager.CreateAsync(user, "P@ssw0rd");
			}

		} 
	}
}
