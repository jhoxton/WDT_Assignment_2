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
        public async Task<IActionResult> OwnerInventory(string productName)
        {
            var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);

            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(x => x.Product.Name.Contains(productName));
                ViewBag.ProductName = productName;
            }
            query = query.OrderBy(x => x.Product.Name);
            return View(await query.ToListAsync());
        }

        // GET: StockRequests
        public async Task<IActionResult> OwnerProcessStockRequest()
        {
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

                //Removes the stock request
                _context.StockRequests.Remove(requestProcess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OwnerProcessStockRequest));
            }

            return View();
        }

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
            foreach (Product productCheck in _context.Products)
            {
                if (product.ProductID == productCheck.ProductID)
                {
                    product.Product = productCheck;
                }
            }
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
                    //Returns error if owner doesn't have enough stock
                    ModelState.AddModelError("", "Unable to process stock request");
                }
                return RedirectToAction(nameof(OwnerIndex));
            }
            return View(productToUpdate);
        }


        public async Task<IActionResult> UpdateOwnerStock(int id, int quantity)
        {
            int levelToUpdate = quantity;
            int prodToUpdate = id;

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