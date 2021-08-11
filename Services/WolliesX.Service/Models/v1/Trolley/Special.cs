using System;
using System.Collections.Generic;
using System.Text;

namespace WolliesX.Service.Models.v1.Trolley
{
	public class Special
	{
		public List<ProductQuantity> Quantities { get; set; }
		public double Total { get; set; }
	}
}
