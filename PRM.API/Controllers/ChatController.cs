using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM.API.DTOs.Request;
using PRM.Application.IService;
using PRM.Application.Model;

namespace PRM.API.Controllers
{
	[Route("api/chat/")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly IChatService _chatService;
		private readonly IConversationService _conversationService;
		public ChatController(IChatService chatService, IConversationService conversationService)
		{
			_chatService = chatService;
			_conversationService = conversationService;
		}
		[HttpPost("conversation")]
		public async Task<IActionResult> CreateConversation([FromBody] ConversationRequest conversationRequest)
		{
			var model = new ConversationModel
			{
				UserId = conversationRequest.UserId,
				AdminId = conversationRequest.AdminId
			};
			var result = await _conversationService.CreateConversation(model);
			if (!result)
			{
				return BadRequest(new { success = false, message = "Failed to create conversation" });
			}
			return Ok(new { success = true, message = "Conversation created successfully" });
		}


		[HttpPost]
		public async Task<IActionResult> SendMessage([FromBody] ChatRequest chatModel)
		{
			var model = new ChatModel
			{
				ConversationId = chatModel.ConservationId,
				SenderId = chatModel.SenderId,
				Content = chatModel.Content
			};
			var result = await _chatService.SendMessageAsync(model);
			if (!result)
			{
				return BadRequest(new { success = false, message = "Failed to send message" });
			}
			return Ok(new { success = true, message = "Message sent successfully" });
		}
		[HttpGet("{conversationId}")]
		public async Task<IActionResult> GetMessages([FromRoute] Guid conversationId, [FromQuery] int page)
		{
			var messages = await _chatService.GetMessagesByConversationIdAsync(conversationId, page);
			return Ok(new { success = true, data = messages });
		}
		[HttpGet("conversation/account/{accountId}")]
		public async Task<IActionResult> GetConversationIdByAccountId([FromRoute] Guid accountId)
		{
			var conversationId = await _chatService.GetConversationIdByAccountId(accountId);
			return Ok(new { success = true, data = conversationId });
		}

	}
}
