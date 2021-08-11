using System;
using System.Collections.Generic;
using System.Text;

namespace WolliesX.Service.Models.v1.Trolley
{
	public class Trolley
	{
		public List<TrolleyProduct> Products { get; set; }
		public List<Special> Specials { get; set; }
		public List<ProductQuantity> Quantities { get; set; }
	}
}
