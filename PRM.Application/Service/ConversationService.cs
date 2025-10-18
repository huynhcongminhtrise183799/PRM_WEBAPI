using PRM.Application.IService;
using PRM.Application.Model;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class ConversationService : IConversationService
	{
		private readonly IConversationRepository _repo;

		public ConversationService(IConversationRepository repository)
		{
			_repo = repository;
		}

		public async Task<bool> CreateConversation(ConversationModel chatModel)
		{
			var conversation = new Conversation
			{
				ConservationId = Guid.NewGuid(),
				UserId = chatModel.UserId,
				AdminId = chatModel.AdminId,
			};
			return await _repo.Add(conversation);
		}
	}
}
