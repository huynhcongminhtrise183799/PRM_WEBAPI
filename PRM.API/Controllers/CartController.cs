using Microsoft.AspNetCore.Mvc;
using PRM.API.DTOs.Request;
using PRM.Application.Common;
using PRM.Application.IService;
using PRM.Domain.Entities;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;
		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}

		[HttpPost("initialize")]
		public async Task<IActionResult> InitializeCart([FromBody] CreateCartRequest request)
		{

			var cart = await _cartService.InitializeUserCartAsync(request.UserId);
			return Ok(cart);
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetCartByUserId(Guid userId)
		{
			if (userId == Guid.Empty) return BadRequest("User ID is required.");
			var cart = await _cartService.GetCartWithItemsAsync(userId);
			if (cart == null) return NotFound("Cart not found for the specified user.");
			return Ok(cart);
		}
	}
}
