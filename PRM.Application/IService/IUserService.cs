using PRM.Application.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IUserService
	{
		Task<UserResponseDto> RegisterAsync(RegisterDto dto);
		Task<UserResponseDto> LoginAsync(LoginDto dto);
		Task<IEnumerable<ProfileResponseDto>> GetAllUserInformationAsync();
		Task<ProfileResponseDto> GetUserInformationAsync(Guid userId);
		Task LogoutAsync();

		Task<UserResponseDto> GetAdmin();
	}
}
