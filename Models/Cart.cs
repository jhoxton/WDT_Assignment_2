using System;
using System.Collections.Generic;

namespace WDT_Assignment_2.Models
{
    public class Cart
    {
       
        public ICollection<CartItem> ItemsInCart { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }
        
    }
}
