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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WDT_Assignment_2.Controllers
{   
    [Authorize(Roles = Constants.FranchiseRole)]
    public class FranchiseController : Controller
    {

        private readonly Context _context;

        public FranchiseController(Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        } 

        public IActionResult FranchiseIndex()
        {
            return View();
        }

        //Only calls Store with ID 1 (Melbourne CDB)
        public async Task<IActionResult> FranchiseInventory(string productName, int id)
        {
            var query = _context.StoreInventory.Include(x => x.Product).Select(x => x).Where(x => x.StoreID == 1).Select(x => x);
                
            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(x => x.Product.Name.Contains(productName));
                ViewBag.ProductName = productName;
            }
            query = query.OrderBy(x => x.Product.Name);

            return View(await query.ToListAsync());

        }

        //FranchiseStockRequest GET
        public IActionResult FranchiseStockRequest()
        {
            currentStoreProducts();
            return View();
        }

        //FranchiseStockRequest POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FranchiseStockRequest([Bind("ProductID,Quantity,StoreID")] StockRequest stockRequest)
        //Based on the ContosoUniversity example
        {
            stockRequest.StoreID = 1;
           
            if (ModelState.IsValid)
            {
                _context.Add(stockRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FranchiseIndex));
            }
            return View(stockRequest);
        }

        private void currentStoreProducts()
        {

            var productQuery = from x in _context.StoreInventory.Where(x => x.StoreID == 1)
                             .Include(x => x.Product)
                             .Select(x => x)
                             orderby x.Product.ProductID
                                   select x;
            
            ViewBag.StoreInventory = new SelectList(productQuery, "ProductID", "Product.Name");
        }
        ///
        //FranchiseNewItem GET
        public IActionResult FranchiseNewItem()
        {
            currentOwnerProducts();
            return View();
        }

        //FranchiseStockRequest POST
        //Error being thrown in here with deleting the new stock request, POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FranchiseNewItem([Bind("ProductID,Quantity,StoreID")] StockRequest stockRequest)
        //Based on the ContosoUniversity example
        {
            stockRequest.StoreID = 1;
            //Hardcoded Store 1 (CBD) as in line with Assignment specs
            if (ModelState.IsValid)
            {
                _context.Add(stockRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FranchiseIndex));
            }
            return View(stockRequest);
        }

        private List<Product> currentOwnerProducts()
        {
            var products = _context.Products.Where(x => !_context.StoreInventory.Any(y => y.ProductID == x.ProductID && y.StoreID == 1));
            //Hardcoded Store 1 (CBD) as in line with Assignment specs
            ViewBag.OwnerInventory = new SelectList(products, "ProductID", "Name");
            return products.ToList();

        }


    }
}
