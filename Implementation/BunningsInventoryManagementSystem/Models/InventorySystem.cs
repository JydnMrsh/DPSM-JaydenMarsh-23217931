using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class InventorySystem
    {
        public List<Item> inventory = new List<Item>(); // List of items in the inventory
        public List<OrderRequest> orderRequests = new List<OrderRequest>(); // List of order requests

        public void Demo()
        {
            AddItem("Screws", 0.10, 100, 20, false); // Add an item to inventory

            OrderRequest request1 = new OrderRequest(GetItem(1), 10); // Example order request
            orderRequests.Add(request1); // Add order request to list
        }

        // Add a new item to the inventory
        public void AddItem(string name, double price, int stockLevel, int lowStockThreshold, bool location)
        {
            Item newItem = new Item(name, price, stockLevel, lowStockThreshold, location);
            inventory.Add(newItem);
        }

        // Get an item from the inventory by its ID
        public Item GetItem(int itemID)
        {
            foreach (Item item in inventory)
            {
                if (item.ItemID == itemID)
                {
                    return item;
                }
            }
            throw new Exception("Item not found.");
        }
    }
}
