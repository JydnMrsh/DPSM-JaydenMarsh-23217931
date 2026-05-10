using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }

    internal class OrderRequest
    {
        public int RequestID { get; set; }
        public Status Status { get; set; }
        public Item RequestedItem { get; set; }
        public int Amount { get; set; }


        public OrderRequest()
        {
            Status = Status.Pending; // Default status is pending
        }

        // Set the status of the order request to approved or rejected
        public void Approve()
        {
            Status = Status.Approved;
        }
        public void Reject()
        {
            Status = Status.Rejected;
        }
    }
}
