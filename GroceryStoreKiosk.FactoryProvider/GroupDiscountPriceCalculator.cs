using GroceryStoreKiosk.Model;
using GroceryStoreKiosk.Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.FactoryProvider
{
    public class GroupDiscountPriceCalculator : IPriceCalculator
    {
        public int ItemCountForDiscount { get; set; }
        private DiscountDataAccessObject dao;

        public GroupDiscountPriceCalculator()
        {
            dao = new DiscountDataAccessObject();
        }
        public void CalculatePrice(List<ScannedItem> scannedItems)
        {
            var productId = scannedItems.ElementAt(0).ProductId;
            Discount discount = dao.GetGroupDiscountByProduct(productId);

            ItemCountForDiscount = ((GroupDiscount)discount).ItemCount;

            int iterationCount = scannedItems.Count / ItemCountForDiscount;
            for (int i = 1; i <= iterationCount; i = i + 2)
            {
                var itemSet = ReturnSet(scannedItems, i);
                double totalItemsPrice = itemSet[1].Price * ItemCountForDiscount;
                double? discountApplied = Math.Round(((totalItemsPrice - discount.DicountedPrice)/ ItemCountForDiscount).Value,2);

                if(discountApplied > 0)
                {
                    itemSet.ForEach(item => item.DiscountedPrice = Math.Round((item.Price - discountApplied).Value, 2));
                }
            }
        }

        private List<ScannedItem> ReturnSet(List<ScannedItem> scannedItems, int i)
        {
            return scannedItems.Skip(ItemCountForDiscount * (i - 1)).Take(ItemCountForDiscount).ToList();
        }

        public void CalculatePrice(List<ScannedItem> scannedItems, Discount discount)
        {
            ItemCountForDiscount = ((GroupDiscount)discount).ItemCount;

            int iterationCount = scannedItems.Count / ItemCountForDiscount;
            for (int i = 1; i <= iterationCount; i = i + 2)
            {
                var itemSet = ReturnSet(scannedItems, i);
                double totalItemsPrice = itemSet[1].Price * ItemCountForDiscount;
                double? discountApplied = Math.Round(((totalItemsPrice - discount.DicountedPrice) / ItemCountForDiscount).Value, 2);

                if (discountApplied > 0)
                {
                    itemSet.ForEach(item => item.DiscountedPrice = Math.Round((item.Price - discountApplied).Value, 2));
                }
            }
        }
    }
}
