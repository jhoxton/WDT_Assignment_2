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
        // GET: /<controller>/

        public IActionResult Login()
        {
            return View();
        } 

        public IActionResult FranchiseIndex()
        {
            return View();
        }

        public IActionResult FranchiseInventory()
        {
            return View();
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
