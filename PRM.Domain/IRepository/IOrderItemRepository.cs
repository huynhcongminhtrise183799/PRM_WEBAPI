using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.IRepository
{
	public interface IOrderItemRepository
	{
		void AddRange(IEnumerable<OrderItem> orderItems);
	}
}
