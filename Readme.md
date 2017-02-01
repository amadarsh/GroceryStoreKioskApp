# Grocery Store Kiosk Checkout Application

This is a program (.net) which runs on a store's self checkout kiosk, and applies proper billing / discounts and provides an itemized summary of the receipt.

## Overview

This is a program which implements a Grocery or any store's automated checkout simulation. This program will read input from a file and print the summary on the console. The program uses a File Watcher to monitor the input folder. As soon the input file copied to this folder the program starts the checkout process.

The solution uses a domain driven approach with Abstract fatory design pattern for Price calculations. With this approach we are future-proofing the code.

## Pre-requisites

Note: Below mentioned folder locations are configurable (in app.config)
1. Make sure the below folders are present
```
Input file folder - ..\GroceryStoreKioskApp\GroceryStoreKiosk\ScannedItems
Product and discount catalog location - ..\GroceryStoreKioskApp\GroceryStoreKiosk\CatalogFiles 
Archive folder - ..\GroceryStoreKioskApp\GroceryStoreKiosk\ArchivedScans
```
2. Make sure the below files are present under the Product and discount catalog folder:

```
ProductCatalog.txt 
RegularDiscountCatalog.txt 
BOGODiscountCatalog.txt 
GroupDiscountCatalog.txt
```
## Program details

Once the program starts up, the Kiosk will start monitoring the scan folder where the customer must place the item list. The program will read through the input list, check the product details in the data store, checks for any discount or offers available from the discount data store and then applies the appropriate tariffs. The output of this program is the receipt viz., the itemized summary of the input list with the actual price and the discounted price.

For any reason, if the any item in input list does not have a product info then the same will not a price calculated.
If an item is eligible or has no discount available then the discounted price in the summary will be shown as '-'.

The summary will also display the total base bill and the total savings after applying the discounts.

When a subsequent input file is processed, the console clears the display and prints the new itemized summary.

The program archives the input files into a different folder - Archive folder.

## Input file Details

1. The input file must have the items specified line by line
```
banana
orange
apple
```
1. An item can be specified any number of times
```
banana
banana
orange
apple
apple
```
1. Items can be in any order
```
banana
orange
banana
apple
apple
orange
```

## Data store file Details

### Product details

File name: ProductCatalog.txt
Columns: Product Id, Product name, Price, Discount Type 
Data format: 
```
1  Banana              0.5       GROUP          
2  Apple               0.4       BOGO      
3  Orange              0.6       BOGO      
4  Mango               0.8       BOGO       
```

### Discount details

Currently there are 3 types of discounts - Regular Discount, Buy one get one (BOGO) discount, Group discount (ex: buy 3 for $10)

File name: RegularDiscountCatalog.txt
Columns: Product Id, Discounted Price 
Data format: 
```
6  0.5                       
7  3.24                       
```

File name: BOGODiscountCatalog.txt
Columns: Product Id, Percentage discount
Data format: 
```
2  100                 
3  60             
4  20                                       
```

File name: GroupDiscountCatalog.txt
Columns: Product Id, Item count eligible for discount, Discounted Price
Data format: 
```
1  3  1                                   
10 5  20 
```

IMPORTANT NOTE:
```
A product can have only one type of discount. The application will throw an error if the same product has more than 1 discount type.

Please make sure, each product is present in one type of catalog only.

Ex: In this case, Product 'Banana' with a product id as '1' either be in 'BOGODiscountCatalog.txt', 'GroupDiscountCatalog.txt' or 'RegularDiscountCatalog.txt' only.
```


## Built With

* Visual Studio 2015
* .Net framework 4.6.1

 
