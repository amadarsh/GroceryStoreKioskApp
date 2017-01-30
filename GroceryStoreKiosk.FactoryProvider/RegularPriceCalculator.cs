using GroceryStoreKiosk.Model;
using GroceryStoreKiosk.Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.FactoryProvider
{
    public class RegularPriceCalculator : IPriceCalculator
    {
        public int ItemCountForDiscount { get; set; }

        private DiscountDataAccessObject dao;
        public RegularPriceCalculator()
        {
            dao = new DiscountDataAccessObject();
        }
        public void CalculatePrice(List<ScannedItem> scannedItems)
        {
            var productId = scannedItems.ElementAt(0).ProductId;
            Discount discount = dao.GetRegularDiscountByProduct(productId);

            scannedItems.ForEach(item => item.DiscountedPrice = discount.DicountedPrice);
        }

        public void CalculatePrice(List<ScannedItem> scannedItems, Discount discount)
        {
            scannedItems.ForEach(item => item.DiscountedPrice = discount.DicountedPrice);
        }
    }
}
