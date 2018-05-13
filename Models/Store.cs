using System;
using System.Collections.Generic;

namespace WDT_Assignment_2.Models
{
    public class Store
    {
        public int StoreID { get; set; }
        public string Name { get; set; }

        public ICollection<StoreInventory> StoreInventory { get; set; } = new List<StoreInventory>();

        //public List<StoreInventory> StoreInventory { get; set; } = new List<StoreInventory>();
    }
}
