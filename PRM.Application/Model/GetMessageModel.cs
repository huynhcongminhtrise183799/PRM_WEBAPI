using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model
{
	public class GetMessageModel
	{
		public Guid MessageId { get; set; }
		public Guid ConversationId { get; set; }
		public Guid SenderId { get; set; }
		public string Email { get; set; }
		//public string? AvatarUrl { get; set; }
		public string Content { get; set; }
		//public string MessageType { get; set; }
		//public string? FileUrl { get; set; }
		public DateTime SentAt { get; set; }
	}
}
