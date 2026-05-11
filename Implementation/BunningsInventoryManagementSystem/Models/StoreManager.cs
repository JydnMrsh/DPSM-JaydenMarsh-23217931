using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class StoreManager : User
    {
        // Store manager can approve or reject order requests
        public void AprroveOrderRequest(int requestID)
        {
            // Approve order request *****TODO*****
        }

        public void RejectOrderRequest(int requestID)
        {
            // Reject order request *****TODO*****
        }


        // Store manager can view stock levels and item locations
        public int ViewStockLevel(Item item)
        {
            return item.StockLevel;
        }
        public bool ViewItemLocation(Item item)
        {
            return item.Location;
        }
    }
}
