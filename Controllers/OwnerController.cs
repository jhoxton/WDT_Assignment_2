using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDT_Assignment_2.Models;
using WDT_Assignment_2.Data;
using Microsoft.AspNetCore.Authorization;

namespace WDT_Assignment_2.Controllers
{
    [Authorize(Roles = Constants.OwnerRole)]
    public class OwnerController : Controller
    {
        private readonly Context _context;

        public OwnerController(Context context)
        {
            _context = context;
        }

        //This method taken from Lecture 6 code example

        // Auto-parsed variables coming in from the request - there is a form on the page to send this data.
        public async Task<IActionResult> OwnerInventory(string productName)
        {
            // Eager loading the Product table - join between OwnerInventory and the Product table.
            var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);

            if (!string.IsNullOrWhiteSpace(productName))
            {
                // Adding a where to the query to filter the data.
                // Note for the first request productName is null thus the where is not always added.
                query = query.Where(x => x.Product.Name.Contains(productName));

                // Storing the search into ViewBag to populate the textbox with the same value for convenience.
                ViewBag.ProductName = productName;
            }

            // Adding an order by to the query for the Product name.
            query = query.OrderBy(x => x.Product.Name);

            // Passing a List<OwnerInventory> model object to the View.
            return View(await query.ToListAsync());
        }

        // GET: StockRequests
        public async Task<IActionResult> OwnerProcessStockRequest()
        {
            //Creates a local var of StockRequests from _context
            var StockRequests = _context.StockRequests;

            foreach(var getStoreName in StockRequests) {  
                
                foreach(Store storeCheck in _context.Stores) {
                    if(getStoreName.StoreID == storeCheck.StoreID){
                        getStoreName.Store = storeCheck;
                    }
                }
            }
            foreach (var getStoreName in StockRequests)
            {  

                foreach (Product productCheck in _context.Products)
                {
                    if (getStoreName.ProductID == productCheck.ProductID)
                    {
                        getStoreName.Product = productCheck;
                    }
                }
            }

            //Sends said var to the View
            return View(await StockRequests.ToListAsync());
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestProcess = await _context.StockRequests.SingleOrDefaultAsync(m => m.StockRequestID == id);

            int quantCrossCheck = requestProcess.Quantity;

            //Checking valid owner stock level
            foreach(var ownerQuant in _context.OwnerInventory.ToList()) {
                if(quantCrossCheck > ownerQuant.StockLevel) {
                    
                   //PRINT SOMETHING HERE
                    //USE ToList() here for the Store Inventory I guess
                } else {



                    //Updates the store inventory
                    await updateStore(requestProcess.StoreID, requestProcess);
                    //Removes the stock request
                    _context.StockRequests.Remove(requestProcess);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(OwnerProcessStockRequest));

            //STILL NEED TO REMOVE STOCK FROM OWNER INVETORY

                }
            }

            return View();
 
        }
       
        public async Task<String> updateOwnerStock(int id,  int quantity)
        {
            foreach(var updateQuant in _context.OwnerInventory) {
                if(id == updateQuant.ProductID) {
                    updateQuant.StockLevel = (updateQuant.StockLevel - quantity);
                }
            }

            await _context.SaveChangesAsync();
            return " ";
        }

        public async Task <String> updateStore(int id, StockRequest requestProcess) {
            
            int storeID = requestProcess.StoreID;

            var storeInventory = await _context.StoreInventory.SingleAsync(x => x.StoreID == storeID && x.ProductID == id);

            storeInventory.StockLevel = requestProcess.Quantity + storeInventory.StockLevel;

            await _context.SaveChangesAsync();
            return " ";
        }
    
       

        public IActionResult OwnerIndex() 
        {
            ViewData["Message"] = " ";
            return View();
        }

        public async Task<IActionResult> OwnerSetStock(int? id)
        {  
            //Take id that is passed here and use it to display said item in the view. Hopefully
           
            //var movie = await _context.OwnerInventory.SingleOrDefaultAsync(m => m.ID == id);
            var product = await _context.OwnerInventory.SingleOrDefaultAsync(m => m.ProductID == id);

            return View(product);
        }

    }
}
