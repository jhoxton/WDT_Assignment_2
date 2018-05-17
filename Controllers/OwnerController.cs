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
using Microsoft.AspNetCore.Http;

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

            foreach (var getStoreName in StockRequests)
            {

                foreach (Store storeCheck in _context.Stores)
                {
                    if (getStoreName.StoreID == storeCheck.StoreID)
                    {
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
            var ownerInventory = await _context.OwnerInventory.SingleOrDefaultAsync(m => m.ProductID == requestProcess.ProductID);
            var storeInventory = await _context.StoreInventory.SingleOrDefaultAsync(x => x.StoreID == requestProcess.StoreID && x.ProductID == requestProcess.ProductID);

            //Checking valid owner stock level

            if (requestProcess.Quantity > ownerInventory.StockLevel)
            {
                ModelState.AddModelError("", "Unable to process stock update");
            }
            else
            {
                //Updates the OwnerInvetory
                ownerInventory.StockLevel -= requestProcess.Quantity;

                if(storeInventory == null)
                {
                    _context.StoreInventory.Add(new StoreInventory
                    {
                        ProductID = requestProcess.ProductID,
                        StoreID = requestProcess.StoreID,
                        StockLevel = requestProcess.Quantity
                    });
                }
                else {
                    storeInventory.StockLevel += requestProcess.Quantity;
                }

                //Updates the store inventory
                // await UpdateStore(requestProcess.StoreID, requestProcess);

                //Removes the stock request
                _context.StockRequests.Remove(requestProcess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OwnerProcessStockRequest));
            }

            return View();
        }

        /*
        public async Task<String> UpdateStore(int id, StockRequest requestProcess)
        {

            int storeID = requestProcess.StoreID;

            var storeInventory = await _context.StoreInventory.SingleAsync(x => x.StoreID == storeID && x.ProductID == id);

            storeInventory.StockLevel = requestProcess.Quantity + storeInventory.StockLevel;

            await _context.SaveChangesAsync();
            return " ";
        }
        */

        public IActionResult OwnerIndex()
        {
            return View();
        }

        public async Task<IActionResult> OwnerSetStock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.OwnerInventory
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            //Finds and sets the name of the selected product
            /////
            foreach (Product productCheck in _context.Products)
            {
                if (product.ProductID == productCheck.ProductID)
                {
                    product.Product = productCheck;
                }
            }
            //////
            return View(product);
        }
        [HttpPost, ActionName("OwnerSetStock")]
        //This is the method the following will run after
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OwnerSetStockPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productToUpdate = await _context.OwnerInventory
                .SingleOrDefaultAsync(c => c.ProductID == id);

            if (await TryUpdateModelAsync<OwnerInventory>(productToUpdate, "", c => c.ProductID, c => c.StockLevel))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */ )
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes.");
                }
                return RedirectToAction(nameof(OwnerIndex));
            }
            //PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            return View(productToUpdate);
        }


        public async Task<IActionResult> UpdateOwnerStock(int id, int quantity)
        {
            //Updating OwnerInventory here...

            int levelToUpdate = quantity;
            int prodToUpdate = id;
            //int levelToUpdate = 100;
            //prodToUpdate = 1;

            //Checking valid owner stock level
            foreach (var ownerQuant in _context.OwnerInventory.ToList())
            {
                if (levelToUpdate > ownerQuant.StockLevel)
                {
                    //PRINT SOMETHING HERE
                }
                else
                {
                    //Updates the OwnerInvetory
                    foreach (var test in _context.OwnerInventory)
                    {
                        if (test.ProductID == prodToUpdate)
                        {
                            test.StockLevel = levelToUpdate;
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(OwnerInventory));

        }
    }
}