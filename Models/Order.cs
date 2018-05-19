using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WDT_Assignment_2.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        public List<Product> Items { get; set; } = new List<Product>();
    }
}
