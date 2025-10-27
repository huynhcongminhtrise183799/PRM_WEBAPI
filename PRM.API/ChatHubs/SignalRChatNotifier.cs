using Microsoft.AspNetCore.SignalR;
using PRM.Application.IService;

namespace PRM.API.ChatHubs
{
	public class SignalRChatNotifier : IChatNotifier
	{
		private readonly IHubContext<ChatHub> _hubContext;

		public SignalRChatNotifier(IHubContext<ChatHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task NotifyMessageAsync(Guid conversation, object messageDto)
		{
			await _hubContext.Clients.Group(conversation.ToString())
				.SendAsync("ReceiveMessage", messageDto);
		}
	}
}
