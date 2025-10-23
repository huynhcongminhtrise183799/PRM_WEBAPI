using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Review
{
	public class CreateReviewDto
	{
		public Guid ProductId { get; set; }
		public Guid UserId { get; set; }
		public int Rating { get; set; }
		public string Details { get; set; }

	}
}
