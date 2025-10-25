using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	public enum UserRole
	{
		Customer,
		Admin,
		Staff
	}
	public enum UserStatus
	{
		Active,
		Inactive,
		Banned
	}
	public class User
	{
		public Guid UserId { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string FullName { get; set; }

		public string Phone { get; set; }

		public string Role { get; set; } // customer | admin | staff

		public string Status { get; set; } // active | inactive | banned

		public virtual ICollection<Cart> Carts { get; set; }

		public virtual ICollection<Order> Orders { get; set; }

		public virtual ICollection<Review> Reviews { get; set; }
		public virtual ICollection<UserDeviceToken> UserDeviceTokens { get; set; }

	}
}
