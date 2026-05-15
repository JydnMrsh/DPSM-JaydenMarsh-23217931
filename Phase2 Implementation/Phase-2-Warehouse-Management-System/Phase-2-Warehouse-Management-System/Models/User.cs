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
    }

    // WAREHOUSE CLASS
    public class WarehouseStaff : User, IStockUpdater, IStockViewer, ILocationViewer
    {
        public WarehouseStaff(int id, string username, string password)
            : base(id, username, password, UserRole.WarehouseStaff) { }
    }
}
