using PRM.Application.IService;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class CartService : ICartService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICartRepository _cartRepository;
		public CartService(IUnitOfWork unitOfWork, ICartRepository cartRepository)
		{
			_unitOfWork = unitOfWork;
			_cartRepository = cartRepository;
		}

		public Task<Cart> GetCartWithItemsAsync(Guid userId)
		{
			var cart = _cartRepository.GetCartWithItemsAsync(userId);

			return cart;
		}

		public async Task<Cart> InitializeUserCartAsync(Guid userId)
		{
			var cart = await _cartRepository.GetOrCreateCartAsync(userId);
			return cart;
		}
	}
}
