using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	public class CartItem
	{
		public Guid CartItemId { get; set; }

		public Guid CartId { get; set; }

		public Guid ProductColorId { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

		[JsonIgnore]
		public Cart Cart { get; set; }
		public ProductColors ProductColor { get; set; }
	}
}
