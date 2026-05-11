using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class LowStockAlert
    {
        private InventorySystem inventorySystem = new InventorySystem();
        private static int nextAlertID = 1;
        public int AlertID { get; set; }
        public Item Item { get; set; }
        public string Message { get; set; }
        public int CurrentStockLevel { get; set; }
        public int StockThreshold { get; set; }
        public bool IsActive { get; set; }

        // Create a low stock alert for a specific item
        public LowStockAlert(Item item)
        {
            AlertID = nextAlertID++;
            Item = item;
            CurrentStockLevel = item.StockLevel;
            StockThreshold = item.LowStockThreshold;
            IsActive = true;
            Message = $"Low stock alert for item: {item.Name}. Stock threshold: {StockThreshold}. Current stock level: {CurrentStockLevel}.";
        }

        public void SendAlert()
        {
            // Log the alert for now, this could be extended to UI notifications. *****TODO*****
            inventorySystem.Logger?.Invoke(Message);
        }

        public void DismissAlert()
        {
            // Dismiss alert by taking it off the UI notifications *****TODO*****
            inventorySystem.Logger?.Invoke($"Alert for item: {Item.Name} dismissed.");
            IsActive = false;
        }
    }
}
