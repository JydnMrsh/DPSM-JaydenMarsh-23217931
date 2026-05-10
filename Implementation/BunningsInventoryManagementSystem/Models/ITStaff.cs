using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal class ITStaff : User
    {
        // IT staff can create user accounts with different roles
        public User CreateUser(string username, string password, Role role)
        {
            switch(role)
            {
                case Role.ITStaff:
                    return new ITStaff { UserName = username, Password = password, UserRole = role };
                case Role.WarehouseStaff:
                    return new WarehouseStaff { UserName = username, Password = password, UserRole = role };
                case Role.RetailStaff:
                    return new RetailStaff { UserName = username, Password = password, UserRole = role };
                case Role.StoreManager:
                    return new StoreManager { UserName = username, Password = password, UserRole = role };
                default:
                    Console.WriteLine("Invalid role.");
                    return null;
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
