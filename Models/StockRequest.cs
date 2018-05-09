using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WDT_Assignment_2.Models
{
    public class StockRequest
    {
        [Display(Name = "StockRequstID")]
      
        public int StockRequestID { get; set; }

        public int StoreID { get; set; }

        public Store Store { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
