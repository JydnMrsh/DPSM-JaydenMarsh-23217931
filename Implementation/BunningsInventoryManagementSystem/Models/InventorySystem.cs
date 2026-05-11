using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class InventorySystem
    {
        public List<Item> inventory = new List<Item>(); // List of items in the inventory
        public List<OrderRequest> orderRequests = new List<OrderRequest>(); // List of order requests
        public Action<string> Logger; // Logger action to log messages

        public void Demo()
        {
            ITStaff itStaff1 = new ITStaff("admin", "password123", Role.ITStaff); // Create an IT staff user

            itStaff1.CreateUser("warehouse1", "warehousePassword1", Role.WarehouseStaff); // ITStaff can create a warehouse staff user
            itStaff1.CreateUser("retail1", "retailPassword1", Role.RetailStaff); // ITStaff can create a retail staff user
            itStaff1.CreateUser("manager1", "managerPassword1", Role.StoreManager); // ITStaff can create a store manager user

            AddItem("Chocolate", 5, 100, 20); // Add an item to inventory
            AddItem("Cereal", 10, 50, 10);
            AddItem("Milk", 3, 30, 5);
            AddOrderRequest(GetItem(1), 50); // Add an order request for the item
        }

        // Add a new item to the inventory
        public void AddItem(string name, double price, int stockLevel, int lowStockThreshold)
        {
            Item newItem = new Item(name, price, stockLevel, lowStockThreshold);
            inventory.Add(newItem);
            Logger?.Invoke($"Added item: {name} with ID: {newItem.ItemID}");
        }

        // Add a new order request for an item
        public void AddOrderRequest(Item item, int amount)
        {
            OrderRequest newRequest = new OrderRequest(item, amount);
            orderRequests.Add(newRequest);
            Logger?.Invoke($"Added order request for item: {item.Name}, amount: {amount}, request ID: {newRequest.RequestID}");
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
