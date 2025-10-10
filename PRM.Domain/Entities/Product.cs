using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	/*
	 id bigint [pk, increment]
  // sku varchar [unique, not null]
  name varchar [not null]
  // slug varchar [unique, not null]
  description text
  price decimal(12,2) [not null]
  // retail_price decimal(12,2)
  weight_kg decimal(8,2)
  category_id int
  supplier_id int
  is_active boolean
  created_at timestamp
  updated_at timestamp*/
	public enum ProductStatus
	{
		Active,
		Inactive
	}
	public class Product
	{
		public Guid ProductId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public double WeightKg { get; set; }

		public Guid CategoryId { get; set; }

		public Guid SupplierId { get; set; }

		public string Status { get; set; } // active | inactive

		public virtual Category Category { get; set; }

		public virtual Suppliers Supplier { get; set; }

		public virtual ICollection<Review> Reviews { get; set; }

		public virtual ICollection<ProductColors> ProductColors { get; set; }

	}
}
