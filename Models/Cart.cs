using System;
using System.Collections.Generic;

namespace WDT_Assignment_2.Models
{
    public class Cart
    {
        //public const string CartSessionKey = "cart";

        public List<StoreInventory> Items { get; set; } = new List<StoreInventory>();

        public void AddItem(StoreInventory item)
        {
            var index = Items.FindIndex(x => x.ProductID == item.ProductID && x.StoreID == item.StoreID);
            if (index == -1)
                Items.Add(item);
            else
                Items[index] = item;
        }

        public void RemoveItem(StoreInventory item)
        {
            
            var index = Items.FindIndex(x => x.ProductID == item.ProductID && x.StoreID == item.StoreID);
            if (index == -1)
                Items.Remove(item);
            else
                Items[index] = item;
        }
       
        //public ICollection<CartItem> ItemsInCart { get; set; } = new List<CartItem>();
        //public decimal TotalPrice { get; set; }


        
    }
}
