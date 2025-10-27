using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
    public interface IChatNotifier
    {
		Task NotifyMessageAsync(Guid conversation, object message);

	}
}
