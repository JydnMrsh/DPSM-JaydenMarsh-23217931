using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public double price { get; set; }
        public int stockLevel { get; set; }
        public int lowStockThreshold { get; set; }
        public bool location { get; set; }


        // Add amount of stock to the current stock level
        public void UpdateStockLevel(int amount)
        {
            stockLevel += amount;
        }

        // update items location, true for warehouse, false for retail
        public void UpdateLocation(bool newLocation)
        {
            location = newLocation;
        }

        // Check if the stock level is below the low stock threshold and trigger an alert
        public void checkLowStock()
        {
            if (stockLevel <= lowStockThreshold)
            {
                // Trigger low stock alert *****TODO*****
            }
        }

        // Add a new item to the inventory
        public void addNewItem(int itemID, string name, double price, int stockLevel, int lowStockThreshold, bool location)
        {
            ItemID = itemID;
            Name = name;
            this.price = price;
            this.stockLevel = stockLevel;
            this.lowStockThreshold = lowStockThreshold;
            this.location = location;
        }
    }
}
