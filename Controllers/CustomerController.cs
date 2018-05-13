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
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            var theStore = _context.StoreInventory
             .Include(c => c.Store)
            .AsNoTracking();
            return View(await theStore.ToListAsync());
        }

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
