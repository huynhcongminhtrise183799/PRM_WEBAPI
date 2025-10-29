using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Order
{
	public class OrderItemDto
	{
		public Guid OrderItemId { get; set; }
		public Guid ProductColorId { get; set; }
		public string? ProductName { get; set; } 
		public string? ColorName { get; set; } 
		public int Quantity { get; set; }
		public double UnitPrice { get; set; }
		public double TotalPrice => Quantity * UnitPrice; 
	}
}
