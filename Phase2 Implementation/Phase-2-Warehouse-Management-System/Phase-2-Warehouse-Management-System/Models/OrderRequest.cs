using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phase_2_Warehouse_Management_System.Models; // Access Models

namespace Phase_2_Warehouse_Management_System.Models
{
    public enum OrderStatus { Pending, Approved, Rejected } // Order status's

    public class OrderRequest
    {
        public int RequestId { get; private set; }
        public int ItemId { get; private set; }
        public string ItemName { get; private set; }
        public int Quantity { get; private set; }
        public OrderStatus Status { get; private set; }
        public string RequestedBy { get; private set; } // Which manager requested it now that we have implementation for multiple managers.

        // Order request constructor
        public OrderRequest(int requestId, int itemId, string itemName, int quantity, string requestedBy)
        {
            RequestId = requestId;
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            Status = OrderStatus.Pending;
            RequestedBy = requestedBy;
        }
    }
}
