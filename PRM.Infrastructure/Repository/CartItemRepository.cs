using Microsoft.EntityFrameworkCore;
using PRM.Application.Model.Cart;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Infrastructure.Repository
{
	public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
	{
		private readonly PRMDbContext _context;

		public CartItemRepository(PRMDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(Guid cartId)
		{
			return await _context.CartItems
				.Where(ci => ci.CartId == cartId)
				.Include(ci => ci.ProductColor)
					.ThenInclude(pc => pc.Product) 
				.ToListAsync();
		}
	}
}
