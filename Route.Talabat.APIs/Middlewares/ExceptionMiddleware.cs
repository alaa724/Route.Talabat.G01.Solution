using Route.Talabat.APIs.Errors;
using System.Net;
using System.Text.Json;

namespace Route.Talabat.APIs.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IWebHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{

			try
			{
				await _next.Invoke(httpContext);

			}
			catch (Exception ex)
			{

				_logger.LogError(ex.Message);  // Development env
											   // Log Exception in (Database | Files) // Production env

				httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError ;
				httpContext.Response.ContentType = "application/json";


				var response = _env.IsDevelopment() ?
					new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
					:
					new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

				var json = JsonSerializer.Serialize(response);

				await httpContext.Response.WriteAsync(json);
			}


		}
	}
}
