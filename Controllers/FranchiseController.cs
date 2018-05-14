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
            var query = _context.StoreInventory.Include(x => x.Product).Select(x => x);
            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(x => x.Product.Name.Contains(productName));
                ViewBag.ProductName = productName;
            }
            query = query.OrderBy(x => x.Product.Name);

            return View(await query.ToListAsync());

        }
        public IActionResult FranchiseStockRequest()
        {
            return View();
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
