using GroceryStoreKiosk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.FactoryProvider
{
    public interface IPriceCalculator
    {
        int ItemCountForDiscount { get; set; }
        void CalculatePrice(List<ScannedItem> scannedItems);
        void CalculatePrice(List<ScannedItem> items, Discount discount);
    }
}
