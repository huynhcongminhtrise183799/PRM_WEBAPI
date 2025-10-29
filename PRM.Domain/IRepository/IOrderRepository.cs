using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.IRepository
{
	public interface IOrderRepository : IGenericRepository<Order>
	{
		Task<Order> GetOrderWithItemsAsync(Guid orderId);
		Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
		Task<bool> IsPaidAsync(Guid orderId);
		Task<List<Order>> GetOrdersByUserAsync(Guid userId);

	}
}
