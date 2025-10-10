using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	
	public enum VoucherStatus
	{
		Active,
		Inactive
	}
	public class Voucher
	{
		public Guid VoucherId { get; set; }

		public string Code { get; set; }

		public string Description { get; set; }

		public double DiscountValue { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime ExpiryDate { get; set; }

		public int UsageLimit { get; set; }

		public string Status { get; set; } // active | inactive

		public ICollection<Order> Orders { get; set; }
	}
}
