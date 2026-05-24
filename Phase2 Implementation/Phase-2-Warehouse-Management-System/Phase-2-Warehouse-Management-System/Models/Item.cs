using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phase_2_Warehouse_Management_System.Models; // Access models

namespace Phase_2_Warehouse_Management_System.Models
{
    public enum ItemLocation { Warehouse, Retail} // Different possible item locations

    public class Item
    {
        public int ItemId { get; private set; }
        public string Name { get; private set; }
        public double Price { get; set; }
        public int WarehouseStock { get; set; }
        public int RetailStock { get; set; }
        public int LowStockThreshold { get; set; } // Trigger low stock alert if stock of this item falls below
        public ItemLocation Location { get; set; }

        // Return total stock between both the warehouse and retail departments.
        public int TotalStock { get { return WarehouseStock + RetailStock; } }

        public Item(int itemId, string name, double price, int warehouseStock, int retailStock, int lowStockThreshold = 5) // Default threshold is 5
        {
            ItemId = itemId;
            Name = name;
            Price = price;
            WarehouseStock = warehouseStock;
            RetailStock = retailStock;
            LowStockThreshold = lowStockThreshold;

            // If warehouse doesn't have any stock of item then retail must have it.
            if (warehouseStock > 0)
                Location = ItemLocation.Warehouse;
            else
                Location = ItemLocation.Retail;
        }

        // Check if item's stock is less than the threshold
        public bool IsLowStock() { return TotalStock < LowStockThreshold; }
    }
}

