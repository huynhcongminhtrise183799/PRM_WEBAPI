using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Model
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public List<string> ImageUrls { get; set; } = new();

    }

    public class CategoryWithProductsDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }
}
