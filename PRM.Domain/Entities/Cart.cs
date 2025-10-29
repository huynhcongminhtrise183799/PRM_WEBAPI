using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	public class Cart
	{
		public Guid CartId { get; set; }

		public Guid UserId { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public User User { get; set; }

		public ICollection<CartItem> CartItems { get; set; }
	}
}
