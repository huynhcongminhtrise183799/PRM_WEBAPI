using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Cart
{
	public class CartItemDto
	{
		public Guid CartItemId { get; set; }
		public Guid CartId { get; set; }
		public Guid ProductColorId { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }

		public string? ProductName { get; set; }
		public string? ColorName { get; set; }
	}
}
