using PRM.Application.Model.Img;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Color
{
	public class ProductColorDto
	{
		public Guid ProductColorsId { get; set; }
		public string ColorName { get; set; }
		public string ColorCode { get; set; }
		public int Stock { get; set; }
		public double Price { get; set; }
		public string Status { get; set; }
		public List<ProductImageDto>? ProductImages { get; set; }
	}
}
