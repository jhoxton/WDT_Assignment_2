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
                    public static Cart cart = new Cart();
                    public CartItem test = new CartItem();

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
            //Currently, this doubles each time so this will clearly need to be moved
            Product testProduct = new Product();
            testProduct.Name = "Rabbit";
            testProduct.ProductID = 1;
            testProduct.Price = 199.00m;
            test.Product = testProduct;
            
            test.StoreID = 1;

            cart.ItemsInCart.Add(test);   
            
            return View(cart);
        }
        public IActionResult CustomerPurchaseHistory()
        {
            return View();
        }

        //public async Task<IActionResult> CustomerDisplayInventory(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var passStore = await _context.Stores

        //        .Include(c => c.StoreInventory)


        //        .AsNoTracking()
        //        .SingleOrDefaultAsync(m => m.StoreID == id);

        //    if (passStore == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(passStore);
        //}


        //HOW DO I COMBINE THESE TWO METHODS????
        public async Task<IActionResult> CustomerDisplayInventory(string productName, int id)
        {
            var storeInvSelect = _context.StoreInventory.Include(x => x.Product).
                                         Where(x => x.StoreID == id).Select(x => x);
                               
            if (!string.IsNullOrWhiteSpace(productName))
            {
                storeInvSelect = storeInvSelect.Where(x => x.Product.Name.Contains(productName));
                ViewBag.ProductName = productName;
            }
            storeInvSelect = storeInvSelect.OrderBy(x => x.Product.Name);

            //foreach(var test in storeInvSelect) {
            //    if(test.StoreID == id) {
            //        storeInvSelect.
            //    }
            //}

            ViewBag.StoreName = (await _context.Stores.SingleAsync(x => x.StoreID == id)).Name;

            return View(await storeInvSelect.ToListAsync());

        }


        public IActionResult addToCart()
        {
            return View();
        }


        public IActionResult CustomerCheckOut()
        {
            return View();
        }
        public IActionResult CustomerConfirmPurchase()
        {
            return View();
        }

    }
}
