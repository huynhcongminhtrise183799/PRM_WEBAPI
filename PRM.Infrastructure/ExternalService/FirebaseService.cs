using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using PRM.Application.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Infrastructure.ExternalService
{
	public class FirebaseService : IFirebaseService 
	{
		private readonly FirebaseMessaging _firebaseMessaging;
		private readonly ILogger<FirebaseService> _logger;

		public FirebaseService(FirebaseMessaging firebaseMessaging, ILogger<FirebaseService> logger)
		{
			_firebaseMessaging = firebaseMessaging ?? throw new ArgumentNullException(nameof(firebaseMessaging));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public async Task SendMulticastNotificationAsync(MulticastMessage message)
		{
			// Kiểm tra đầu vào cơ bản
			if (message == null)
			{
				_logger.LogWarning("SendMulticastNotificationAsync called with a null message.");
				throw new ArgumentNullException(nameof(message)); // Hoặc return tùy logic xử lý lỗi
			}
			if (message.Tokens == null || !message.Tokens.Any())
			{
				_logger.LogWarning("SendMulticastNotificationAsync called with no tokens in the message.");
				return;
			}

			_logger.LogInformation("Attempting to send multicast notification to {TokenCount} devices.", message.Tokens.Count);
			 
			try
			{
				BatchResponse response = await _firebaseMessaging.SendEachForMulticastAsync(message);

				// Log kết quả (Quan trọng để debug)
				if (response.FailureCount > 0)
				{
					_logger.LogWarning("Failed to send notification to {FailureCount} out of {TotalCount} devices.",
									 response.FailureCount, message.Tokens.Count);
					int tokenIndex = 0;
					foreach (var resp in response.Responses)
					{
						if (!resp.IsSuccess)
						{
							// Lấy token tương ứng (có thể bị lỗi index nếu danh sách trả về không khớp thứ tự)
							string? failedToken = (tokenIndex < message.Tokens.Count) ? message.Tokens[tokenIndex] : "unknown token";
							_logger.LogError("    Failed token [{TokenIndex}] ({Token}): {ErrorCode} - {ErrorMessage}",
										   tokenIndex, failedToken, resp.Exception?.MessagingErrorCode, resp.Exception?.Message);
						}
						tokenIndex++;
					}
				}

				if (response.SuccessCount > 0)
	{
					_logger.LogInformation("Successfully sent notification to {SuccessCount} devices.", response.SuccessCount);
				}

			}
			catch (FirebaseMessagingException fcmEx)
			{
				_logger.LogError(fcmEx, "FirebaseMessagingException occurred while sending multicast notification. ErrorCode: {ErrorCode}", fcmEx.MessagingErrorCode);
				// Ném lại lỗi để tầng gọi (ví dụ ProductService) có thể biết và xử lý (nếu cần)
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unexpected error occurred while sending multicast notification.");
				throw; // Ném lại lỗi
			}
		}
	}
}
