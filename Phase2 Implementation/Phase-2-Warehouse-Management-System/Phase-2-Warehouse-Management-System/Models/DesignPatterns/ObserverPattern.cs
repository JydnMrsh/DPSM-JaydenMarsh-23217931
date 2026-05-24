using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phase_2_Warehouse_Management_System.Models; // Access Models

namespace Phase_2_Warehouse_Management_System.DesignPatterns
{
    /* ObserverPattern.cs
    DESIGN PATTERN: Observer
    
    Phase 1 weakness addressed:
    OrderRequest has no way to tell who to send it to.
    Hard-coded to send to one store manager means no scalability.
    
    Fix:
    StoreManagers register as observers on the OrderNotifier.
    When an order is generated, All observers are
    notified automatically. Adding a second store manager
    requires zero changes to existing code, just Subscribe(). */

    public interface IOrderObserver
    {
        void OnOrderReceived(OrderRequest order);
    }

    // ORDER NOTIFIER
    public class OrderNotifier
    {
        private List<IOrderObserver> _observers = new List<IOrderObserver>(); // List of observers to be notified when an order is created

        // Add new observer
        public void Subscribe(IOrderObserver observer)
        {
            _observers.Add(observer);
            Console.WriteLine("  [Observer] " + observer + " subscribed to order notifications.");
        }

        // Remove observer
        public void Unsubscribe(IOrderObserver observer)
        {
            _observers.Remove(observer);
        }

        // Notify all the observers
        public void NotifyAll(OrderRequest order)
        {
            Console.WriteLine("\n  [Observer] Order #" + order.RequestId + " requested to " + _observers.Count + " managers(s)");
            
            // Send each observer alert
            foreach (var observer in _observers)
                observer.OnOrderReceived(order);
        }
    }


    // ORDER HANDLER
    public class ManagerOrderHandler : IOrderObserver
    {
        private StoreManager _manager;
        private List<OrderRequest> _pendingOrders = new List<OrderRequest>(); // Pending order list

        // Constructor
        public ManagerOrderHandler(StoreManager manager)
        {
            _manager = manager;
        }

        // Add order to list of pending orders
        public void OnOrderReceived(OrderRequest order)
        {
            _pendingOrders.Add(order);
            Console.WriteLine("  [Observer] " + _manager.Username + " received Order #" + order.RequestId + " for '" + order.ItemName + "' x" + order.Quantity);
        }

        // Approve
        public void ApproveOrder(int requestId)
        {
            OrderRequest order = null;

            // Check for matching order and update status
            foreach (var o in _pendingOrders)
                if (o.RequestId == requestId) { order = o; break; }
            if (order == null) { Console.WriteLine("  Order not found."); return; }
            order.UpdateStatus(OrderStatus.Approved);

            // Remove order from pending orders
            _pendingOrders.Remove(order);
        }

        // Reject
        public void RejectOrder(int requestId)
        {
            // Check for matching order and update status
            OrderRequest order = null;
            foreach (var o in _pendingOrders)
                if (o.RequestId == requestId) { order = o; break; }
            if (order == null) { Console.WriteLine("  Order not found."); return; }
            order.UpdateStatus(OrderStatus.Rejected);

            // Remove order from pending orders
            _pendingOrders.Remove(order);
        }


        // View pending orders
        public void ViewPendingOrders()
        {
            Console.WriteLine("\n  [" + _manager.Username + "] Pending Orders:");
            if (_pendingOrders.Count == 0) { Console.WriteLine("  None."); return; }

            // Display all pending orders
            foreach (var o in _pendingOrders) Console.WriteLine("    " + o);
        }
    }
}
