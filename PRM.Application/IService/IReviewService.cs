using PRM.Application.Model.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IReviewService
	{
		Task<(bool IsSuccess, string Message, ReviewDto? Data)> CreateAsync(CreateReviewDto dto);
		Task<IEnumerable<ReviewDto>> GetByProductIdAsync(Guid productId);
		Task<(bool IsSuccess, string Message)> DeleteAsync(Guid reviewId);
	}
}
