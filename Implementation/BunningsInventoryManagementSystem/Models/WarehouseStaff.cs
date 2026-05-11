using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class WarehouseStaff : User
    {
        public WarehouseStaff(string username, string password, Role role) : base(username, password, role)
        {
        }


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

        // Warehouse staff can generate order requests
        public void GenerateOrderRequest(Item item, int amount)
        {
            OrderRequest newRequest = new OrderRequest(item, amount);
        }

        // Warehouse staff can view order request status
        public Status ViewOrderStatus(InventorySystem system, int requestID)
        {
            foreach (OrderRequest request in system.orderRequests) // Find the order request with the given ID
            {
                if (request.RequestID == requestID)
                {
                    return request.Status;
                }
            }

            throw new Exception("Order request not found.");
        }

        // Warehouse staff can view stock levels and item locations
        public int ViewStockLevel(Item item)
        {
            return item.StockLevel;
        }
        public bool ViewItemLocation(Item item)
        {
            return item.InWarehouse;
        }
    }
}
