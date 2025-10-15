
namespace PRM.Application.Model
{
    public class VoucherDto
    {
        public Guid VoucherId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
        public string Status { get; set; }
    }

    public class CreateVoucherDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
    }

    public class UpdateVoucherDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
        public string Status { get; set; }
    }
}
