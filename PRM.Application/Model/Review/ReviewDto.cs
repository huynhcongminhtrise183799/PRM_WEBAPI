using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Review
{
	public class ReviewDto
	{
		public Guid ReviewId { get; set; }
		public int Rating { get; set; }
		public string Details { get; set; }
		public DateOnly ReviewDate { get; set; }

		public Guid UserId { get; set; }
		public string? UserName { get; set; }

		public Guid ProductId { get; set; }

		public MinimalUserDto User { get; set; }
		public MinimalProductDto Product { get; set; }
	}
}
