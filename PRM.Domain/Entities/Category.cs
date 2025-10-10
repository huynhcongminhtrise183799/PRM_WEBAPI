using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	public enum CategoryStatus
	{
		Active,
		Inactive
	}
	public class Category
	{
		public Guid CategoryId { get; set; }
		public string Name { get; set; }

		public string Status { get; set; } // active | inactive

		public virtual ICollection<Product> Products { get; set; }
	}
}
