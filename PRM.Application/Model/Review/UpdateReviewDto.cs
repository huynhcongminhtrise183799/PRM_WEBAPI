using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Review
{
	public class UpdateReviewDto
	{
		public Guid ReviewId { get; set; } // Để xác định review cần cập nhật
		public int Rating { get; set; }
		public string Details { get; set; }
	}
}
