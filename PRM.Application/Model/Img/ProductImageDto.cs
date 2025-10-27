using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Img
{
	public class ProductImageDto
	{
		public Guid ProductImageId { get; set; }
		public string ImageUrl { get; set; }
		public string Status { get; set; }
	}
}
