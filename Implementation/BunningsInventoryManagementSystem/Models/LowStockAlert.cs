using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class LowStockAlert
    {
        public int AlertID { get; set; }
        public Item Item { get; set; }
        public string Message { get; set; }
        public int CurrentStockLevel { get; set; }
        public int StockThreshold { get; set; }
        public bool IsActive { get; set; }

        // Create a low stock alert for a specific item
        public LowStockAlert(int alertID, Item item)
        {
            AlertID = alertID;
            Item = item;
            CurrentStockLevel = item.stockLevel;
            StockThreshold = item.lowStockThreshold;
            IsActive = true;
            Message = $"Low stock alert for item: {item.Name}. Stock threshold: {StockThreshold}. Current stock level: {CurrentStockLevel}.";
        }

        public void SendAlert()
        {
            // Send alert *****TODO*****
            Console.WriteLine(Message);
        }

        public void DismissAlert()
        {
            // Dismiss alert *****TODO*****
            Console.WriteLine($"Alert for item: {Item.Name} dismissed.");
            IsActive = false;
        }
    }
}
