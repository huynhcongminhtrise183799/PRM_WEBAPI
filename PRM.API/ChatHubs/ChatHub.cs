using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace PRM.API.ChatHubs
{
	public class ChatHub : Hub
	{
		public async Task JoinGroup(string tripId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, tripId);
		}

		public async Task LeaveGroup(string tripId)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, tripId);
		}
	}
}
