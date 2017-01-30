namespace GroceryStoreKiosk.Model
{
    public class Discount
    {
        public int ProductId { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? DicountedPrice { get; set; }
    }
    public class GroupDiscount: Discount
    {
        public int ItemCount { get; set; }
    }
}