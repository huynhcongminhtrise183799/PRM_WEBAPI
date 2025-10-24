using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Auth
{
	public class UserResponseDto
	{
		public Guid UserId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
	}
}
