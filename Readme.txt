This is a .net application which runs on a store's self checkout kiosk, and applies proper billing / discounts and provides an itemized summary of the receipt.

The solution uses a domain driven approach with Abstract fatory design pattern for Price calculations. With this approach we are future-proofing the code.

Application Details:
Before running the application, please make sure the required folders are present.
Input file folder : 
..\GroceryStoreKioskApp\GroceryStoreKiosk\ScannedItems
Product and discount catalog location : 
..\GroceryStoreKioskApp\GroceryStoreKiosk\CatalogFiles
Archive folder: 
..\GroceryStoreKioskApp\GroceryStoreKiosk\ArchivedScans

You can use different folder locations, but make sure you have the Product and  discount catalog files.

Once the application starts up, the Kiosk will start monitoring the scan folder where the customer must place the item list (.txt file). The scan folder is configurable. 
Example of an input file:
Banana
Apple
Banana
Orange

Once the customer places the input list, the Kiosk will scan through the items, apply proper discounts and generates the receipt of the item price summary. The discounts and prices are configured by the marketing team. 
Product and Discount catalogs are placed as txt files:
ProductCatalog.txt
RegularDiscountCatalog.txt
BOGODiscountCatalog.txt
GroupDiscountCatalog.txt

For any product or discount changes, the marketing has to make relevant changes to the above files (data stores).

Important: A product can have only one type of discount. The application will throw an error if the same product has more than 1 discount type.
Please make sure, each product is present in one type of catalog only.
Ex: In this case, Product 'Banana' with a product id as '1' either be in 'BOGODiscountCatalog.txt', 'GroupDiscountCatalog.txt' or 'RegularDiscountCatalog.txt' only.

Note: If the scanned item is not present in the current list of product catalog then the price of that item will not be calculated.

Catalog details: (Data store details)

Product Catalog:
Columns: Product Id, Product name, Price, Discount Type
Data format:
1  Banana              0.5       GROUP          
2  Apple               0.4       BOGO      
3  Orange              0.6       BOGO      
4  Mango               0.8       BOGO       

Discount Catalogs:

Regular Discount catalog
Columns: Product Id, Discounted Price
Data format:
6  0.5                       
7  0.34

BOGO (Buy One get one) Discount catalog
Columns: Product Id, Percentage discount
Data format:
2  100                 
3  60             
4  20                

Group Discount catalog
Columns: Product Id, Item count eligible for discount, Discounted Price
Data format:
1  3  1                                   
10 5  5