using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.IRepository
{
	public interface IMessageRepository 
	{
		Task<bool> Add(Messages messages);
		Task<List<Messages>> GetMessagesByConversationIdAsync(Guid conversationId, int page);
	}
}
