using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Order
{
	public class OrderHistoryDto
	{
		public Guid OrderId { get; set; }
		public double TotalAmount { get; set; }
		public string Status { get; set; }
		public string PaymentStatus { get; set; }
		public string PaymentMethod { get; set; }
		public DateOnly? PaymentDate { get; set; }
	}
}
