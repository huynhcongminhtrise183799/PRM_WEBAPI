using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Order
{
	public class CreateOrderRequestDto
	{
		public Guid UserId { get; set; }
		public Guid? VoucherId { get; set; }

		public List<CreateOrderItemRequestDto> Items { get; set; } = new();
	}
}
