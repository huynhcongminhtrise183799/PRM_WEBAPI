using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	/*
	 id bigint [pk, increment]
  cart_id bigint
  product_color_id bigint
  quantity int
  unit_price decimal(12,2)
  added_at timestamp*/
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
