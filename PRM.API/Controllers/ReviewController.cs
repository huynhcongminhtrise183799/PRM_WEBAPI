using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;
using PRM.Application.Model.Review;

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

		[HttpPost]
		public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
		{
			if (dto == null)
				return BadRequest("Dữ liệu không hợp lệ.");

			var result = await _reviewService.CreateAsync(dto);
			if (!result.IsSuccess)
				return BadRequest(result.Message);

			return Ok(new
			{
				message = result.Message,
				data = result.Data
			});
		}

		[HttpGet("product/{productId}")]
		public async Task<IActionResult> GetReviewsByProductId(Guid productId)
		{
			var reviews = await _reviewService.GetByProductIdAsync(productId);
			return Ok(reviews);
		}

		[HttpDelete("{reviewId}")]
		public async Task<IActionResult> DeleteReview(Guid reviewId)
		{
			var result = await _reviewService.DeleteAsync(reviewId);
			if (!result.IsSuccess)
				return NotFound(result.Message);

			return Ok(new { message = result.Message });
		}
	}
}
