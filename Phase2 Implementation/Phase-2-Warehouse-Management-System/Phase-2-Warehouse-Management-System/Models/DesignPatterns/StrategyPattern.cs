using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phase_2_Warehouse_Management_System.Models; // Access Models

namespace Phase_2_Warehouse_Management_System.DesignPatterns
{
    /* 
    DESIGN PATTERN: Strategy
    
    Phase 1 weakness:
    No interface for permissions, so the permission logic was
    scattered all over the code, e.g. each time you press a
    button it will check what role the user has.
    
    Fix:
    Each role has a concrete PermissionStrategy.
    The AccessController selects the correct strategy at login.
    New roles only require a new strategy class. */



    // Permission strategy
    public interface IPermissionStrategy
    {
        // Permissions:
        bool CanViewStock { get; }
        bool CanUpdateStock { get; }
        bool CanViewLocation { get; }
        bool CanGenerateOrder { get; }
        bool CanApproveOrder { get; }
        bool CanManageUsers { get; }

        // Show permissions for user
        void PrintPermissions(string username);
    }

    // WAREHOUSE PERMISSIONS
    public class WarehousePermissions : IPermissionStrategy
    {
        public bool CanViewStock { get { return true; } }
        public bool CanUpdateStock { get { return true; } }
        public bool CanViewLocation { get { return true; } }
        public bool CanGenerateOrder { get { return true; } }
        public bool CanApproveOrder { get { return false; } } // Can't approve orders
        public bool CanManageUsers { get { return false; } } // Can't manage users
        
        // Show permissions for warehouse user
        public void PrintPermissions(string username)
        {
            Console.WriteLine("  [" + username + "] Warehouse: View:Y Update:Y Location:Y Order:Y Approve:N ManageUsers:N");
        }
    }

    // RETAIL PERMISSIONS
    public class RetailPermissions : IPermissionStrategy
    {
        public bool CanViewStock { get { return true; } }
        public bool CanUpdateStock { get { return true; } }
        public bool CanViewLocation { get { return true; } }
        public bool CanGenerateOrder { get { return false; } } // Can't generate order requests
        public bool CanApproveOrder { get { return false; } } // Can't Approve orders
        public bool CanManageUsers { get { return false; } } // Cant manage users
        
        // Show the permissions for retail user
        public void PrintPermissions(string username)
        {
            Console.WriteLine("  [" + username + "] Retail: View:Y Update:Y Location:Y Order:N Approve:N ManageUsers:N");
        }
    }


    // MANAGER PERMISSIONS
    public class ManagerPermissions : IPermissionStrategy
    {
        public bool CanViewStock { get { return true; } }
        public bool CanUpdateStock { get { return false; } } // Can't update stock
        public bool CanViewLocation { get { return true; } }
        public bool CanGenerateOrder { get { return false; } } // Can't generate order requests
        public bool CanApproveOrder { get { return true; } }
        public bool CanManageUsers { get { return false; } } // Can't manage user permissions (Only IT)
        
        // Show the permissions for store managers
        public void PrintPermissions(string username)
        {
            Console.WriteLine("  [" + username + "] Manager: View:Y Update:N Location:Y Order:N Approve:Y ManageUsers:N");
        }
    }


    // ITSTAFF PERMISSIONS
    public class ITPermissions : IPermissionStrategy
    {
        public bool CanViewStock { get { return true; } }
        public bool CanUpdateStock { get { return true; } }
        public bool CanViewLocation { get { return true; } }
        public bool CanGenerateOrder { get { return false; } } // Can't generate orders
        public bool CanApproveOrder { get { return false; } } // Can't approve order requests
        public bool CanManageUsers { get { return true; } }

        // Show permissions for IT Staff user
        public void PrintPermissions(string username)
        {
            Console.WriteLine("  [" + username + "] IT: View:Y Update:Y Location:Y Order:N Approve:N ManageUsers:Y");
        }
    }

    // ACCESS CONTROLLER: Selects the correct strategy based on role.
    public class AccessController
    {
        private IPermissionStrategy _strategy;
        private User _currentUser;

        public User CurrentUser { get { return _currentUser; } }
        public IPermissionStrategy Permissions { get { return _strategy; } }

        // Get the permissions for requested role
        public static IPermissionStrategy GetStrategyForRole(UserRole role)
        {
            if (role == UserRole.WarehouseStaff) return new WarehousePermissions();
            if (role == UserRole.RetailStaff) return new RetailPermissions();
            if (role == UserRole.StoreManager) return new ManagerPermissions();
            if (role == UserRole.ITStaff) return new ITPermissions();

            throw new Exception("No permission strategy for role: " + role);
        }


        // LOGIN
        public bool Login(User user, string username, string password)
        {
            // Check if credentials are correct
            if (!user.LogIn(username, password))
            {
                Console.WriteLine("  [Login] Incorrect username or password.");
                return false;
            }
            _currentUser = user;

            // Get permissions for user
            _strategy = GetStrategyForRole(user.Role);
            Console.WriteLine("  [Login] Welcome, " + user.Username + ".");

            // Show permissions for that user
            _strategy.PrintPermissions(user.Username);
            return true;
        }

        // Check if user has permission
        public bool Check(bool permission, string action)
        {
            if (!permission)
                Console.WriteLine("  [Access Denied] You do not have permission to: " + action);
            return permission;
        }
    }
}
