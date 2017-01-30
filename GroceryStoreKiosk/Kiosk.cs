using GroceryStoreKiosk.FactoryProvider;
using GroceryStoreKiosk.Model;
using GroceryStoreKiosk.Model.DAL;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GroceryStoreKiosk
{
    public class Kiosk
    {
        private ProductDataAccessObject productDAO;
        private DiscountDataAccessObject discountDAO;
        private IPriceCalculator calculator;

        private List<ScannedItem> scannedItems;
        public double FinalBill { get; set; }

        private List<Product> _products;

        public List<Product> Products
        {
            get
            {
                return _products == null ? GetAllProducts() : _products;
            }

            set
            {
                _products = value;
            }
        }

        private List<Discount> _discounts;

        public List<Discount> Discounts
        {
            get
            {
                return _discounts == null ? GetAllDiscounts() : _discounts;
            }

            set
            {
                _discounts = value;
            }
        }
        /// <summary>
        /// Main constructor 
        /// </summary>
        /// <param name="items"></param>
        public Kiosk(List<ScannedItem> items)
        {
            productDAO = new ProductDataAccessObject();
            discountDAO = new DiscountDataAccessObject();
            scannedItems = items;
        }

        /// <summary>
        /// Kiosk constructor called from the Unit tests
        /// </summary>
        /// <param name="items"></param>
        /// <param name="products"></param>
        /// <param name="discounts"></param>
        public Kiosk(List<ScannedItem> items, List<Product> products, List<Discount> discounts) : this(items)
        {
            this.Products = products;
            this.Discounts = discounts;
        }

        /// <summary>
        /// This method makes the database call and fetches the product details
        /// </summary>
        /// <returns></returns>
        private List<Product> GetAllProducts()
        {
            return productDAO.GetAllProducts();
        }

        /// <summary>
        /// This method makes the database call and fetches the discount details
        /// </summary>
        /// <returns></returns>
        private List<Discount> GetAllDiscounts()
        {
            return discountDAO.GetAllDiscounts();
        }

        /// <summary>
        /// This method applies the appropriate discounts on each of the scanned item
        /// The discount is applied by invoking the corresponding Discount Type interface
        /// </summary>
        public void CheckOut()
        {
            try
            {
                //Assign product details to each scanned item
                AssignProductDetailsToScannedItems();
                //get unique scanned items
                var uniqueProductIds = scannedItems.Select(i => i.ProductId).Distinct().ToList();

                //Apply the discount for each set of unique scanned items
                uniqueProductIds.ForEach(pId =>
                {
                    //If the scanned item is not present in the Product catalog then price will not be calculated
                    //If the scanned item is not present in the Product catalog then Product id = 0
                    if (pId != 0)
                    {
                        var items = scannedItems.FindAll(item => item.ProductId == pId);
                        var discountType = Products.Find(p => p.ProductId == pId).DiscountType;

                        calculator = PriceCalculatorFactory.GetPriceCalculator(discountType);
                        if (calculator != null)
                        {
                            var discount = Discounts.Find(dis => dis.ProductId == pId);
                            //Call the Calculator
                            calculator.CalculatePrice(items, discount);
                        }
                    }
                });
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Exception during Checkout process", ex);
            }

            PrintReceipt();
        }

        /// <summary>
        /// This method assigns the corresponding product details to the ScannedItems
        /// </summary>
        private void AssignProductDetailsToScannedItems()
        {
            scannedItems.ForEach(item =>
            {
                var product = Products.Find(p => p.Name.ToUpper() == item.Name.ToUpper());
                if (product != null)
                {
                    item.ProductId = product.ProductId;
                    item.Price = product.Price;
                }
            });
        }

        /// <summary>
        /// This method prints the itemized summary with the Product Id, Product name, Actual price and the discounted price if any
        /// This method provides the total cost and the total savings after the discounts
        /// </summary>
        private void PrintReceipt()
        {
            Console.WriteLine("Your receipt:");
            Console.WriteLine("  Product ID Item            Base Price Discounted Price");
            scannedItems.ForEach(item =>
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}",
                    string.Format("{0}", item.ProductId).PadLeft(12),
                    string.Format("{0}", item.Name).PadRight(15),
                    (item.Price == 0 ? "NA" : string.Format(" {0:c}", item.Price)).PadLeft(10),
                    (item.DiscountedPrice.HasValue ? string.Format("{0:c}", item.DiscountedPrice.Value) : "-").PadLeft(16)));                
            });

            FinalBill = scannedItems.Select(item => item.DiscountedPrice.HasValue ? item.DiscountedPrice.Value : item.Price).Sum();
            double BillWithoutDiscount = scannedItems.Select(item => item.Price).Sum();
            Console.WriteLine("Total bill: " + string.Format("{0:c}", FinalBill));
            Console.WriteLine("Your savings: " + string.Format("{0:c}", BillWithoutDiscount - FinalBill));
        }
    }
}
