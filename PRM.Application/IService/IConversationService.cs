using PRM.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IConversationService
	{
		Task<bool> CreateConversation(ConversationModel chatModel);

	}
}
