using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Phase_2_Warehouse_Management_System.Models;
using Phase_2_Warehouse_Management_System.DesignPatterns;

namespace Phase_2_Warehouse_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // DEMO

            Banner("BUNNINGS WAREHOUSE - INVENTORY MANAGEMENT SYSTEM");
            Banner("Phase 2: Design Patterns Demo");

            // SETUP
            Section("SETUP: Users & System Initialisation");

            var warehouseWorker = new WarehouseStaff(1, "dave_w", "pass123");
            var retailWorker = new RetailStaff(2, "sarah_r", "pass456");
            var manager = new StoreManager(3, "mgr_jane", "mgr789");
            var manager2 = new StoreManager(4, "mgr_tom", "mgr000");
            var itStaff = new ITStaff(5, "it_admin", "admin999");
            var customer = new Customer(6, "customer", "");

            // PATTERN 1: Observer - both managers subscribe
            Console.WriteLine("\n[Observer Pattern] Registering managers as order observers:");
            var orderNotifier = new OrderNotifier();
            var janeHandler = new ManagerOrderHandler(manager);
            var tomHandler = new ManagerOrderHandler(manager2);
            orderNotifier.Subscribe(janeHandler);
            orderNotifier.Subscribe(tomHandler);

            // PATTERN 2: Facade - single entry point for inventory
            var inventory = new InventoryService(orderNotifier);

            Console.WriteLine("\n[Facade Pattern] Adding items via InventoryService:");
            inventory.AddItem("Hammer", 15.99, 20, 8);
            inventory.AddItem("Drill Bit Set", 45.50, 3, 2, 10);
            inventory.AddItem("Safety Gloves", 12.00, 50, 15);

            // DEMO 1: Strategy Pattern - Login & Permissions
            Section("PATTERN 3: Strategy Pattern - Role-Based Permissions");

            var warehouseAccess = new AccessController();
            Console.WriteLine("\n-- Warehouse Staff Login --");
            warehouseAccess.Login(warehouseWorker, "dave_w", "pass123");

            var retailAccess = new AccessController();
            Console.WriteLine("\n-- Retail Staff Login --");
            retailAccess.Login(retailWorker, "sarah_r", "pass456");

            var managerAccess = new AccessController();
            Console.WriteLine("\n-- Store Manager Login --");
            managerAccess.Login(manager, "mgr_jane", "mgr789");

            Console.WriteLine("\n-- Retail staff tries to generate order (should be denied) --");
            retailAccess.Check(retailAccess.Permissions.CanGenerateOrder, "GenerateOrderRequest");

            Console.WriteLine("\n-- Manager tries to update stock (should be denied) --");
            managerAccess.Check(managerAccess.Permissions.CanUpdateStock, "UpdateStockLevel");

            Console.WriteLine("\n-- Warehouse staff can generate order (allowed) --");
            warehouseAccess.Check(warehouseAccess.Permissions.CanGenerateOrder, "GenerateOrderRequest");

            // DEMO 2: Facade Pattern - Inventory Operations
            Section("PATTERN 2: Facade Pattern - InventoryService");

            Console.WriteLine("\n-- Warehouse staff views all stock --");
            inventory.ViewAllStock(warehouseWorker);

            Console.WriteLine("\n-- Retail staff views item locations --");
            inventory.ViewItemLocations(retailWorker);

            Console.WriteLine("\n-- Warehouse staff updates stock (received delivery: +10 Hammers) --");
            inventory.UpdateStock(warehouseWorker, warehouseWorker, 1, 10, true);

            Console.WriteLine("\n-- Retail staff transfers Safety Gloves back to warehouse --");
            inventory.TransferToWarehouse(retailWorker, retailWorker, 3, 5);

            Console.WriteLine("\n-- Warehouse transfers Drill Bit Sets to retail --");
            inventory.TransferToRetail(warehouseWorker, warehouseWorker, 2, 2);

            Console.WriteLine("\n-- System runs low stock alert check --");
            inventory.RunStockAlertCheck();

            // DEMO 3: Observer Pattern - Order Request Flow
            Section("PATTERN 1: Observer Pattern - Order Request Workflow");

            Console.WriteLine("\n-- Warehouse staff generates order for Drill Bit Sets --");
            var order1 = inventory.GenerateOrderRequest(warehouseWorker, 2, 50);

            Console.WriteLine("\n-- Warehouse staff generates order for Hammers --");
            var order2 = inventory.GenerateOrderRequest(warehouseWorker, 1, 30);

            Console.WriteLine("\n-- Jane (Manager 1) views pending orders and approves order 1 --");
            janeHandler.ViewPendingOrders();
            janeHandler.ApproveOrder(order1.RequestId);

            Console.WriteLine("\n-- Tom (Manager 2) views pending orders and rejects order 2 --");
            tomHandler.ViewPendingOrders();
            tomHandler.RejectOrder(order2.RequestId);

            // DEMO 4: Customer access (no login needed)
            Section("Customer Access - External Stock Visibility");

            Console.WriteLine("\n-- Customer checks item availability via website --");
            customer.CheckAvailability(inventory.GetItem(1));
            customer.CheckAvailability(inventory.GetItem(2));

            // DEMO 5: IT Staff
            Section("IT Staff - User Management");

            var itAccess = new AccessController();
            itAccess.Login(itStaff, "it_admin", "admin999");
            Console.WriteLine("\n  Can manage users: " + itAccess.Permissions.CanManageUsers);
            itStaff.CreateUser();
            itStaff.ManageUserPermission(retailWorker, UserRole.StoreManager);

            // FINAL: Updated inventory
            Section("FINAL: Updated Inventory State");
            inventory.ViewAllStock(manager);

            Banner("END OF DEMO");
        }

        static void Section(string title)
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("  " + title);
            Console.WriteLine("========================================");
        }

        static void Banner(string title)
        {
            Console.WriteLine("\n******************************");
            Console.WriteLine("  " + title);
            Console.WriteLine("******************************");
        }
    }
}
