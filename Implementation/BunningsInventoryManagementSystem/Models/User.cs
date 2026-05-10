using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunningsInventoryManagementSystem.Models
{
    internal abstract class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

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
