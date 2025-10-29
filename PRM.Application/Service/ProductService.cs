using FirebaseAdmin.Messaging;
using PRM.Application.IService;
using PRM.Application.Model;
using PRM.Application.Model.Color;
using PRM.Application.Model.Img;
using PRM.Application.Model.Product;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PRM.Application.Service
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IProductRepository _productRepository;
		private readonly IFirebaseService _firebaseService;

		public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, IFirebaseService firebaseService)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_firebaseService = firebaseService;
		}

		public async Task<(bool IsSuccess, string Message, Model.Product.ProductDto? Data)> CreateAsync(CreateProductDto dto)
		{
			try
			{
				var product = new Product
				{
					ProductId = Guid.NewGuid(),
					Name = dto.Name,
					Description = dto.Description,
					WeightKg = dto.WeightKg,
					CategoryId = dto.CategoryId,
					SupplierId = dto.SupplierId,
					Status = "active",
					ProductColors = new List<ProductColors>()
				};

				await _unitOfWork.Repository<Product>().AddAsync(product);


				foreach (var colorDto in dto.ProductColors)
				{
					var color = new ProductColors
					{
						ProductColorsId = Guid.NewGuid(),
						ProductId = product.ProductId,
						ColorName = colorDto.ColorName,
						ColorCode = colorDto.ColorCode,
						Stock = colorDto.Stock,
						Price = colorDto.Price,
						Status = "active"
					};
					await _unitOfWork.Repository<ProductColors>().AddAsync(color);


					// Thêm ProductImages
					if (colorDto.ProductImages != null && colorDto.ProductImages.Any())
					{
						foreach (var imgDto in colorDto.ProductImages)
						{
							var image = new ProductImage
							{
								ProductImageId = Guid.NewGuid(),
								ProductColorId = color.ProductColorsId,
								ImageUrl = imgDto.ImageUrl, // lấy từ ProductImageDto
								Status = "active"
							};
							await _unitOfWork.Repository<ProductImage>().AddAsync(image);
						}
					}
				}
				await _unitOfWork.SaveChangesAsync();
				var category = await _unitOfWork.Repository<Category>().GetByIdAsync(product.CategoryId);
				var supplier = await _unitOfWork.Repository<Suppliers>().GetByIdAsync(product.SupplierId);

				var repo = _unitOfWork.Repository<UserDeviceToken>();
				var deviceTokens = await repo.GetAllAsync();
				var tokens = deviceTokens.Select(dt => dt.FCMToken).ToList();
				if (tokens.Any())
				{
					var message = new MulticastMessage()
					{
						Tokens = tokens,

						// Gửi data để Flutter xử lý hiển thị
						Data = new Dictionary<string, string>()
{
					{ "title", "🎉 Sản phẩm mới đã về!" },
					{ "body", $"Khám phá ngay: {product.Name}" },
					{ "screen", "/productDetail" },
					{ "productId", product.ProductId.ToString() }
},

						Android = new AndroidConfig
						{
							Priority = Priority.High,
							Notification = new AndroidNotification
							{
								ChannelId = "high_importance_channel", // trùng với channel Flutter
								Icon = "ic_stat_notification",          // tên icon trong mipmap
								Sound = "default",
								Title = "🎉 Sản phẩm mới đã về!",
								Body = $"Khám phá ngay: {product.Name}"
							},
							TimeToLive = TimeSpan.FromHours(1)
						}
					};

					// Gửi đi
					await _firebaseService.SendMulticastNotificationAsync(message);
				}
				var result = new Model.Product.ProductDto
				{
					ProductId = product.ProductId,
					Name = product.Name,
					Description = product.Description,
					WeightKg = product.WeightKg,
					CategoryId = product.CategoryId,
					SupplierId = product.SupplierId,
					Status = product.Status,
					ProductColors = product.ProductColors?.Select(pc => new ProductColorDto
					{
						ProductColorsId = pc.ProductColorsId,
						ColorName = pc.ColorName,
						ColorCode = pc.ColorCode,
						Stock = pc.Stock,
						Price = pc.Price,
						Status = pc.Status,
						ProductImages = pc.ProductImages?.Select(img => new ProductImageDto
						{
							ProductImageId = img.ProductImageId,
							ImageUrl = img.ImageUrl,
							Status = img.Status
						}).ToList()
					}).ToList(),
					CategoryName = product.Category?.Name,
					SupplierName = product.Supplier?.Name
				};
				return (true, "Product created successfully.", result);
			}
			catch (Exception ex)
			{
				return (false, $"Error: {ex.Message}", null);
			}
		}

		public async Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id)
		{
			var repo =  _unitOfWork.Repository<Product>();
			var product = await repo.GetByIdAsync(id);
			if (product == null)
				return (false,"Product not found");
			if (product.Status == "inactive")
				return (false,"Product is delete");
			product.Status = "inactive";
			repo.Update(product);
			await _unitOfWork.SaveChangesAsync();

			return (true, "Product delete Successfully");
		}

		public async Task<IEnumerable<Model.Product.ProductDto>> GetAllAsync()
		{
			var products = await _productRepository.GetAllWithDetailsAsync();

			return products.Select(p => new Model.Product.ProductDto
			{
				ProductId = p.ProductId,
				Name = p.Name,
				Description = p.Description,
				WeightKg = p.WeightKg,
				CategoryId = p.CategoryId,
				SupplierId = p.SupplierId,
				Status = p.Status,
				CategoryName = p.Category?.Name,
				SupplierName = p.Supplier?.Name,
				ProductColors = p.ProductColors?.Select(pc => new ProductColorDto
				{
					ProductColorsId = pc.ProductColorsId,
					ColorName = pc.ColorName,
					ColorCode = pc.ColorCode,
					Stock = pc.Stock,
					Price = pc.Price,
					Status = pc.Status,
					ProductImages = pc.ProductImages?.Select(img => new ProductImageDto
					{
						ProductImageId = img.ProductImageId,
						ImageUrl = img.ImageUrl,
						Status = img.Status
					}).ToList()
				}).ToList()
			}).ToList();
		}

		public async Task<Model.Product.ProductDto?> GetByIdAsync(Guid id)
		{
			var product = await _productRepository.GetByIdWithDetailsAsync(id);
			if (product == null) return null;


			return new Model.Product.ProductDto
			{
				ProductId = product.ProductId,
				Name = product.Name,
				Description = product.Description,
				WeightKg = product.WeightKg,
				CategoryId = product.CategoryId,
				SupplierId = product.SupplierId,
				Status = product.Status,
				CategoryName = product.Category?.Name,
				SupplierName = product.Supplier?.Name,
				ProductColors = product.ProductColors?.Select(pc => new ProductColorDto
				{
					ProductColorsId = pc.ProductColorsId,
					ColorName = pc.ColorName,
					ColorCode = pc.ColorCode,
					Stock = pc.Stock,
					Price = pc.Price,
					Status = pc.Status,
					ProductImages = pc.ProductImages?.Select(img => new ProductImageDto
					{
						ProductImageId = img.ProductImageId,
						ImageUrl = img.ImageUrl,
						Status = img.Status
					}).ToList()
				}).ToList()
			};
		}

		public async Task<(bool IsSuccess, string Message, Model.Product.ProductDto? Data)> UpdateAsync(Guid id, UpdateProductDto dto)
		{
			try
			{

				var product = await _productRepository.GetByIdWithDetailsAsync(id);

				if (product == null)
					return (false, "Không tìm thấy sản phẩm.", null);

				// Cập nhật thông tin cơ bản
				product.Name = dto.Name ?? product.Name;
				product.Description = dto.Description ?? product.Description;
				product.WeightKg = dto.WeightKg != 0 ? dto.WeightKg : product.WeightKg;
				product.CategoryId = dto.CategoryId != Guid.Empty ? dto.CategoryId : product.CategoryId;
				product.SupplierId = dto.SupplierId != Guid.Empty ? dto.SupplierId : product.SupplierId;
				product.Status = dto.Status ?? product.Status;

				
					if (dto.ProductColors != null && dto.ProductColors.Any())
				{
				
					_unitOfWork.Repository<ProductColors>().DeleteRange(product.ProductColors);

					foreach (var colorDto in dto.ProductColors)
					{
						var newColor = new ProductColors
						{
							ProductColorsId = Guid.NewGuid(),
							ProductId = product.ProductId,
							ColorName = colorDto.ColorName,
							ColorCode = colorDto.ColorCode,
							Stock = colorDto.Stock,
							Price = colorDto.Price,
							Status = "active",
							ProductImages = new List<ProductImage>()
						};

						if (colorDto.ProductImages != null && colorDto.ProductImages.Any())
						{
							foreach (var imgDto in colorDto.ProductImages)
							{
								newColor.ProductImages.Add(new ProductImage
								{
									ProductImageId = Guid.NewGuid(),
									ProductColorId = newColor.ProductColorsId,
									ImageUrl = imgDto.ImageUrl,
									Status = "active"
								});
							}
						}

						await _unitOfWork.Repository<ProductColors>().AddAsync(newColor);
					}
				}

				// Lưu thay đổi
				await _unitOfWork.SaveChangesAsync();

				// Map lại DTO kết quả
				var result = new Model.Product.ProductDto
				{
					ProductId = product.ProductId,
					Name = product.Name,
					Description = product.Description,
					WeightKg = product.WeightKg,
					CategoryId = product.CategoryId,
					SupplierId = product.SupplierId,
					Status = product.Status,
					CategoryName = product.Category?.Name,
					SupplierName = product.Supplier?.Name,
					ProductColors = product.ProductColors?.Select(pc => new ProductColorDto
					{
						ProductColorsId = pc.ProductColorsId,
						ColorName = pc.ColorName,
						ColorCode = pc.ColorCode,
						Stock = pc.Stock,
						Price = pc.Price,
						Status = pc.Status,
						ProductImages = pc.ProductImages?.Select(img => new ProductImageDto
						{
							ProductImageId = img.ProductImageId,
							ImageUrl = img.ImageUrl,
							Status = img.Status
						}).ToList()
					}).ToList(),
				};

				return (true, "Cập nhật sản phẩm thành công.", result);
			}
			catch (Exception ex)
			{
				return (false, $"Lỗi khi cập nhật sản phẩm: {ex.Message}", null);
			}
		}
	}
}
