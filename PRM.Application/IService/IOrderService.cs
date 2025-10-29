using PRM.Application.Model.Order;
using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IOrderService
	{
		Task<Order> CreateOrderFromCartAsync(Guid userId);
		Task<List<OrderHistoryDto>> GetOrderHistoryByUserAsync(Guid userId);
	}
}
