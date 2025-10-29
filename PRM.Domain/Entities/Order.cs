using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	/*
	 id bigint [pk, increment]
  order_number varchar [unique, not null]
  user_id bigint
  total_amount decimal(12,2) [not null]
  shipping_fee decimal(12,2)
  discount_amount decimal(12,2)
  status order_status
  payment_status payment_status
  shipping_address varchar
  placed_at timestamp
  updated_at timestamp*/
	public enum OrderStatus
	{
		Pending,
		Processing,
		Shipped,
		Delivered,
		Cancelled
	}
	
	public class Order
	{
		public Guid OrderId { get; set; }

		public Guid UserId { get; set; }

		public Guid? VoucherId { get; set; }

		public double TotalAmount { get; set; }

		public string Status { get; set; }

		public string PaymentStatus { get; set; }

		public User User { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }

		public virtual Payments Payments { get; set; }

		public Voucher Voucher { get; set; }
	}
}
