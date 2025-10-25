using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model
{
	public class UserDeviceTokenModel
	{
		public Guid UserId { get; set; }
		public string FCMToken { get; set; }
	}
}
