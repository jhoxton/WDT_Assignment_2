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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            //PopulateDepartmentsDropDownList(stockRequest.Store.StoreInventory);


            return View(stockRequest);
        }

        private void currentStoreProducts()
        {
            //THIS IS ONLY CREATING STOCK REQUESTS N PRODUCT 1
            var productQuery = from x in _context.StoreInventory.Where(x => x.StoreID == 1)
                             .Include(x => x.Product)
                             .Select(x => x)
                             orderby x.Product.ProductID
                                   select x;
            
            ViewBag.StoreInventory = new SelectList(productQuery, "ProductID", "Product.Name");
        }

        public IActionResult FranchiseSetStock()
        {
            return View();
        }
        public IActionResult FranchiseNewItem()
        {
            return View();
        }
    }
}
