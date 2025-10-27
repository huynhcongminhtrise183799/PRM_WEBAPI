using PRM.Application.Model.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface ICartItemService
	{
		Task<IEnumerable<CartItemDto>> GetItemsByCartIdAsync(Guid cartId);
		Task<(bool, string, CartItemDto result)> AddCartItemAsync(Guid userId, CreateCartItemDto dto);
		Task<(bool, string, CartItemDto result)> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemDto dto);
		Task<(bool IsSuccess, string Message)> RemoveCartItemAsync(Guid cartItemId);

	}
}
