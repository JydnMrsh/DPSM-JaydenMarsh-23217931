using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class WarehouseStaff : User
    {
        // Warehouse staff can update stock levels
        public void UpdateStockLevel(Item item, int amount)
        {
            item.UpdateStockLevel(amount);
        }

        // Warehouse staff can transfer stock to retail
        public void TransferStockToRetail(Item item, bool newLocation)
        {
            item.UpdateLocation(newLocation);
        }

        public void GenerateOrderRequest(Item item, int amount)
        {
            // Generate order request *****TODO*****
        }

        public void ViewOrderStatus(int requestID)
        {
            // View order status *****TODO*****
        }

        // Warehouse staff can view stock levels and item locations
        public int ViewStockLevel(Item item)
        {
            return item.stockLevel;
        }
        public bool ViewItemLocation(Item item)
        {
            return item.location;
        }
    }
}
