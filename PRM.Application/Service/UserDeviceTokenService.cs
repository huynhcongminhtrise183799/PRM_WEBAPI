using PRM.Application.IService;
using PRM.Application.Model;
using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class UserDeviceTokenService : IUserDeviceTokenService
	{
		private readonly IUnitOfWork _unitOfWork;
		public UserDeviceTokenService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<bool> AddToken(UserDeviceTokenModel userDeviceToken)
		{
			var entity = new UserDeviceToken
			{
				UserDeviceTokenId = Guid.NewGuid(),
				UserId = userDeviceToken.UserId,
				FCMToken = userDeviceToken.FCMToken,
			};
			var repo = _unitOfWork.Repository<UserDeviceToken>();
			await repo.AddAsync(entity);
			var result = await _unitOfWork.SaveChangesAsync();
			return result > 0;
		}
	}
}
