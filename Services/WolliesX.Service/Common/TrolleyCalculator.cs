using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WolliesX.Service.Models.v1.Trolley;

namespace WolliesX.Service.Common
{
	public static class TrolleyCalculator
	{
		public static double CalculateTrolley(Trolley trolley)
		{
            var total = 0.0;

            foreach (var product in trolley.Products)
            {
                var trolleyQuantity = trolley.Quantities.FirstOrDefault(q => q.Name == product.Name);
                var special = trolley.Specials.FirstOrDefault(q => q.Quantities.Any(y => y.Name == product.Name));

                if (special != null)
                {
                    var specialQuantities = special.Quantities.FirstOrDefault().Quantity;

                    if (trolleyQuantity.Quantity < specialQuantities)
                    {
                        total += product.Price * trolleyQuantity.Quantity;
                    }
                    else
                    {
                        while (trolleyQuantity.Quantity >= specialQuantities)
                        {
                            total += special.Total;
                            trolleyQuantity.Quantity -= specialQuantities;
                        }

                        total = total + (trolleyQuantity.Quantity > 0 ? product.Price * trolleyQuantity.Quantity : 0);
                    }
                }
                else
                {
                    total += trolleyQuantity.Quantity * product.Price;
                }
            }

            return total;
        }
	}
}
