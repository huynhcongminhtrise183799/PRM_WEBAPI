using Microsoft.EntityFrameworkCore;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Infrastructure.Repository
{
	public class CartRepository : GenericRepository<Cart>, ICartRepository
	{
		private readonly PRMDbContext _context;

		public CartRepository(PRMDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Cart> GetOrCreateCartAsync(Guid userId)
		{
			var cart = await _context.Carts
			.Include(c => c.CartItems)
				.ThenInclude(ci => ci.ProductColor)
					.ThenInclude(pc => pc.Product)
			.FirstOrDefaultAsync(c => c.UserId == userId);
			if (cart == null)
			{
				cart = new Cart
				{
					CartId = Guid.NewGuid(),
					UserId = userId,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};

				await _context.Carts.AddAsync(cart);
				await _context.SaveChangesAsync();
			}

			return cart;
		}
		public async Task<Cart> GetCartWithItemsAsync(Guid userId)
		{
			var cart = await _context.Carts
		   .Include(c => c.CartItems)
			   .ThenInclude(ci => ci.ProductColor)
				   .ThenInclude(pc => pc.Product)
		   .Include(c => c.CartItems)
			   .ThenInclude(ci => ci.ProductColor)
				   .ThenInclude(pc => pc.ProductImages) // 🔹 thêm dòng này
		   .FirstOrDefaultAsync(c => c.UserId == userId);
			if (cart == null)
			{
				cart = new Cart
				{
					UserId = userId,
					CartItems = new List<CartItem>()
				};
			}
			cart.CartItems ??= new List<CartItem>();

			return cart;
		}

		public Task<IEnumerable<Cart>> GetCartsByUserIdAsync(Guid userId)
		{
			var carts = _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.ProductColor)
				.Where(c => c.UserId == userId)
				.AsEnumerable();
			return Task.FromResult(carts);
		}
	}
}
