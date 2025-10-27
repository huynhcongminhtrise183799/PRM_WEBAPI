using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model.Auth
{
	public class UpdateProfileRequestDto
	{
		public string FullName { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;

	}
}
