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
        //Static Cart object. Secure and stable but NOT scaleable. Can only support one customer. 
        static List<Product> PurchaseHistory = new List<Product>();

        private readonly Context _context;

        public CustomerController(Context context)
        {
            _context = context;
        }
        public IActionResult CustomerIndex()
        {
            return View();
        }
        public IActionResult CustomerStoreSelect()
        {
            return View();
        }


        // GET: Cart. Won't return cart page if Cart object is empty
        public IActionResult CustomerViewCart()
        {
            if (cart != null)
            {
                return View(cart);
            }
            else
            {
                //Do a script here to say "cart is empty"
                return RedirectToAction(nameof(CustomerIndex));
            }
        }

        public async Task<IActionResult> RemoveFromCart(int productID, int storeID)
        {
            var storeInventory = await _context.StoreInventory.Include(x => x.Product).Include(x => x.Store).
            SingleAsync(x => x.ProductID == productID && x.StoreID == storeID);

            storeInventory.Store.StoreInventory.Clear();
            cart.RemoveItem(storeInventory);

            return RedirectToAction(nameof(CustomerViewCart));

        }

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
            ViewBag.StoreName = (await _context.Stores.SingleAsync(x => x.StoreID == id)).Name;

            return View(await storeInvSelect.ToListAsync());
        }


        public async Task<IActionResult> AddToCart(int productID, int storeID)
        {
            var storeInventory = await _context.StoreInventory.Include(x => x.Product).Include(x => x.Store).
                SingleAsync(x => x.ProductID == productID && x.StoreID == storeID);

            storeInventory.StockLevel = 1;
            storeInventory.Store.StoreInventory.Clear();
            cart.AddItem(storeInventory);

            return RedirectToAction(nameof(CustomerViewCart));

        }

        public IActionResult CustomerCheckOut()
        {
            //Gets subtracted from relevent stores then turned into AngularJS

            return View();
        }
        public IActionResult CustomerConfirmPurchase()
        {
            {

                if (cart != null)
                {
                    return View(cart);
                }
                else
                {
                   
                    return RedirectToAction(nameof(CustomerIndex));
                }


            }
        }
        public IActionResult CustomerPurchaseHistory()
        {
            return View();
        }


        public IActionResult AddToHistory(Cart cart)
        {
            foreach(var test in cart.Items) {
                
                PurchaseHistory.Add(test.Product);
            }

            return RedirectToAction(nameof(CustomerIndex));
        }
    }
}


