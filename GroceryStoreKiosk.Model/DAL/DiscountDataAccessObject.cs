using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.Model.DAL
{
    public class DiscountDataAccessObject
    {
        private string discountCatalogLocation;

        public List<Discount> GetAllDiscounts()
        {
            List<Discount> allDiscounts = new List<Discount>();
            allDiscounts.AddRange(GetRegularDiscounts());
            allDiscounts.AddRange(GetBOGODiscounts());
            allDiscounts.AddRange(GetGroupDiscounts());

            return allDiscounts;
        }

        private IEnumerable<Discount> GetGroupDiscounts()
        {
            List<Discount> discounts = new List<Discount>();
            discountCatalogLocation = ConfigurationSettings.AppSettings["groupDiscountCatalogLocation"].ToString();

            try
            { 
                 var discountDetails = Helper.ReadFromFile(discountCatalogLocation);
                discountDetails.ForEach(dis => discounts.Add(new GroupDiscount
                {
                    ProductId = int.Parse(dis.Substring(0,3).Trim()),
                    ItemCount = int.Parse(dis.Substring(3, 3).Trim()),
                    DicountedPrice = double.Parse(dis.Substring(6).Trim())
                }));
                return discounts;
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error in getting Group Discount Information", ex);
            }
        }

        private IEnumerable<Discount> GetBOGODiscounts()
        {
            List<Discount> discounts = new List<Discount>();
            discountCatalogLocation = ConfigurationSettings.AppSettings["bogoFileCatalogLocation"].ToString();

            try
            { 
                var discountDetails = Helper.ReadFromFile(discountCatalogLocation);
                discountDetails.ForEach(dis => discounts.Add(new Discount
                {
                    ProductId = int.Parse(dis.Substring(0, 3).Trim()),
                    DiscountPercentage = double.Parse(dis.Substring(3).Trim())
                }));
                return discounts;
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error in getting BOGO Offer Details", ex);
            }
        }

        private IEnumerable<Discount> GetRegularDiscounts()
        {
            List<Discount> discounts = new List<Discount>();
            discountCatalogLocation = ConfigurationSettings.AppSettings["regDiscountCatalogLocation"].ToString();
            
            try
            { 
                var discountDetails = Helper.ReadFromFile(discountCatalogLocation);
                discountDetails.ForEach(dis => discounts.Add(new Discount
                {
                    ProductId = int.Parse(dis.Substring(0, 3).Trim()),
                    DicountedPrice = double.Parse(dis.Substring(3).Trim())
                }));
                return discounts;
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error in getting Regular Product Discounts", ex);
            }
        }

        public Discount GetRegularDiscountByProduct(int productId)
        {
            discountCatalogLocation = ConfigurationSettings.AppSettings["regDiscountCatalogLocation"].ToString();
            string discountLine = GetDiscountDetails(discountCatalogLocation, productId);
            return new Discount
            {
                ProductId = productId,
                DicountedPrice = double.Parse(discountLine.Substring(3).Trim())
            };
        }

        public Discount GetBOGODiscountByProduct(int productId)
        {
            discountCatalogLocation = ConfigurationSettings.AppSettings["bogoFileCatalogLocation"].ToString();
            string discountLine = GetDiscountDetails(discountCatalogLocation, productId);
            return new Discount
            {
                ProductId = productId,
                DiscountPercentage = double.Parse(discountLine.Substring(3).Trim())
            };
        }

        public Discount GetGroupDiscountByProduct(int productId)
        {
            discountCatalogLocation = ConfigurationSettings.AppSettings["groupDiscountCatalogLocation"].ToString();
            string discountLine = GetDiscountDetails(discountCatalogLocation, productId);
            return new GroupDiscount
            {
                ProductId = productId,
                ItemCount = int.Parse(discountLine.Substring(3, 3).Trim()),
                DicountedPrice = double.Parse(discountLine.Substring(6).Trim())
                
            };
        }

        private string GetDiscountDetails(string discountCatalogLocation, int productId)
        {
            var discountDetails = Helper.ReadFromFile(discountCatalogLocation);
            return discountDetails.Find(line => productId == int.Parse(line.Substring(0, 3).Trim()));
        }
    }
}
