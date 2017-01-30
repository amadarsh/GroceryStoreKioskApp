using System;
using GroceryStoreKiosk.Model;

namespace GroceryStoreKiosk.Tests
{
    internal class ObjectCreator
    {
        internal static Product CreateProduct(int productId, string productName, double productPrice, string discountType)
        {
            return new Product { ProductId = productId, Name = productName, Price = productPrice, DiscountType = discountType };
        }
        internal static Discount CreateDiscount(int productId, double discountPrice, double discountPercent, string discountType, int itemCount)
        {
            if (discountType == Constants.DISCOUNT_GROUP)
            {
                return new GroupDiscount { ProductId = productId, DicountedPrice = discountPrice, DiscountPercentage = discountPercent, ItemCount = itemCount };
            }
            else
            {
                return new Discount { ProductId = productId, DicountedPrice = discountPrice, DiscountPercentage = discountPercent };
            }
        }

        internal static ScannedItem CreateScannedItem(string productName)
        {
            return new ScannedItem { Name = productName};
        }
    }
}