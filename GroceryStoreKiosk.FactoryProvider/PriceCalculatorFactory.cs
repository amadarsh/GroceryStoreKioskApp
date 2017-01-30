using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.FactoryProvider
{
    public class PriceCalculatorFactory
    {
        /// <summary>
        /// This method is used to get the price calculator based on the discount Type (BOGO, GROUP and SALE) in this case.
        /// This acts as an Abstract Factory provider and can be extended to more discount types (like Boxing Day Sale, New year sale and so on) 
        /// New Discount types can be added to Factory without affecting Kiosk project.
        /// </summary>
        /// <param name="discountType">Type of discount</param>
        /// <returns>PriceCalculator</returns>
        public static IPriceCalculator GetPriceCalculator(string discountType)
        {
            IPriceCalculator priceCalculator = null;
            switch (discountType)
            {
                case "BOGO":
                    priceCalculator = new BOGOPriceCalculator();
                    break;
                case "GROUP":
                    priceCalculator = new GroupDiscountPriceCalculator();
                    break;
                case "SALE":
                    priceCalculator = new RegularPriceCalculator();
                    break;
            }
            return priceCalculator;
        }
    }
}
