using Phase_2_Warehouse_Management_System.DesignPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase_2_Warehouse_Management_System
{
    public class AppState
    {
        // ---- Singleton ----
        private static AppState _instance;
        public static AppState Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppState();
                return _instance;
            }
        }

        // ---- Users ----
        public WarehouseStaff WarehouseWorker { get; } = new WarehouseStaff(1, "dave_w", "pass123");
        public RetailStaff RetailWorker { get; } = new RetailStaff(2, "sarah_r", "pass456");
        public StoreManager Manager1 { get; } = new StoreManager(3, "mgr_jane", "mgr789");
        public StoreManager Manager2 { get; } = new StoreManager(4, "mgr_tom", "mgr000");
        public ITStaff ItAdmin { get; } = new ITStaff(5, "it_admin", "admin999");

        // ---- Observer infrastructure ----
        public OrderNotifier OrderNotifier { get; }
        public ManagerOrderHandler JaneHandler { get; }
        public ManagerOrderHandler TomHandler { get; }

        // ---- Facade ----
        public InventoryService Inventory { get; }

        // ---- Currently logged-in session ----
        public User? CurrentUser { get; set; }
        public AccessController? Access { get; set; }
        public ManagerOrderHandler? CurrentHandler { get; set; }

        private AppState()
        {
            OrderNotifier = new OrderNotifier();
            JaneHandler = new ManagerOrderHandler(Manager1);
            TomHandler = new ManagerOrderHandler(Manager2);
            OrderNotifier.Subscribe(JaneHandler);
            OrderNotifier.Subscribe(TomHandler);
            Inventory = new InventoryService(OrderNotifier);

            // Seed inventory
            Inventory.AddItem("Hammer", 15.99, 20, 8);
            Inventory.AddItem("Drill Bit Set", 45.50, 3, 2, 10);
            Inventory.AddItem("Safety Gloves", 12.00, 50, 15);
            Inventory.AddItem("Paint Brush", 8.50, 30, 10);
            Inventory.AddItem("Tape Measure", 19.99, 12, 4);
        }

        // Returns all registered users for the login screen to validate against
        public User? FindUser(string username, string password)
        {
            User[] all = { WarehouseWorker, RetailWorker, Manager1, Manager2, ItAdmin };
            foreach (var u in all)
                if (u.LogIn(username, password)) return u;
            return null;
        }

        // Returns the ManagerOrderHandler for a given StoreManager
        public ManagerOrderHandler? GetHandler(StoreManager manager)
        {
            if (manager == Manager1) return JaneHandler;
            if (manager == Manager2) return TomHandler;
            return null;
        }
    }
}
