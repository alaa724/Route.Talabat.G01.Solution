using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.Errors;
using Talabat.Infrastructure.Data;

namespace Route.Talabat.APIs.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuggyController : BaseApiController
	{
		private readonly StoreDbContext _dbContext;

		public BuggyController(StoreDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet("notfound")] // Get : api/Buggy/notfound
		public ActionResult GetNotFoundRequest()
		{
			var product = _dbContext.Products.Find(100);

			if(product is null)
				return NotFound(new ApiResponse(404));

			return Ok(product);
		}

		[HttpGet("servererror")] // Get : api/Buggy/servererror
		public ActionResult GetServerError()
		{
			var product = _dbContext.Products.Find(100);

			var productToReturn = product.ToString(); // will throw exception "NullReferenceException"

			return Ok(productToReturn);

		}

		[HttpGet("badrequest")] // Get : api/Buggy/badrequest
		public ActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400));
		}

		[HttpGet("badrequest/{id}")] // Get : api/Buggy/badrequest/five
		public ActionResult GetBadRequest(int id) // Validation Error
		{
			return Ok();
		}

		[HttpGet("unauthorize")]
		public ActionResult GetUnAuthoriaed()
		{
			return Unauthorized(new ApiResponse(401));
		}
	}
}
