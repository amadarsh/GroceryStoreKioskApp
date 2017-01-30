using GroceryStoreKiosk.Model;
using GroceryStoreKiosk.Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.FactoryProvider
{
    public class BOGOPriceCalculator : IPriceCalculator
    {
        public int ItemCountForDiscount { get; set; }
        private DiscountDataAccessObject dao;
        public BOGOPriceCalculator()
        {
            ItemCountForDiscount = 2;
            dao = new DiscountDataAccessObject();
        }
        public virtual void CalculatePrice(List<ScannedItem> scannedItems)
        {
            var productId = scannedItems.ElementAt(0).ProductId;
            Discount discount = dao.GetBOGODiscountByProduct(productId);

            int iterationCount = scannedItems.Count / ItemCountForDiscount;
            for(int i = 1; i <= iterationCount; i= i+2)
            {
                var itemSet = ReturnSet(scannedItems, i);
                var percentDiscount = (100 - discount.DiscountPercentage) / 100;
                itemSet[ItemCountForDiscount - 1].DiscountedPrice = Math.Round((itemSet[1].Price * percentDiscount).Value, 2);
            }
        }

        private List<ScannedItem> ReturnSet(List<ScannedItem> scannedItems, int i)
        {
            return scannedItems.Skip(ItemCountForDiscount * (i - 1)).Take(ItemCountForDiscount).ToList();
        }

        public void CalculatePrice(List<ScannedItem> scannedItems, Discount discount)
        {
            int iterationCount = scannedItems.Count / ItemCountForDiscount;
            for (int i = 1; i <= iterationCount; i = i + 2)
            {
                var itemSet = ReturnSet(scannedItems, i);
                var percentDiscount = (100 - discount.DiscountPercentage) / 100;
                itemSet[ItemCountForDiscount - 1].DiscountedPrice = Math.Round((itemSet[1].Price * percentDiscount).Value, 2);
            }
        }
    }
}
