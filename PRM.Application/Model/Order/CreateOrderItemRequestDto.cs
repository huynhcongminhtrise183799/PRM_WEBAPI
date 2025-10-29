using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Order
{
	public class CreateOrderItemRequestDto
	{
		public Guid ProductColorId { get; set; }
		public int Quantity { get; set; }
		public double UnitPrice { get; set; }
	}
}
