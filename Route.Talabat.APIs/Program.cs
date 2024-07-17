
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Route.Talabat.APIs.Errors;
using Route.Talabat.APIs.Extensions;
using Route.Talabat.APIs.Helpers;
using Route.Talabat.APIs.Middlewares;
using StackExchange.Redis;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repository.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure._Identity;
using Talabat.Infrastructure.Data;

namespace Route.Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var WebApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			WebApplicationBuilder.Services.AddControllers();

			WebApplicationBuilder.Services.AddSwaggerServices();

			WebApplicationBuilder.Services.AddApplicationServices();

			WebApplicationBuilder.Services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			WebApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
			});

			WebApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = WebApplicationBuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			WebApplicationBuilder.Services.AddAuthServices(WebApplicationBuilder.Configuration);

			WebApplicationBuilder.Services.AddIdentity<ApplicationUsers, IdentityRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            WebApplicationBuilder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", policyOptions =>
				{
					policyOptions.AllowAnyHeader().AllowAnyMethod().WithOrigins(WebApplicationBuilder.Configuration["FrontBaseUrl"]);
				});
			});

			#endregion

			var app = WebApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreDbContext>(); // Ask CLR for Creating Object from DbContext "Explicitly"

			var _IdentitydbContext = services.GetRequiredService<ApplicationIdentityDbContext>(); // Ask CLR for Creating Object from DbContext "Explicitly"

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database

				await StoreContextSeed.SeedAsync(_dbContext); // DataSeeding

				await _IdentitydbContext.Database.MigrateAsync(); // Update-Database

				var _userManager = services.GetRequiredService<UserManager<ApplicationUsers>>();

				await ApplicationIdentityDbContextSeed.SeedUserAsync(_userManager);
			}
			catch (Exception ex)
			{

				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, " An Error Occured While Migration ");
			}


			#region Configure Kestrel Middelwares

			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseCors("MyPolicy");

			app.MapControllers();

			app.UseAuthentication();

			app.UseAuthorization();

			#endregion

			app.Run();
		}
	}
}
