using PRM.Application.Model.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Product
{
	public class UpdateProductDto
	{
		public string Name { get; set; }

		public string? Description { get; set; }

		public double WeightKg { get; set; }

		public Guid CategoryId { get; set; }

		public Guid SupplierId { get; set; }

		//public string Status { get; set; }

		public List<CreateProductColorDto>? ProductColors { get; set; }
	}
}
