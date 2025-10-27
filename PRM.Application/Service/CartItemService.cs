using PRM.Application.IService;
using PRM.Application.Model;
using PRM.Application.Model.Cart;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class CartItemService : ICartItemService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICartItemRepository _cartItemRepository;
		private readonly ICartRepository _cartRepository;
		private readonly IProductColorRepository _productColorRepository;
		public CartItemService(IUnitOfWork unitOfWork, ICartItemRepository cartItemRepository, ICartRepository cartRepository, IProductColorRepository productColorRepository)
		{
			_unitOfWork = unitOfWork;
			_cartItemRepository = cartItemRepository;
			_cartRepository = cartRepository;
			_productColorRepository = productColorRepository;
		}
		public async Task<IEnumerable<CartItemDto>> GetItemsByCartIdAsync(Guid cartId)
		{
			var items = await _cartItemRepository.GetItemsByCartIdAsync(cartId);


			return items.Select(ci => new CartItemDto
			{
				CartItemId = ci.CartItemId,
				CartId = ci.CartId,
				ProductColorId = ci.ProductColorId,
				Quantity = ci.Quantity,
				Price = ci.Price,
				ProductName = ci.ProductColor?.Product?.Name,
				ColorName = ci.ProductColor?.ColorName
			});
		}

		public async Task<(bool, string, CartItemDto result)> AddCartItemAsync(Guid userId, CreateCartItemDto dto)
		{

			try
			{
				var cart = await _cartRepository.GetOrCreateCartAsync(userId);

				var productColor = await _productColorRepository.GetByIdAsync(dto.ProductColorId);
				if (productColor == null)
					return (false, "Product color not found", null);
				var entity = new CartItem
				{
					CartItemId = Guid.NewGuid(),
					CartId = cart.CartId, 
					ProductColorId = dto.ProductColorId,
					Quantity = dto.Quantity,
					Price = productColor.Price,
				};

				await _cartItemRepository.AddAsync(entity);
				await _unitOfWork.SaveChangesAsync();

				var result = new CartItemDto
				{
					CartItemId = entity.CartItemId,
					CartId = entity.CartId,
					ProductColorId = entity.ProductColorId,
					Quantity = entity.Quantity,
					Price = productColor.Price,
					ColorName = productColor.ColorName,
					ProductName = productColor.Product?.Name
				};

				return (true, "Create cart item successfully", result);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[CartItemService.AddCartItemAsync] {ex.Message}");
				Console.WriteLine(ex.StackTrace);
				throw; 
			}
		}

		public async Task<(bool, string, CartItemDto result)> UpdateCartItemAsync(Guid userId,Guid cartItemId, UpdateCartItemDto dto)
		{
			try
			{
				
				var cart = await _cartRepository.GetOrCreateCartAsync(userId);

				
				var existing = await _cartItemRepository.GetByIdAsync(cartItemId);
				if (existing == null || existing.CartId != cart.CartId)
					return (false, "Cart item not found or not belong to user", null);

				var productColor = await _productColorRepository.GetByIdAsync(existing.ProductColorId);
				if (productColor == null)
					return (false, "Product color not found", null);

				
				if (dto.Quantity > productColor.Stock)
					return (false, $"Chỉ còn {productColor.Stock} sản phẩm trong kho", null);

				
				if (dto.Quantity <= 0)
				{
					_cartItemRepository.Remove(existing);
					await _unitOfWork.SaveChangesAsync();
					return (true, "Đã xóa sản phẩm khỏi giỏ hàng", null);
				}

				existing.Quantity = dto.Quantity;
				existing.Price = productColor.Price;

				_cartItemRepository.Update(existing);
				await _unitOfWork.SaveChangesAsync();

				
				var result = new CartItemDto
				{
					CartItemId = existing.CartItemId,
					CartId = existing.CartId,
					ProductColorId = existing.ProductColorId,
					Quantity = existing.Quantity,
					Price = existing.Price,
					ColorName = productColor.ColorName,
					ProductName = productColor.Product?.Name
				};

				return (true, "Update cart item successfully", result);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[CartItemService.UpdateCartItemAsync] {ex.Message}");
				Console.WriteLine(ex.StackTrace);
				throw;
			}

		}
		public async Task<(bool IsSuccess, string Message)> RemoveCartItemAsync(Guid cartItemId)
		{
			var existing = await _cartItemRepository.GetByIdAsync(cartItemId);
			if (existing == null)
				return (false, "CartItem not found");

			_cartItemRepository.Remove(existing);
			await _unitOfWork.SaveChangesAsync();
			return (true, "Remove Successfully");
		}
	}
}
