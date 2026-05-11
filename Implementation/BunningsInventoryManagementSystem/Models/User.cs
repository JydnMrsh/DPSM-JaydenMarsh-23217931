using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BunningsInventoryManagementSystem.Models
{
    public enum Role
    {
        ITStaff,
        WarehouseStaff,
        RetailStaff,
        StoreManager
    }

    internal abstract class User
    {
        private static int nextUserID = 1;
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role UserRole { get; set; }

        public User(string username, string password, Role role)
        {
            UserID = nextUserID++;
            UserName = username;
            Password = password;
            UserRole = role;
        }

        public bool Login(string username, string password)
        {
            // Check if the username and password match
            if(username == UserName && password == Password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
