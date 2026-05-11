using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class Item
    {
        private static int nextItemID = 1;
        public int ItemID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockLevel { get; set; }
        public int LowStockThreshold { get; set; }
        public bool Location { get; set; }

        // Contructor
        public Item(string name, double price, int stockLevel, int lowStockThreshold, bool location)
        {
            ItemID = nextItemID++;
            Name = name;
            Price = price;
            StockLevel = stockLevel;
            LowStockThreshold = lowStockThreshold;
            Location = location;
        }

        // Add amount of stock to the current stock level
        public void UpdateStockLevel(int amount)
        {
            StockLevel += amount;
            CheckLowStock(); // Check if the stock level is below the low stock threshold after updating
        }

        // update items location, true for warehouse, false for retail
        public void UpdateLocation(bool newLocation)
        {
            Location = newLocation;
        }

        // Check if the stock level is below the low stock threshold and trigger an alert
        public void CheckLowStock()
        {
            if (StockLevel <= LowStockThreshold)
            {
                new LowStockAlert(this).SendAlert();
            }
        }
    }
}
