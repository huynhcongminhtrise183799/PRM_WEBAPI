using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
	    private readonly IReviewService _reviewService;

		public ReviewController(IReviewService reviewService)
		{
			_reviewService = reviewService;
		}
	}
}
