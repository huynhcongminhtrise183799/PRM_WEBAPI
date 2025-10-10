using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	
	public class OrderItem
	{
		public Guid OrderItemId { get; set; }

		public Guid OrderId { get; set; }

		public Guid ProductColorId { get; set; }

		public int Quantity { get; set; }

		public double UnitPrice { get; set; }

		public Order Order { get; set; }

		public ProductColors ProductColor { get; set; }
	}
}
