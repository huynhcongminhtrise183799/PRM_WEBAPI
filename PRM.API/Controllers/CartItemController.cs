using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;
using PRM.Application.Model.Cart;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartItemController : ControllerBase
	{
		private readonly ICartItemService _cartItemService;
		public CartItemController(ICartItemService cartItemService)
		{
			_cartItemService = cartItemService;
		}

		[HttpGet("{cartId}")]
		public async Task<IActionResult> GetItemsByCartId(Guid cartId)
		{
			var items = await _cartItemService.GetItemsByCartIdAsync(cartId);
			return Ok(items);
		}

		[HttpPost]
		public async Task<IActionResult> AddCartItem(Guid userId, [FromBody] CreateCartItemDto dto)
		{
			try
			{
				
				//var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? throw new Exception("User not found"));
				var (isSuccess, message, result) = await _cartItemService.AddCartItemAsync(userId, dto);

				if (!isSuccess)
					return BadRequest(new { message });

				return Ok(new { message, data = result });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = ex.Message });
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateCartItem(Guid userId, Guid cartItemId, [FromBody] UpdateCartItemDto dto)
		{
			var (isSuccess, message, result) = await _cartItemService.UpdateCartItemAsync(userId, cartItemId, dto);
			if (!isSuccess)
				return BadRequest(new { message });
			return Ok(new { message, data = result });
		}

		[HttpDelete("{cartItemId}")]
		public async Task<IActionResult> RemoveCartItem(Guid cartItemId)
		{
			var (isSuccess, message) = await _cartItemService.RemoveCartItemAsync(cartItemId);
			if (!isSuccess)
				return NotFound(new { message });
			return Ok(new { message });
		}

	}
}
