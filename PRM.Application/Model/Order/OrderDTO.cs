using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Order
{
	public class OrderDTO
	{
		public Guid OrderId { get; set; }
		public Guid UserId { get; set; }
		public Guid? VoucherId { get; set; }
		public double TotalAmount { get; set; }
		public string Status { get; set; } = "Pending";
		public string PaymentStatus { get; set; } = "Unpaid";

		public string? UserName { get; set; }
		public string? VoucherCode { get; set; }

		public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

	}
}
