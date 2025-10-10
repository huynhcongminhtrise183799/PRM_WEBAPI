using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	/*
	  id int [pk, increment]
  name varchar [not null]
  contact_name varchar
  phone varchar
  email varchar
  address text
  created_at timestamp*/
	public class Suppliers
	{
		public Guid SupplierId { get; set; }

		public string Name { get; set; }

		public virtual ICollection<Product> Products { get; set; }

	}
}
