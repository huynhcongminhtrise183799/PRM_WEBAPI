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
	public class ChatService : IChatService
	{
		private readonly IChatNotifier _chatNotifier;
		private readonly IMessageRepository _messageRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConversationRepository _conversationRepository;
		public ChatService(IChatNotifier chatNotifier, IMessageRepository messageRepository, IUnitOfWork unitOfWork, IConversationRepository conversationRepository)
		{
			_chatNotifier = chatNotifier;
			_messageRepository = messageRepository;
			_unitOfWork = unitOfWork;
			_conversationRepository = conversationRepository;
		}

		public async Task<Guid> GetConversationIdByAccountId(Guid accountId)
		{
			var conversation = await _conversationRepository.GetConversationByAccountId(accountId);
			if (conversation == null)
			{
				return Guid.Empty;
			}
			return conversation.ConservationId;
		}

		public async Task<List<GetMessageModel>> GetMessagesByConversationIdAsync(Guid conversationId, int page)
		{
			var messages = await _messageRepository.GetMessagesByConversationIdAsync(conversationId, page);
			var result = new List<GetMessageModel>();
			foreach (var message in messages)
			{
				var account = await _unitOfWork.Repository<User>().GetByIdAsync(message.SenderId);
				result.Add(new GetMessageModel
				{
					MessageId = message.MessageId,
					ConversationId = message.ConservationId,
					SenderId = message.SenderId,
					Email = account.Email,
					//AvatarUrl = account.AvatarUrl,
					Content = message.Content,
					//MessageType = message.Type,
					//FileUrl = message.FileUrl,
					SentAt = message.SendAt,
				});
			}
			return result;
		}

		public async Task<bool> SendMessageAsync(ChatModel chatModel)
		{
			var message = new Messages
			{
				MessageId = Guid.NewGuid(),
				ConservationId = chatModel.ConversationId,
				SenderId = chatModel.SenderId,
				Content = chatModel.Content,
				SendAt = DateTime.UtcNow
			};
			var account = await _unitOfWork.Repository<User>().GetByIdAsync(chatModel.SenderId);
			var result = await _messageRepository.Add(message);
			if (!result)
			{
				return false;
			}
			var chatNoti = new GetMessageModel
			{
				MessageId = Guid.NewGuid(),
				ConversationId = chatModel.ConversationId,
				SenderId = chatModel.SenderId,
				Email = account.Email,
				//AvatarUrl = account.AvatarUrl,
				Content = chatModel.Content,
				//MessageType = chatModel.Type,
				//FileUrl = chatModel.FileUrl,
				SentAt = message.SendAt,
			};
			await _chatNotifier.NotifyMessageAsync(chatModel.ConversationId, chatNoti);
			return true;

		}
	}
}
