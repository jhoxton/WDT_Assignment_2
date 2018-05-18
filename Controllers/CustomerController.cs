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

        //static List<Product> intList = new List<Product>();

        private readonly Context _context;

        public CustomerController(Context context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult CustomerIndex()
        {
            //Product testProduct = new Product();
            //testProduct.Name = "Rabbit";
            //testProduct.ProductID = 1;
            //testProduct.Price = 199.00m;
            //test.Product = testProduct;

            //test.StoreID = 1;

            //cart.ItemsInCart.Add(test);
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
                return View();
            }
            //Product testProduct = new Product();
            //testProduct.Name = "Rabbit";
            //testProduct.ProductID = 1;
            //testProduct.Price = 199.00m;
            //test.Product = testProduct;

            //test.StoreID = 1;

            //cart.ItemsInCart.Add(test);

           

        }



        public async Task<IActionResult> RemoveFromCart(int productID, int storeID)
        {
            //var cart = HttpContext.Session.GetCart();


            //Check cart quantity is only 1!!!!!
            var storeInventory = await _context.StoreInventory.Include(x => x.Product).Include(x => x.Store).
                SingleAsync(x => x.ProductID == productID && x.StoreID == storeID);

            storeInventory.Store.StoreInventory.Clear();
            cart.RemoveItem(storeInventory);

            //HttpContext.Session.SetCart(cart);
            return RedirectToAction(nameof(CustomerViewCart));
            //return RedirectToAction("Index");
        }

        public IActionResult CustomerPurchaseHistory()
        {
            return View();
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
            //var cart = HttpContext.Session.GetCart();

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
            return View();
        }
        public IActionResult CustomerConfirmPurchase()
        {
            return View();
        }



    }
}


