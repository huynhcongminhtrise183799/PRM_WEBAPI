using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
    public enum ProductImageStatus
    {
        Active,
        Inactive
    }
    public class ProductImage
    {
        public Guid ProductImageId { get; set; }

        public Guid ProductColorId { get; set; }

        public string ImageUrl { get; set; }

        public string Status { get; set; } // active | inactive

		public  ProductColors ProductColor { get; set; }



	}
}
