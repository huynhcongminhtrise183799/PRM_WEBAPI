using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;
using PRM.Application.Model.Product;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var products = await _productService.GetAllAsync();
			return Ok(products);
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var product = await _productService.GetByIdAsync(id);
			if (product == null)
				return NotFound(new { Message = "Product not found" });

			return Ok(product);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _productService.CreateAsync(dto);
			if (!result.IsSuccess)
				return BadRequest(new { result.Message });

			return Ok(result.Data);
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _productService.UpdateAsync(id, dto);
			if (!result.IsSuccess)
				return BadRequest(new { result.Message });

			return Ok(result.Data);
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await _productService.DeleteAsync(id);
			if (!result.IsSuccess)
				return BadRequest(new { result.Message });

			return Ok(new { result.Message });
		}

	}
}
