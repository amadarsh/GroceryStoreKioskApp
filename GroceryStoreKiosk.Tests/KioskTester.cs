using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryStoreKiosk.Model;
using System.Collections.Generic;
using GroceryStoreKiosk;

namespace GroceryStoreKiosk.Tests
{
    [TestClass]
    public class KioskTester
    {
        [TestMethod]
        public void TestOneScannedItem()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 0.50, null));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            Kiosk kiosk = new Kiosk(items, products, new List<Discount>());
            kiosk.CheckOut();

            foreach (var item in items)
            {
                Assert.AreEqual(1, item.ProductId);
                Assert.AreEqual(0.50, item.Price);
                Assert.AreEqual(null, item.DiscountedPrice);
            }
            Assert.AreEqual(0.50, kiosk.FinalBill);
        }

        [TestMethod]
        public void TestMultipleScannedItemOfSameProduct()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 0.50, null));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            Kiosk kiosk = new Kiosk(items, products, new List<Discount>());
            kiosk.CheckOut();

            foreach (var item in items)
            {
                Assert.AreEqual(1, item.ProductId);
                Assert.AreEqual(0.50, item.Price);
                Assert.AreEqual(null, item.DiscountedPrice);
            }
            Assert.AreEqual(1, kiosk.FinalBill);
        }

        [TestMethod]
        public void TestOneScannedItemOfDiffProduct()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 0.50, null));
            products.Add(ObjectCreator.CreateProduct(2, "Apple", 0.65, null));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Apple"));
            Kiosk kiosk = new Kiosk(items, products, new List<Discount>());
            kiosk.CheckOut();
            
            Assert.AreEqual(1, items[0].ProductId);
            Assert.AreEqual(0.50, items[0].Price);
            Assert.AreEqual(null, items[0].DiscountedPrice);

            Assert.AreEqual(2, items[1].ProductId);
            Assert.AreEqual(0.65, items[1].Price);
            Assert.AreEqual(null, items[1].DiscountedPrice);

            Assert.AreEqual(1.15, kiosk.FinalBill);
        }

        [TestMethod]
        public void TestMultipleScannedItemOfDiffProduct()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 0.50, null));
            products.Add(ObjectCreator.CreateProduct(2, "Apple", 0.65, null));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Apple"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Apple"));
            Kiosk kiosk = new Kiosk(items, products, new List<Discount>());
            kiosk.CheckOut();

            Assert.AreEqual(1, items[0].ProductId);
            Assert.AreEqual(0.50, items[0].Price);
            Assert.AreEqual(null, items[0].DiscountedPrice);

            Assert.AreEqual(2, items[1].ProductId);
            Assert.AreEqual(0.65, items[1].Price);
            Assert.AreEqual(null, items[1].DiscountedPrice);

            Assert.AreEqual(1, items[0].ProductId);
            Assert.AreEqual(0.50, items[0].Price);
            Assert.AreEqual(null, items[0].DiscountedPrice);

            Assert.AreEqual(2, items[1].ProductId);
            Assert.AreEqual(0.65, items[1].Price);
            Assert.AreEqual(null, items[1].DiscountedPrice);

            Assert.AreEqual(2.30, kiosk.FinalBill);
        }

        [TestMethod]
        public void TestOneScannedItemWithRegularDiscount()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 0.65, Constants.REGULAR_SALE));

            var discounts = new List<Discount>();
            discounts.Add(ObjectCreator.CreateDiscount(1, 0.50, 0, Constants.REGULAR_SALE, 0));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            Kiosk kiosk = new Kiosk(items, products, discounts);
            kiosk.CheckOut();

            Assert.AreEqual(1, items[0].ProductId);
            Assert.AreEqual(0.65, items[0].Price);
            Assert.AreEqual(0.50, items[0].DiscountedPrice);

            Assert.AreEqual(1, items[1].ProductId);
            Assert.AreEqual(0.65, items[1].Price);
            Assert.AreEqual(0.50, items[1].DiscountedPrice);

            Assert.AreEqual(1.00, kiosk.FinalBill);
        }

        [TestMethod]
        public void TestOneScannedItemWithBOGODiscount()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 0.65, Constants.DISCOUNT_BOGO));

            var discounts = new List<Discount>();
            discounts.Add(ObjectCreator.CreateDiscount(1, 0, 40, Constants.DISCOUNT_BOGO, 0));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            Kiosk kiosk = new Kiosk(items, products, discounts);
            kiosk.CheckOut();

            Assert.AreEqual(1, items[0].ProductId);
            Assert.AreEqual(0.65, items[0].Price);
            Assert.AreEqual(null, items[0].DiscountedPrice);

            Assert.AreEqual(1, items[1].ProductId);
            Assert.AreEqual(0.65, items[1].Price);
            Assert.AreEqual(0.39, items[1].DiscountedPrice);

            Assert.AreEqual(1.04, kiosk.FinalBill);
        }

        [TestMethod]
        public void TestOneScannedItemWithGroupDiscount()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 0.65, Constants.DISCOUNT_GROUP));

            var discounts = new List<Discount>();
            discounts.Add(ObjectCreator.CreateDiscount(1, 1.50, 0, Constants.DISCOUNT_GROUP, 3));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            Kiosk kiosk = new Kiosk(items, products, discounts);
            kiosk.CheckOut();

            Assert.AreEqual(1, items[0].ProductId);
            Assert.AreEqual(0.65, items[0].Price);
            Assert.AreEqual(0.50, items[0].DiscountedPrice);

            Assert.AreEqual(1, items[1].ProductId);
            Assert.AreEqual(0.65, items[1].Price);
            Assert.AreEqual(0.50, items[1].DiscountedPrice);

            Assert.AreEqual(1, items[2].ProductId);
            Assert.AreEqual(0.65, items[2].Price);
            Assert.AreEqual(0.50, items[2].DiscountedPrice);

            Assert.AreEqual(1, items[3].ProductId);
            Assert.AreEqual(0.65, items[3].Price);
            Assert.AreEqual(null, items[3].DiscountedPrice);

            Assert.AreEqual(2.15, kiosk.FinalBill);
        }

        [TestMethod]
        public void TestMultipleScannedItemOfDiffProductWithGroupDiscount()
        {
            var products = new List<Product>();
            products.Add(ObjectCreator.CreateProduct(1, "Banana", 1, Constants.DISCOUNT_GROUP));
            products.Add(ObjectCreator.CreateProduct(2, "Apple", 2, Constants.DISCOUNT_BOGO));
            products.Add(ObjectCreator.CreateProduct(4, "Orange", 3, Constants.REGULAR_SALE));
            products.Add(ObjectCreator.CreateProduct(3, "Mango", 4, null));

            var discounts = new List<Discount>();
            discounts.Add(ObjectCreator.CreateDiscount(1, 2, 0, Constants.DISCOUNT_GROUP, 3));
            discounts.Add(ObjectCreator.CreateDiscount(2, 0, 30, Constants.DISCOUNT_BOGO, 0));
            discounts.Add(ObjectCreator.CreateDiscount(4, 1.15, 0, Constants.REGULAR_SALE, 0));

            var items = new List<ScannedItem>();
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Banana"));
            items.Add(ObjectCreator.CreateScannedItem("Apple"));
            items.Add(ObjectCreator.CreateScannedItem("Apple"));
            items.Add(ObjectCreator.CreateScannedItem("Apple"));
            items.Add(ObjectCreator.CreateScannedItem("Orange"));
            items.Add(ObjectCreator.CreateScannedItem("Orange"));
            items.Add(ObjectCreator.CreateScannedItem("Mango"));
            Kiosk kiosk = new Kiosk(items, products, discounts);
            kiosk.CheckOut();

            //Banana
            Assert.AreEqual(1, items[0].ProductId);
            Assert.AreEqual(1, items[0].Price);
            Assert.AreEqual(0.67, items[0].DiscountedPrice);

            Assert.AreEqual(1, items[1].ProductId);
            Assert.AreEqual(1, items[1].Price);
            Assert.AreEqual(0.67, items[1].DiscountedPrice);

            Assert.AreEqual(1, items[2].ProductId);
            Assert.AreEqual(1, items[2].Price);
            Assert.AreEqual(0.67, items[2].DiscountedPrice);

            Assert.AreEqual(1, items[3].ProductId);
            Assert.AreEqual(1, items[3].Price);
            Assert.AreEqual(null, items[3].DiscountedPrice);
            //Apple
            Assert.AreEqual(2, items[4].ProductId);
            Assert.AreEqual(2, items[4].Price);
            Assert.AreEqual(null, items[4].DiscountedPrice);

            Assert.AreEqual(2, items[5].ProductId);
            Assert.AreEqual(2, items[5].Price);
            Assert.AreEqual(1.40, items[5].DiscountedPrice);

            Assert.AreEqual(2, items[6].ProductId);
            Assert.AreEqual(2, items[6].Price);
            Assert.AreEqual(null, items[6].DiscountedPrice);

            //Orange
            Assert.AreEqual(4, items[7].ProductId);
            Assert.AreEqual(3, items[7].Price);
            Assert.AreEqual(1.15, items[7].DiscountedPrice);

            Assert.AreEqual(4, items[8].ProductId);
            Assert.AreEqual(3, items[8].Price);
            Assert.AreEqual(1.15, items[8].DiscountedPrice);
            
            //Mango
            Assert.AreEqual(3, items[9].ProductId);
            Assert.AreEqual(4, items[9].Price);
            Assert.AreEqual(null, items[9].DiscountedPrice);

            Assert.AreEqual(14.71, kiosk.FinalBill);
        }
    }
}
