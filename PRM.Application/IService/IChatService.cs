using PRM.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IChatService
	{
		Task<bool> SendMessageAsync(ChatModel chatModel);

		Task<List<GetMessageModel>> GetMessagesByConversationIdAsync(Guid tripId, int page);
	}
}
