using PRM.Application.Model;
using PRM.Application.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IProductService
	{

		Task<IEnumerable<Model.Product.ProductDto>> GetAllAsync();
		Task<Model.Product.ProductDto?> GetByIdAsync(Guid id);
		//Task<ProductWithCategory?> GetProductsWithCategoryAsync(Guid id);
		Task<(bool IsSuccess, string Message, Model.Product.ProductDto? Data)> CreateAsync(CreateProductDto dto);
		Task<(bool IsSuccess, string Message, Model.Product.ProductDto? Data)> UpdateAsync(Guid id, UpdateProductDto dto);
		Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id);
	}
}
