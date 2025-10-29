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
	public class OrderRepository : GenericRepository<Order>, IOrderRepository
	{
		private readonly PRMDbContext _context;
		public OrderRepository(PRMDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Order>> GetOrdersByUserAsync(Guid userId)
		{
			return await _context.Orders
			.Where(o => o.UserId == userId)
			.Include(o => o.Payments)
			.OrderByDescending(o => o.OrderId)
			.ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
		{
			return await _context.Orders
			.Where(o => o.UserId == userId)
			.Include(o => o.OrderItems)
			.ToListAsync();
		}

		public async Task<Order> GetOrderWithItemsAsync(Guid orderId)
		{
			var order = await _context.Orders
			.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.ProductColor)
			.FirstOrDefaultAsync(o => o.OrderId == orderId);

			return order ?? throw new KeyNotFoundException($"Order {orderId} not found.");
		}

		public async Task<bool> IsPaidAsync(Guid orderId)
		{
			var status = await _context.Orders
			.Where(o => o.OrderId == orderId)
			.Select(o => o.PaymentStatus)
			.FirstOrDefaultAsync();

			return status == "Paid";
		}
	}
}
