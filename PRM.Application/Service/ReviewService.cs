using PRM.Application.IService;
using PRM.Application.Model.Review;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class ReviewService : IReviewService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IReviewRepository _reviewRepository;

		public ReviewService(IUnitOfWork unitOfWork, IReviewRepository reviewRepository)
		{
			_unitOfWork = unitOfWork;
			_reviewRepository = reviewRepository;
		}

		public async Task<(bool IsSuccess, string Message, ReviewDto? Data)> CreateAsync(CreateReviewDto dto)
		{
			try
			{
				var review = new Review
				{
					ReviewId = Guid.NewGuid(),
					ProductId = dto.ProductId,
					UserId = dto.UserId,
					Rating = dto.Rating,
					Details = dto.Details,
					ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow)
				};

				await _unitOfWork.Repository<Review>().AddAsync(review);
				await _unitOfWork.SaveChangesAsync();

				// Load user & product
				var user = await _unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);
				var product = await _unitOfWork.Repository<Product>().GetByIdAsync(dto.ProductId);

				var result = new ReviewDto
				{
					ReviewId = review.ReviewId,
					Rating = review.Rating,
					Details = review.Details,
					ReviewDate = review.ReviewDate,
					UserId = user.UserId,
					Email = user.Email,
					ProductId = product.ProductId,

					User = new MinimalUserDto
					{
						UserId = user?.UserId ?? Guid.Empty,
						Email = user?.Email
					},

					Product = new MinimalProductDto
					{
						ProductId = product?.ProductId ?? Guid.Empty,
						Name = product?.Name
					}
				};
				return (true, "Review created successfully.", result);
			}
			catch (Exception ex)
			{
				return (false, $"Error: {ex.Message}", null);
			}
		}

		public async Task<(bool IsSuccess, string Message)> DeleteAsync(Guid reviewId)
		{
			try
			{
				var review = await _unitOfWork.Repository<Review>().GetByIdAsync(reviewId);
				if (review == null)
					return (false, "Không tìm thấy đánh giá để xóa.");

				_unitOfWork.Repository<Review>().Remove(review);
				await _unitOfWork.SaveChangesAsync();

				return (true, "Xóa đánh giá thành công.");
			}
			catch (Exception ex)
			{
				return (false, $"Lỗi khi xóa đánh giá: {ex.Message}");
			}
		}

		public async Task<IEnumerable<ReviewDto>> GetByProductIdAsync(Guid productId)
		{
			var reviews = await _reviewRepository.GetByProductIdWithIncludeAsync(productId);

			return reviews.Select(r => new ReviewDto
			{
				ReviewId = r.ReviewId,
				Rating = r.Rating,
				Details = r.Details,
				ReviewDate = r.ReviewDate,
				UserId = r.UserId,
				Email = r.User?.Email,
				ProductId = r.ProductId,

				User = new MinimalUserDto
				{
					UserId = r.User?.UserId ?? Guid.Empty,
					Email = r.User?.Email
				},
				Product = new MinimalProductDto
				{
					ProductId = r.Product?.ProductId ?? Guid.Empty,
					Name = r.Product?.Name
				}
			}).ToList();

		}
	}
}
