using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	public class UserDeviceToken
	{
		public Guid UserDeviceTokenId { get; set; }
		public Guid UserId { get; set; }

		public string FCMToken { get; set; }

		public User User { get; set; }
	}
}
