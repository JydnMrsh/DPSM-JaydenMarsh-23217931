using Phase_2_Warehouse_Management_System.Models;
using Phase_2_Warehouse_Management_System.DesignPatterns;
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
using System.Windows.Shapes;

namespace Phase_2_Warehouse_Management_System
{
    public partial class DashboardWindow : Window
    {
        // Fields
        private readonly AppState _state = AppState.Instance;

        // View models
        public class ItemRow
        {
            public int ItemId { get; set; }
            public string Name { get; set; }
            public string PriceDisplay { get; set; }
            public int WarehouseStock { get; set; }
            public int RetailStock { get; set; }
            public int TotalStock { get; set; }
            public string Location { get; set; }
            public string StockStatus { get; set; }
        }

        public class PermissionRow
        {
            public string Icon { get; set; }
            public string Label { get; set; }
            public string Colour { get; set; }
        }

        public class StrategyRow
        {
            public string Role { get; set; }
            public string Perms { get; set; }
        }

        // Constructor
        public DashboardWindow()
        {
            InitializeComponent();
            SetupForRole();
            RefreshAll();
        }

        // Role-based UI setup
        private void SetupForRole()
        {
            var user = _state.CurrentUser;
            var perms = _state.Access.Permissions;

            TxtNavUser.Text = user.Username + "  ·  " + user.Role;

            PermissionsList.ItemsSource = new List<PermissionRow>
            {
                Perm("View Stock",     perms.CanViewStock),
                Perm("Update Stock",   perms.CanUpdateStock),
                Perm("View Location",  perms.CanViewLocation),
                Perm("Generate Order", perms.CanGenerateOrder),
                Perm("Approve Orders", perms.CanApproveOrder),
                Perm("Manage Users",   perms.CanManageUsers),
            };

            TabUpdateStock.Visibility = perms.CanUpdateStock ? Visibility.Visible : Visibility.Collapsed;
            TabTransfer.Visibility = perms.CanUpdateStock ? Visibility.Visible : Visibility.Collapsed;
            TabIT.Visibility = perms.CanManageUsers ? Visibility.Visible : Visibility.Collapsed;

            PanelGenerateOrder.Visibility = perms.CanGenerateOrder ? Visibility.Visible : Visibility.Collapsed;
            PanelPendingOrders.Visibility = perms.CanApproveOrder ? Visibility.Visible : Visibility.Collapsed;
            PanelManagerActions.Visibility = perms.CanApproveOrder ? Visibility.Visible : Visibility.Collapsed;

            if (perms.CanManageUsers) LoadStrategyViewer();
        }

        private PermissionRow Perm(string label, bool allowed)
        {
            return new PermissionRow
            {
                Icon = allowed ? "Y" : "N",
                Label = "  " + label,
                Colour = allowed ? "#1E7E34" : "#C0392B"
            };
        }

        // Refresh helpers
        private void RefreshAll()
        {
            RefreshStock();
            RefreshItemCombos();
            RefreshOrders();
        }

        private void RefreshStock()
        {
            var rows = _state.Inventory.GetAllItems().Select(item => new ItemRow
            {
                ItemId = item.ItemId,
                Name = item.Name,
                PriceDisplay = "$" + item.Price.ToString("F2"),
                WarehouseStock = item.WarehouseStock,
                RetailStock = item.RetailStock,
                TotalStock = item.TotalStock,
                Location = item.Location.ToString(),
                StockStatus = item.IsLowStock() ? "LOW STOCK" : "OK"
            }).ToList();
            StockGrid.ItemsSource = rows;
        }

        private void RefreshItemCombos()
        {
            var items = _state.Inventory.GetAllItems().ToList();

            CboUpdateItem.ItemsSource = items;
            CboUpdateItem.DisplayMemberPath = "Name";
            if (items.Any()) CboUpdateItem.SelectedIndex = 0;

            CboTransferItem.ItemsSource = items;
            CboTransferItem.DisplayMemberPath = "Name";
            if (items.Any()) CboTransferItem.SelectedIndex = 0;

            CboOrderItem.ItemsSource = items;
            CboOrderItem.DisplayMemberPath = "Name";
            if (items.Any()) CboOrderItem.SelectedIndex = 0;
        }

        private void RefreshOrders()
        {
            if (_state.CurrentHandler != null)
                OrdersGrid.ItemsSource = _state.CurrentHandler.GetPendingOrders();
        }

        private void LoadStrategyViewer()
        {
            StrategyViewer.ItemsSource = new List<StrategyRow>
            {
                new StrategyRow { Role = "WarehouseStaff", Perms = "View ✔  Update ✔  Location ✔  Order ✔  Approve ✗  Users ✗" },
                new StrategyRow { Role = "RetailStaff",    Perms = "View ✔  Update ✔  Location ✔  Order ✗  Approve ✗  Users ✗" },
                new StrategyRow { Role = "StoreManager",   Perms = "View ✔  Update ✗  Location ✔  Order ✗  Approve ✔  Users ✗" },
                new StrategyRow { Role = "ITStaff",        Perms = "View ✔  Update ✔  Location ✔  Order ✗  Approve ✗  Users ✔" },
            };
        }

        // Button handlers
        private void BtnRefreshStock_Click(object sender, RoutedEventArgs e)
        {
            RefreshStock();
        }

        private void BtnUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            if (!(_state.CurrentUser is IStockUpdater updater))
            {
                SetResult(TxtUpdateResult, "Access denied.", false);
                return;
            }
            if (!(CboUpdateItem.SelectedItem is Item item)) return;

            if (!int.TryParse(TxtUpdateQty.Text, out int qty))
            {
                SetResult(TxtUpdateResult, "Invalid quantity.", false);
                return;
            }
            bool isWarehouse = (CboUpdateLocation.SelectedIndex == 0);
            _state.Inventory.UpdateStock(updater, _state.CurrentUser, item.ItemId, qty, isWarehouse);
            RefreshStock();
            SetResult(TxtUpdateResult, "Updated '" + item.Name + "' stock by " + qty + ".", true);
        }

        private void BtnTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (!(_state.CurrentUser is IStockUpdater updater))
            {
                SetResult(TxtTransferResult, "Access denied.", false);
                return;
            }
            if (!(CboTransferItem.SelectedItem is Item item)) return;
            if (!int.TryParse(TxtTransferQty.Text, out int qty) || qty <= 0)
            {
                SetResult(TxtTransferResult, "Enter a positive quantity.", false);
                return;
            }
            bool toRetail = (CboTransferDirection.SelectedIndex == 0);
            if (toRetail)
                _state.Inventory.TransferToRetail(updater, _state.CurrentUser, item.ItemId, qty);
            else
                _state.Inventory.TransferToWarehouse(updater, _state.CurrentUser, item.ItemId, qty);
            RefreshStock();
            SetResult(TxtTransferResult, "Transferred " + qty + "x '" + item.Name + "' " +
                (toRetail ? "to Retail" : "to Warehouse") + ".", true);
        }

        private void BtnGenerateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!(CboOrderItem.SelectedItem is Item item)) return;
            if (!int.TryParse(TxtOrderQty.Text, out int qty) || qty <= 0)
            {
                SetResult(TxtOrderResult, "Enter a positive quantity.", false);
                return;
            }
            if (!(_state.CurrentUser is WarehouseStaff staff))
            {
                SetResult(TxtOrderResult, "Access denied.", false);
                return;
            }
            _state.Inventory.GenerateOrderRequest(staff, item.ItemId, qty);
            RefreshOrders();
            SetResult(TxtOrderResult, "Order sent for " + qty + "x '" + item.Name + "'.", true);
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (!(OrdersGrid.SelectedItem is OrderRequest order)) return;
            _state.CurrentHandler.ApproveOrder(order.RequestId);
            RefreshOrders();
            SetResult(TxtApproveResult, "Order #" + order.RequestId + " approved.", true);
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            if (!(OrdersGrid.SelectedItem is OrderRequest order)) return;
            _state.CurrentHandler.RejectOrder(order.RequestId);
            RefreshOrders();
            SetResult(TxtApproveResult, "Order #" + order.RequestId + " rejected.", false);
        }

        private void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            var username = TxtNewUsername.Text.Trim();
            var role = (CboNewRole.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "";
            if (string.IsNullOrEmpty(username))
            {
                TxtITResult.Text = "Enter a username.";
                TxtITResult.Foreground = new SolidColorBrush(Color.FromRgb(192, 57, 43));
                return;
            }
            TxtITResult.Text = "[IT] Created user '" + username + "' with role " + role + ".";
            TxtITResult.Foreground = new SolidColorBrush(Color.FromRgb(30, 126, 52));
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _state.CurrentUser = null;
            _state.Access = null;
            _state.CurrentHandler = null;
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void TabOrders_GotFocus(object sender, RoutedEventArgs e)
        {
            RefreshOrders();
        }

        private void SetResult(System.Windows.Controls.TextBlock tb, string msg, bool success)
        {
            tb.Text = msg;
            tb.Foreground = success
                ? new SolidColorBrush(Color.FromRgb(30, 126, 52))
                : new SolidColorBrush(Color.FromRgb(192, 57, 43));
        }
    }
}
