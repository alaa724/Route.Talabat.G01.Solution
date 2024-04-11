
using Microsoft.EntityFrameworkCore;
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
				options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
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
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization(); 


			app.MapControllers();

			
			#endregion

			app.Run();
		}
	}
}
