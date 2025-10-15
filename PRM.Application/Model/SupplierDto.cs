
namespace PRM.Application.Model
{
    public class SupplierDto
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
    }

    public class CreateSupplierDto
    {
        public string Name { get; set; }
    }

    public class UpdateSupplierDto
    {
        public string Name { get; set; }
    }

    public class SupplierWithProductsDto
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public List<ProductDto> Products { get; set; }
    }

}
