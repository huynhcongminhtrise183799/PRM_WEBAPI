using FirebaseAdmin.Messaging;
using PRM.Application.IService;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class CartService : ICartService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICartRepository _cartRepository;
		private readonly IFirebaseService _firebaseService;
		public CartService(IUnitOfWork unitOfWork, ICartRepository cartRepository, IFirebaseService firebaseService)
		{
			_unitOfWork = unitOfWork;
			_cartRepository = cartRepository;
			_firebaseService = firebaseService;
		}

		public async Task<Cart> GetCartWithItemsAsync(Guid userId)
		{
			var cart = await _cartRepository.GetCartWithItemsAsync(userId);
			
			return cart;
		}

		public async Task<Cart> InitializeUserCartAsync(Guid userId)
		{
			var cart = await _cartRepository.GetOrCreateCartAsync(userId);
			if (cart.CartItems.Any())
			{
				var repo = _unitOfWork.Repository<UserDeviceToken>();
				var deviceTokens = await repo.GetAllAsync();
				var tokens = deviceTokens.Select(dt => dt.FCMToken).ToList();
				if (tokens.Any())
				{
					var message = new MulticastMessage()
					{
						Tokens = tokens,

						// Gửi data để Flutter xử lý hiển thị
						Data = new Dictionary<string, string>()
{
					{ "title", "Bạn có hàng trong giỏ hàng " },
					{ "body", $"Bạn có hàng trong giỏ hàng, nhấn để xem chi tiết!" },
					{ "screen", "/cart" },
					{ "userId", cart.UserId.ToString() }
},

						Android = new AndroidConfig
						{
							Priority = Priority.High,
							Notification = new AndroidNotification
							{
								ChannelId = "high_importance_channel", // trùng với channel Flutter
								Icon = "ic_stat_notification",          // tên icon trong mipmap
								Sound = "default",
								Title = "Bạn có hàng trong giỏ hàng",
								Body = $"Bạn có hàng trong giỏ hàng, nhấn để xem chi tiết!"
							},
							TimeToLive = TimeSpan.FromHours(1)
						}
					};
					await _firebaseService.SendMulticastNotificationAsync(message);
				}
			}
			return cart;
		}
	}
}
