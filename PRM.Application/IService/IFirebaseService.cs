using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IFirebaseService
	{
		Task SendMulticastNotificationAsync(MulticastMessage message);

	}
}
