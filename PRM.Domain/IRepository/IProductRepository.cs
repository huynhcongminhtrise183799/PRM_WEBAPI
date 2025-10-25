using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.IRepository
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		Task<IEnumerable<Product>> GetAllWithDetailsAsync();
		Task<Product?> GetByIdWithDetailsAsync(Guid id);
	}
}
