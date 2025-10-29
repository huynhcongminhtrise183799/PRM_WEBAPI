using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Payment
{
	public class PaymentResponseDto
	{
		public Guid OrderId { get; set; }
		public string PaymentUrl { get; set; } = string.Empty;
	}
}
