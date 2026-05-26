using Phase_2_Warehouse_Management_System.Models; // Access models
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase_2_Warehouse_Management_System
{
    public enum UserRole { WarehouseStaff, RetailStaff, StoreManager, ITStaff, Customer } // User roles

    // USER CLASS
    public abstract class User
    {
        public int UserId { get; protected set; }
        public string Username { get; protected set; }
        protected string Password { get; set; }
        public UserRole Role { get; protected set; }
        
        // User constructor
        protected User(int userId, string username, string password, UserRole role)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Role = role;
        }

        // Login with username and password
        public bool LogIn(string username, string password)
        {
            return Username == username && Password == password;
        }

        // To string override
        public override string ToString() { return "[" + Role + "] " + Username; }
    }

    // WAREHOUSESTAFF CLASS
    public class WarehouseStaff : User, IStockUpdater, IStockViewer, ILocationViewer
    {
        // Same as base user class
        public WarehouseStaff(int id, string username, string password) : base(id, username, password, UserRole.WarehouseStaff) { }
    }

    // RETAILSTAFF CLASS
    public class RetailStaff : User, IStockUpdater, IStockViewer, ILocationViewer
    {
        // Same as base user class
        public RetailStaff(int id, string username, string password) : base(id, username, password, UserRole.RetailStaff) { }
    }

    public class StoreManager : User, IStockViewer, ILocationViewer, IOrderApprover
    {
        // Same as base user class
        public StoreManager(int id, string username, string password) : base(id, username, password, UserRole.StoreManager) { }
    }

    // ITSTAFF CLASS
    public class ITStaff : User
    {
        public ITStaff(int id, string username, string password) : base(id, username, password, UserRole.ITStaff) { }

        // Create new user account
        public void CreateUser()
        {
            Console.WriteLine("  [IT] Creating new user account.");
        }

        // Change role of existing user account
        public void ManageUserPermission(User user, UserRole newRole)
        {
            Console.WriteLine("  [IT] Updating " + user.Username + "'s role to " + newRole + ".");
        }
    }

    // CUSTOMER
    public class Customer : User
    {
        public Customer(int id, string username, string password) : base(id, username, password, UserRole.Customer) { }

        // Simulate checking availability from Bunnings Warehouse website
        public void CheckAvailability(Item item)
        {
            Console.WriteLine("  [Website] '" + item.Name + "' - Total stock: " + item.TotalStock);
        }
    }
}
