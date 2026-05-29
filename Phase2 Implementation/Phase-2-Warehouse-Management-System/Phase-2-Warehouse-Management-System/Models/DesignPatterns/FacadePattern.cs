using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phase_2_Warehouse_Management_System.Models; // Access Models

namespace Phase_2_Warehouse_Management_System.DesignPatterns
{
    /* DESIGN PATTERN: Facade
    
    Phase 1 weakness addressed:
    Item class is doing too much - handled stock updates,
    location updates, low stock checks, and adding new items.
    
    Fix:
    InventoryService is a Facade with a single simplified
    interface to three hidden subsystems:
    - InventoryStorage (data storage)
    - StockService (stock/transfer logic)
    - AlertService (low stock detection) */



    // Subsystem 1: STORAGE
    public class InventoryStorage
    {
        private Dictionary<int, Item> _items = new Dictionary<int, Item>(); // Stores all the items, better than a list because we don't need it ordered.
        private int _nextId = 1;

        // Add item to Dict.
        public void AddItem(Item item) { _items[item.ItemId] = item; }

        // Try find item with itemid
        public Item GetItem(int itemId)
        {
            Item item;
            return _items.TryGetValue(itemId, out item) ? item : null;
        }

        // Get all items
        public IEnumerable<Item> GetAll() { return _items.Values; }
        
        // Auto increment id's
        public int NextId() { return _nextId++; }
    }



    // Subsystem 2: STOCKSERVICE
    public class StockService
    {
        // Update warehouse stock and set item location to warehouse
        public void UpdateWarehouseStock(Item item, int change) // change: amount to change stock by
        {
            item.WarehouseStock += change;
            UpdateLocation(item);
            Console.WriteLine("  [Stock] '" + item.Name + "' warehouse stock -> " + item.WarehouseStock);
        }

        // Update retail stock and set item location to retail
        public void UpdateRetailStock(Item item, int change)
        {
            item.RetailStock += change;
            UpdateLocation(item);
            Console.WriteLine("  [Stock] '" + item.Name + "' retail stock -> " + item.RetailStock);
        }

        // Transfer stock to retail and set item location
        public bool TransferToRetail(Item item, int quantity)
        {
            // CHeck that there is enough stock to transfer
            if (item.WarehouseStock < quantity)
            {
                Console.WriteLine("  [Transfer] Failed: only " + item.WarehouseStock + " in warehouse.");
                return false;
            }
            item.WarehouseStock -= quantity;
            item.RetailStock += quantity;
            UpdateLocation(item);
            Console.WriteLine("  [Transfer] Moved " + quantity + "x '" + item.Name + "' -> Retail.");
            return true;
        }

        // Transfer stock to warehouse and set item location
        public bool TransferToWarehouse(Item item, int quantity)
        {
            // Check that there is enough stock to transfer
            if (item.RetailStock < quantity)
            {
                Console.WriteLine("  [Transfer] Failed: only " + item.RetailStock + " in retail.");
                return false;
            }
            item.RetailStock -= quantity;
            item.WarehouseStock += quantity;
            UpdateLocation(item);
            Console.WriteLine("  [Transfer] Moved " + quantity + "x '" + item.Name + "' -> Warehouse.");
            return true;
        }

        // If item exists in warehouse change location to warehouse, otherwise retail
        private void UpdateLocation(Item item)
        {
            // Check where items are located and update location
            if (item.WarehouseStock > 0 && item.RetailStock > 0)
                item.Location = ItemLocation.Both;
            else if (item.WarehouseStock > 0)
                item.Location = ItemLocation.Warehouse;
            else
                item.Location = ItemLocation.Retail;
        }
    }



    // Subsystem 3: ALERTSERVICE
    public class AlertService
    {
        // Check if low stock and alert if needed
        public void CheckAndAlert(Item item)
        {
            if (item.IsLowStock())
                Console.WriteLine("  [LOW STOCK ALERT] '" + item.Name + "' has only " + item.TotalStock + " units (threshold: " + item.LowStockThreshold + ")");
        }

        // Check all items for low stock, and alert if needed
        public void CheckAll(IEnumerable<Item> items)
        {
            foreach (var item in items) CheckAndAlert(item);
        }
    }



    // Facade: INVENTORYSERVICE, single entry point
    public class InventoryService
    {
        private InventoryStorage _storage = new InventoryStorage();
        private StockService _stockService = new StockService();
        private AlertService _alertService = new AlertService();
        private OrderNotifier _orderNotifier;
        private int _nextOrderId = 1;

        // Constructor
        public InventoryService(OrderNotifier orderNotifier)
        {
            _orderNotifier = orderNotifier;
        }

        public Item AddItem(string name, double price, int warehouseStock, int retailStock, int lowStockThreshold = 5) // Default threshold is 5
        {
            var item = new Item(_storage.NextId(), name, price, warehouseStock, retailStock, lowStockThreshold); // Create new item with input variables
            _storage.AddItem(item); // Add item to storage
            Console.WriteLine("  [Inventory] Added: " + item);
            return item;
        }

        // Get item using storage's GetItem
        public Item GetItem(int itemId) { return _storage.GetItem(itemId); }

        // Get all items using storage's get all
        public IEnumerable<Item> GetAllItems() { return _storage.GetAll(); }

        // View all stock in storage
        public void ViewAllStock(User requester)
        {
            // Only users allowed can view stock
            if (!(requester is IStockViewer) && !(requester is Customer))
            {
                Console.WriteLine("  [Access Denied] You cannot view stock levels.");
                return;
            }

            // Display all items in storage
            Console.WriteLine("\n  --- Inventory (viewed by " + requester.Username + ") ---"); 
            foreach (var item in _storage.GetAll())
                Console.WriteLine("    " + item);
        }

        // View item locations
        public void ViewItemLocations(User requester)
        {
            // Only allowed users can view item locations
            if (!(requester is ILocationViewer))
            {
                Console.WriteLine("  [Access Denied] You cannot view item locations.");
                return;
            }

            // Display all item locations
            Console.WriteLine("\n  --- Item Locations (viewed by " + requester.Username + ") ---");
            foreach (var item in _storage.GetAll())
                Console.WriteLine("    [" + item.ItemId + "] " + item.Name + ": " + item.Location);
        }

        // Update stock depending on location
        public void UpdateStock(IStockUpdater updater, User user, int itemId, int change, bool isWarehouse)
        {
            var item = _storage.GetItem(itemId);
            if (item == null) { Console.WriteLine("  Item not found."); return; }

            // Update warehouse stock otherwise retail
            if (isWarehouse) _stockService.UpdateWarehouseStock(item, change);
            else _stockService.UpdateRetailStock(item, change);
            _alertService.CheckAndAlert(item); // Check item stock level after updates
        }
        
        // Transfer stock to retail
        public void TransferToRetail(IStockUpdater updater, User user, int itemId, int quantity)
        {
            var item = _storage.GetItem(itemId);
            if (item == null) { Console.WriteLine("  Item not found."); return; }

            // Use stock service to transfer
            _stockService.TransferToRetail(item, quantity);
            _alertService.CheckAndAlert(item); // Check item stock level after updates
        }

        // Transfer stock to warehouse
        public void TransferToWarehouse(IStockUpdater updater, User user, int itemId, int quantity)
        {
            var item = _storage.GetItem(itemId);
            if (item == null) { Console.WriteLine("  Item not found."); return; }

            // Use stock service to transfer
            _stockService.TransferToWarehouse(item, quantity);
            _alertService.CheckAndAlert(item); // Check item stock level after updates
        }


        // Generate order request
        public OrderRequest GenerateOrderRequest(WarehouseStaff staff, int itemId, int quantity)
        {
            var item = _storage.GetItem(itemId);
            if (item == null) throw new Exception("Item not found.");

            // Create new order request with incremented order id
            var order = new OrderRequest(_nextOrderId++, itemId, item.Name, quantity, staff.Username);
            Console.WriteLine("\n  [Order] " + staff.Username + " generated order request: " + order);
            return order;
        }

        // Check all items for low stock
        public void RunStockAlertCheck()
        {
            Console.WriteLine("\n  --- Running Stock Alert Check ---");
            _alertService.CheckAll(_storage.GetAll());
        }
    }
}
