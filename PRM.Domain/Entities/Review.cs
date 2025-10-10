using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	/*
	 id bigint [pk, increment]
  product_id bigint
  user_id bigint
  rating int
  title varchar
  body text
  is_approved boolean
  created_at timestamp
  updated_at timestamp*/
	public class Review
	{
		public Guid ReviewId { get; set; }

		public Guid ProductId { get; set; }

		public Guid UserId { get; set; }

		public int Rating { get; set; }

		public DateOnly ReviewDate { get; set; }

		public string Details { get; set; }

		public User User { get; set; }
		public Product Product { get; set; }

	}
}
