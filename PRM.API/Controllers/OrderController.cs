using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet("history/{userId}")]
		public async Task<IActionResult> GetOrderHistory(Guid userId)
		{
			var result = await _orderService.GetOrderHistoryByUserAsync(userId);
			return Ok(new { data = result });
		}

	}
}
