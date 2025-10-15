using Microsoft.EntityFrameworkCore;
using PRM.Application.Interfaces.Repositories;
using PRM.Domain.Entities;
using PRM.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PRM.Infrastructure.Repositories
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        private readonly PRMDbContext _context;

        public VoucherRepository(PRMDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Voucher?> GetByCodeAsync(string code)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code);
        }

        public async Task<bool> IsDuplicateCodeAsync(string code, Guid? excludeId = null)
        {
            return await _context.Vouchers
                .AnyAsync(v => v.Code.ToLower() == code.ToLower() && (!excludeId.HasValue || v.VoucherId != excludeId.Value));
        }
    }
}
