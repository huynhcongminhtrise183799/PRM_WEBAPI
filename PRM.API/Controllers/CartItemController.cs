using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;
using PRM.Application.Model.Cart;
using PRM.Application.Service;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartItemController : ControllerBase
	{
		private readonly ICartItemService _cartItemService;
		private readonly ICartService _cartService;
		public CartItemController(ICartItemService cartItemService, ICartService cartService)
		{
			_cartItemService = cartItemService;
			_cartService = cartService;
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
		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetItemsByUserId(Guid userId)
		{
			if (userId == Guid.Empty)
				return BadRequest(new { message = "User ID is required." });

			try
			{
				// 🔹 Lấy giỏ hàng có chứa các item
				var cart = await _cartService.GetCartWithItemsAsync(userId);

				if (cart == null)
					return NotFound(new { message = "Cart not found for this user." });

				if (cart.CartItems == null || !cart.CartItems.Any())
					return Ok(new List<object>()); // trả về rỗng để frontend dễ xử lý

				// 🔹 Map sang DTO tương tự GetItemsByCartIdAsync
				var items = cart.CartItems.Select(ci => new
				{
					cartItemId = ci.CartItemId,
					cartId = ci.CartId,
					productColorId = ci.ProductColorId,
					quantity = ci.Quantity,
					price = ci.Price,
					productName = ci.ProductColor?.Product?.Name,
					colorName = ci.ProductColor?.ColorName,
					imageUrl = ci.ProductColor?.ProductImages
								
				});

				return Ok(items);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[CartItemController.GetItemsByUserId] {ex.Message}");
				return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
			}
		}
	}
}
