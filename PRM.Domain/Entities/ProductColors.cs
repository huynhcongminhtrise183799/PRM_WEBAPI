using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	public enum ProductColorsStatus
	{
		Active,
		Inactive
	}
	public class ProductColors
	{
		public Guid ProductColorsId { get; set; }
		public Guid ProductId { get; set; }
		public string ColorName { get; set; }
		public string ColorCode { get; set; } // ví dụ: #FF0000

		public int Stock { get; set; }
		public double Price { get; set; } // có thể khác base_price nếu cần

		public string Status { get; set; } // active | inactive

		public virtual Product Product { get; set; }

		public virtual ICollection<ProductImage> ProductImages { get; set; }

		public virtual ICollection<CartItem> CartItems { get; set; }

		public virtual ICollection<OrderItem> OrderItems { get; set; }
	}
}
