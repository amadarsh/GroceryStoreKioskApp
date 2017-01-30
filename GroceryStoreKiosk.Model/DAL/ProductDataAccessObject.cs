using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.Model.DAL
{
    public class ProductDataAccessObject
    {
        private string productFileCatalogLocation = ConfigurationSettings.AppSettings["productFileCatalogLocation"].ToString();
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            try
            { 
                var productDetails = Helper.ReadFromFile(productFileCatalogLocation);
                productDetails.ForEach(line =>
                {
                    products.Add(MapToProduct(line));
                });
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error in reading Products from Product DB", ex);
            }
            return products;
        }

        public Product GetProductById(int productId)
        {
            try
            { 
                var productDetails = Helper.ReadFromFile(productFileCatalogLocation);
                var product = productDetails.Find(line => productId == int.Parse(line.Substring(0, 3).Trim()));
                return MapToProduct(product);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error getting product by the specified Id", ex);
            }
        }

        private Product MapToProduct(string line)
        {
            return new Product
            {
                ProductId = int.Parse(line.Substring(0, 3).Trim()),
                Name = line.Substring(3, 20).Trim(),
                Price = double.Parse(line.Substring(23, 10).Trim()),
                DiscountType = line.Substring(33).Trim()
            };
        }
    }
}
