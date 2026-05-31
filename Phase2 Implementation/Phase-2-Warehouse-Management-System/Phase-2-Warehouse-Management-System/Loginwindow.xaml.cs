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
    /// <summary>
    /// Interaction logic for Loginwindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent(); TxtUsername.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e) => TryLogin();

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) TryLogin();
        }

        private void TryLogin()
        {
            TxtError.Visibility = Visibility.Collapsed;

            var username = TxtUsername.Text.Trim();
            var password = TxtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter both username and password.");
                return;
            }

            var state = AppState.Instance;
            var user = state.FindUser(username, password);

            if (user == null)
            {
                ShowError("Incorrect username or password.");
                TxtPassword.Clear();
                TxtUsername.Focus();
                return;
            }

            // Set up session
            var access = new AccessController();
            access.Login(user, username, password);
            state.CurrentUser = user;
            state.Access = access;

            // If manager, attach their order handler
            if (user is StoreManager mgr)
                state.CurrentHandler = state.GetHandler(mgr);
            else
                state.CurrentHandler = null;

            // Open dashboard and close login
            var dashboard = new DashboardWindow();
            dashboard.Show();
            this.Close();
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }
    }
}
