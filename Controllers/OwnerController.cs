﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



namespace WDT_Assignment_2.Controllers
{
    public class OwnerController : Controller
    {
        /* GET: /<controller>/
        /The below is the method used to return the view, so do one for each view page
        *In owner, that is:
         * OwnerIndex
         * Inventory
         * SetStock
         * StockRequest
         */

        public IActionResult OwnerIndex() 
        {
            ViewData["Message"] = "Hello from my first view and controller!";
            return View();
        }
    }
}