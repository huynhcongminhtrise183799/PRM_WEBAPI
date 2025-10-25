namespace PRM.API.DTOs.Request
{
	public class ChatRequest
	{
		public Guid ConservationId { get; set; }
		public Guid SenderId { get; set; }
		public string Content { get; set; }
		//public string Type { get; set; }  // "text" | "image" | "file" | "system"
		//public string? FileUrl { get; set; }
	}
}
