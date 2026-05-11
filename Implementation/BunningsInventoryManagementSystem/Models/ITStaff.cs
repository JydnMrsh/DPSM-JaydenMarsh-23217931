using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class ITStaff : User
    {
        private InventorySystem inventorySystem = new InventorySystem();

        public ITStaff(string username, string password, Role role) : base(username, password, role)
        {
        }


        // IT staff can create user accounts with different roles
        public User CreateUser(string username, string password, Role role)
        {
            switch(role)
            {
                case Role.ITStaff:
                    inventorySystem.Logger?.Invoke($"Creating IT staff user: {username}");
                    return new ITStaff(username, password, role);
                case Role.WarehouseStaff:
                    inventorySystem.Logger?.Invoke($"Creating Warehouse staff user: {username}");
                    return new WarehouseStaff(username, password, role);
                case Role.RetailStaff:
                    inventorySystem.Logger?.Invoke($"Creating Retail staff user: {username}");
                    return new RetailStaff(username, password, role);
                case Role.StoreManager:
                    inventorySystem.Logger?.Invoke($"Creating Store Manager user: {username}");
                    return new StoreManager(username, password, role);
                default:
                    throw new Exception("Invalid role.");
            }
        }

        // Change user permissions by updating their role
        // Currently changing the role will not change the user type, but it will update their role
        public void ManageUserPermission(User user, Role newRole)
        {
            user.UserRole = newRole;
        }
    }
}
