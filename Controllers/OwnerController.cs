using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDT_Assignment_2.Models;

namespace WDT_Assignment_2.Controllers
{
    public class OwnerController : Controller
    {
        private readonly Context _context;

        public OwnerController(Context context)
        {
            _context = context;
        }

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

            foreach(var getStoreName in StockRequests) {  //THIS WILL NEED TO ACCESS THE STORE CONTROL ASPECT
                int loopID = getStoreName.StoreID;

                if (loopID ==1 ) {
                   
                    Console.WriteLine("FFFUUUCKKK");
                }

            }

            //Sends said var to the View
            return View(await StockRequests.ToListAsync());
        }


        /* GET: /<controller>/
       /The below is the method used to return the view, so do one for each view page
        */

        public IActionResult OwnerIndex() 
        {
            ViewData["Message"] = "Hello from my first view and controller!";
            return View();
        }

        //public IActionResult OwnerInventory()
        //{
        //    return View();
        //}
        //public IActionResult OwnerProcessStockRequest()
        //{
        //    return View();
        //}
        public IActionResult OwnerSetStock()
        {
            return View();
        }
    }
}
