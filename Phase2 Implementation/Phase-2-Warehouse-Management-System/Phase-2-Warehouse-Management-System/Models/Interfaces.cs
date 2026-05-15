using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase_2_Warehouse_Management_System.Models
{
    public interface IStockUpdater { } // Interface showing that a class is allowed to update stock levels
    public interface IStockViewer { } // Interface showing that a class is allowed to view stock info
    public interface ILocationViewer { } // Interface showing that a class is allowed to view item location
    public interface IOrderApprover { } // Interface showing that a class is allowed to approve orders
}
