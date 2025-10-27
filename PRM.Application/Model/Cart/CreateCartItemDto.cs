using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Cart
{
	public class CreateCartItemDto
	{
		//public Guid  { get; set; }
		public Guid ProductColorId { get; set; }
		public int Quantity { get; set; }
		//public decimal Price { get; set; }
	}
}
