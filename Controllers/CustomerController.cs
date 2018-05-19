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
        //public CartItem test = new CartItem();

        static List<Product> PurchaseHistory = new List<Product>();

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


        // GET: Cart
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

            //Check cart quantity is only 1!!!!!
            var storeInventory = await _context.StoreInventory.Include(x => x.Product).Include(x => x.Store).
                SingleAsync(x => x.ProductID == productID && x.StoreID == storeID);

            storeInventory.Store.StoreInventory.Clear();
            cart.RemoveItem(storeInventory);

            //HttpContext.Session.SetCart(cart);
            return RedirectToAction(nameof(CustomerViewCart));
            //return RedirectToAction("Index");
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

            //HttpContext.Session.SetCart(cart);
            return RedirectToAction(nameof(CustomerViewCart));
            //return RedirectToAction("Index");
        }

        public IActionResult CustomerCheckOut()
        {

            //REMOVE ALL ITESM FROM CART HERE
            //FIND A WAY TO GENERATE ORDER ID


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
                    //Do a script here to say "cart is empty"
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


