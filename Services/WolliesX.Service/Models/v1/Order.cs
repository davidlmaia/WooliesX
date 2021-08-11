using System;
using System.Collections.Generic;
using System.Text;

namespace WolliesX.Service.Models.v1
{
	public class Order
	{
		public int CustomerId { get; set; }
		public IEnumerable<Product> Products { get; set; }
	}
}
