using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface ICartService
	{
		Task<Cart> InitializeUserCartAsync(Guid userId);
		Task<Cart> GetCartWithItemsAsync(Guid userId);
	}
}
