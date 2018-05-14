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
    [Authorize(Roles = Constants.CustomerRole)]
    public class CustomerController : Controller
    {
        private readonly Context _context;

        public CustomerController(Context context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult CustomerIndex()
        {
            return View();
        }
        public IActionResult CustomerStoreSelect()
        {
            return View();
        }
        public IActionResult CustomerViewCart()
        {
            return View();
        }
        public IActionResult CustomerPurchaseHistory()
        {
            return View();
        }

        public async Task<IActionResult> CustomerDisplayInventory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passStore = await _context.Stores
                .Include(c => c.StoreInventory)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.StoreID == id);
            if (passStore == null)
            {
                return NotFound();
            }

            return View(passStore);
        }

        //public IActionResult CustomerDisplayInventory(int id) {

        //    int IDcheck = id;
        //    Store passStore = new Store();

        //    foreach(Store test in _context.Stores) {
        //        if(test.StoreID == IDcheck) {
        //            passStore = test;

        //        }
        //     //Something is getting passed to the page, just need to validate the data
        //    }

        //    //NEED TO FIND A WAY TO ADD AND VERIFY THE STORE
        //    //Instead of this, you will need to find the store inventory and pass it to another method
        //    // which returns the view. The view will then have to take the Store Inventory as an argument


        //    return View(passStore);
        //}



        public IActionResult CustomerCheckOut()
        {
            return View();
        }
        public IActionResult CustomerConfirmPurchase()
        {
            return View();
        }
       
        //public IActionResult populateStore(Store passingStore)
        //{
        //    return RedirectToAction(nameof(CustomerIndex));
        //}
    }
}
