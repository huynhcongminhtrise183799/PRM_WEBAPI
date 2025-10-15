using Microsoft.AspNetCore.Mvc;
using PRM.Application.Interfaces;
using PRM.Application.Model;
namespace PRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _voucherService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _voucherService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { Message = "Voucher not found" });

            return Ok(result);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _voucherService.GetByCodeAsync(code);
            if (result == null)
                return NotFound(new { Message = "Voucher not found" });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVoucherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message, data) = await _voucherService.CreateAsync(dto);
            if (!isSuccess)
                return BadRequest(new { Message = message });

            return CreatedAtAction(nameof(GetById), new { id = data.VoucherId }, data);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVoucherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message, data) = await _voucherService.UpdateAsync(id, dto);
            if (!isSuccess)
                return BadRequest(new { Message = message });

            return Ok(data);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var (isSuccess, message) = await _voucherService.DeleteAsync(id);
            if (!isSuccess)
                return NotFound(new { Message = message });

            return Ok(new { Message = message });
        }
    }
}
