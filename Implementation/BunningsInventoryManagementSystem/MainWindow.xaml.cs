using BunningsInventoryManagementSystem.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BunningsInventoryManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InventorySystem inventorySystem = new InventorySystem();

        public MainWindow()
        {
            InitializeComponent();

            inventorySystem.Logger = Log; // Set the logger action to the Log method in this class
            inventorySystem.Demo(); // Run demo
        }

        // Log messages to the console box in the UI
        public void Log(string message)
        {
            ConsoleBox.Text += message + "\n";
        }
    }
}
