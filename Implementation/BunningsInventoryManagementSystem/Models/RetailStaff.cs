using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class RetailStaff : User
    {
        // Retail staff can update stock levels
        public void UpdateStockLevel(Item item, int amount)
        {
            item.UpdateStockLevel(amount);
        }

        // Retail staff can transfer stock to warehouse
        public void TransferStockToWarehouse(Item item, bool newLocation)
        {
            item.UpdateLocation(newLocation);
        }

        // Retail staff can view stock levels and item locations
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
