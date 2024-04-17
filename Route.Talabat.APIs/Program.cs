
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route.Talabat.APIs.Errors;
using Route.Talabat.APIs.Helpers;
using Route.Talabat.APIs.Middlewares;
using Talabat.Core.Repository.Contract;
using Talabat.Infrastructure;
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
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			WebApplicationBuilder.Services.AddEndpointsApiExplorer();
			WebApplicationBuilder.Services.AddSwaggerGen();

			WebApplicationBuilder.Services.AddDbContext<StoreDbContext>(options =>
			{
				options/*.UseLazyLoadingProxies()*/.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			WebApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			WebApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));

			WebApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
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

			#endregion

			var app = WebApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreDbContext>(); // Ask CLR for Creating Object from DbContext "Explicitly"

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database

				await StoreContextSeed.SeedAsync(_dbContext); // DataSeeding
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
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();


			app.UseStaticFiles();


			app.MapControllers();

			
			#endregion

			app.Run();
		}
	}
}
