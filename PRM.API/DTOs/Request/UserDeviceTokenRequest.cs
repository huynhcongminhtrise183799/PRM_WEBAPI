namespace PRM.API.DTOs.Request
{
    public class UserDeviceTokenRequest
    {
		public Guid UserId { get; set; }
		public string FCMToken { get; set; }
	}
}
