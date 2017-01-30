namespace GroceryStoreKiosk.Model
{
    public class ScannedItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
    }
}