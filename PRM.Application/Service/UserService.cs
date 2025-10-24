using PRM.Application.Common;
using PRM.Application.IService;
using PRM.Application.Model.Auth;
using PRM.Domain.Entities;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Email))
				throw new InvalidException("Email không được để trống.", 400, "EMAIL_REQUIRED");

			if (string.IsNullOrWhiteSpace(dto.Password))
				throw new InvalidException("Mật khẩu không được để trống.", 400, "PASSWORD_REQUIRED");

			if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
				throw new InvalidException("Vui lòng xác nhận mật khẩu.", 400, "CONFIRM_PASSWORD_REQUIRED");

			if (dto.Password != dto.ConfirmPassword)
				throw new InvalidException("Mật khẩu xác nhận không khớp.", 400, "PASSWORD_MISMATCH");

			if (string.IsNullOrWhiteSpace(dto.FullName))
				throw new InvalidException("Họ tên không được để trống.", 400, "FULLNAME_REQUIRED");

			if (string.IsNullOrWhiteSpace(dto.Phone))
				throw new InvalidException("Số điện thoại không được để trống.", 400, "PHONE_REQUIRED");

			var userRepo = _unitOfWork.Repository<User>();

			var existing = (await userRepo.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
			if (existing != null)
				throw new AppException("Email này đã được đăng ký.", 400, "EMAIL_EXISTS");

			var hashedPassword = HashPassword(dto.Password);

			var user = new User
			{
				UserId = Guid.NewGuid(),
				Email = dto.Email.Trim(),
				Password = hashedPassword,
				FullName = dto.FullName.Trim(),
				Phone = dto.Phone.Trim(),
				Role = UserRole.Customer.ToString(),
				Status = UserStatus.Active.ToString()
			};

			await userRepo.AddAsync(user);
			await _unitOfWork.SaveChangesAsync();

			return new UserResponseDto
			{
				UserId = user.UserId,
				Email = user.Email,
				FullName = user.FullName,
				Role = user.Role,
				Status = user.Status
			};
		}

		public async Task<UserResponseDto> LoginAsync(LoginDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Email))
				throw new InvalidException("Email không được để trống.", 400, "EMAIL_REQUIRED");

			if (string.IsNullOrWhiteSpace(dto.Password))
				throw new InvalidException("Mật khẩu không được để trống.", 400, "PASSWORD_REQUIRED");

			var userRepo = _unitOfWork.Repository<User>();
			var user = (await userRepo.FindAsync(u => u.Email == dto.Email.Trim())).FirstOrDefault();

			if (user == null)
				throw new AppException("Email không tồn tại.", 404, "EMAIL_NOT_FOUND");

			if (user.Status == UserStatus.Inactive.ToString())
				throw new AppException("Tài khoản đang bị vô hiệu hóa.", 403, "ACCOUNT_INACTIVE");

			if (user.Status == UserStatus.Banned.ToString())
				throw new AppException("Tài khoản đã bị cấm.", 403, "ACCOUNT_BANNED");

			if (!VerifyPassword(dto.Password, user.Password))
				throw new AppException("Mật khẩu không chính xác.", 401, "INVALID_PASSWORD");

			return new UserResponseDto
			{
				UserId = user.UserId,
				Email = user.Email,
				FullName = user.FullName,
				Role = user.Role,
				Status = user.Status
			};
		}

		public async Task<IEnumerable<ProfileResponseDto>> GetAllUserInformationAsync()
		{
			var userRepo = _unitOfWork.Repository<User>();
			var users = await userRepo.GetAllAsync();

			if (users == null || !users.Any())
				throw new AppException("Không có người dùng nào trong hệ thống.", 404, "NO_USERS_FOUND");

			return users.Select(user => new ProfileResponseDto
			{
				UserId = user.UserId,
				Email = user.Email,
				FullName = user.FullName,
				Phone = user.Phone,
				Role = user.Role,
				Status = user.Status
			}).ToList();
		}


		public async Task<ProfileResponseDto> GetUserInformationAsync(Guid userId)
		{
			var userRepo = _unitOfWork.Repository<User>();
			var user = await userRepo.GetByIdAsync(userId);

			if (user == null)
				throw new AppException("Người dùng không tồn tại.", 404, "USER_NOT_FOUND");

			return new ProfileResponseDto
			{
				UserId = user.UserId,
				Email = user.Email,
				FullName = user.FullName,
				Phone = user.Phone,
				Role = user.Role,
				Status = user.Status
			};
		}
		public Task LogoutAsync()
		{
			return Task.CompletedTask;
		}
		private static string HashPassword(string password)
		{
			using var sha = SHA256.Create();
			var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(bytes);
		}

		private static bool VerifyPassword(string inputPassword, string hashedPassword)
		{
			var hash = HashPassword(inputPassword);
			return hash == hashedPassword;
		}

		public async Task<UserResponseDto> GetAdmin()
		{
			var userRepo = _unitOfWork.Repository<User>();
			var admin = await userRepo.GetAsync(u => u.Role == UserRole.Admin.ToString());
			if (admin == null)
				throw new AppException("Admin not found.", 404, "ADMIN_NOT_FOUND");
			var adminDto = new UserResponseDto
			{
				UserId = admin.UserId,
				Email = admin.Email,
				FullName = admin.FullName,
				Role = admin.Role,
				Status = admin.Status
			};
			return adminDto;
		}
	}
}
