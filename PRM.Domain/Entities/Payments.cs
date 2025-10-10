using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	/*
	  id bigint [pk, increment]
  order_id bigint
  amount decimal(12,2)
  method varchar
  provider_transaction_id varchar
  status payment_status
  paid_at timestamp
  created_at timestamp*/

	public enum PaymentStatus
	{
		Paid,
		Unpaid,
		Refunded
	}

	public enum PaymentMethod
	{
		CreditCard,
		DebitCard,
		PayPal,
		BankTransfer,
		CashOnDelivery
	}

	public class Payments
	{
		public Guid PaymentId { get; set; }
		public Guid OrderId { get; set; }
		public double Amount { get; set; }
		public string Method { get; set; }

		public DateOnly PaymentDate { get; set; }

		public string Status { get; set; } // paid | unpaid | refunded
		public Order Order { get; set; }


	}
}
