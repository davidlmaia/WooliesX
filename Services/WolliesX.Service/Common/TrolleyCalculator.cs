using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WolliesX.Service.Models.v1.Trolley;

namespace WolliesX.Service.Common
{
	public static class TrolleyCalculator
	{
        
        //Would have to double check the requirements for this.
        //The way this is calculating is checking if the Product has specials. If it does it will use the value according to the qty informed in quantitites object to 
        

        //Ex:

        // Product A, cost $10. There is a special of 2 for $15. But in quantities Object (assuming this is the trolley total amount for that prodct) has 3 items. 
        // Code will calculate 2 for $15 + 1 for $10, giving a total back for Product A as $25.

        // Product B cost $100 and there is no special for it. Trolly quantity is set to 1. Code will calculate 1 for $100, giving the total back as $100

        // Code would then sum all products returning $125

		public static double CalculateTrolley(Trolley trolley)
		{
            var total = 0.0;

            foreach (var product in trolley.Products)
            {
                var totalProduct = 0.0;

                var trolleyQuantity = trolley.Quantities.FirstOrDefault(q => q.Name == product.Name);
                var specialForProduct = trolley.Specials.FirstOrDefault(q => q.Quantities.Any(y => y.Name == product.Name));

                if (specialForProduct != null)
                {
                    var specialQuantities = specialForProduct.Quantities.FirstOrDefault().Quantity;

                    if (trolleyQuantity.Quantity < specialQuantities)
                    {
                        totalProduct += product.Price * trolleyQuantity.Quantity;
                    }
                    else
                    {
                        while (trolleyQuantity.Quantity >= specialQuantities)
                        {
                            totalProduct += specialForProduct.Total;
                            trolleyQuantity.Quantity -= specialQuantities;
                        }

                        totalProduct = totalProduct + (trolleyQuantity.Quantity > 0 ? product.Price * trolleyQuantity.Quantity : 0);
                    }
                }
                else
                {
                    totalProduct += trolleyQuantity.Quantity * product.Price;
                }

                total += totalProduct;
            }

            return total;
        }
	}
}
