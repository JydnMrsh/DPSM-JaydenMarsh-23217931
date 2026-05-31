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
    /// Interaction logic for LauncherWindow.xaml
    /// </summary>
    public partial class LauncherWindow : Window
    {
        public LauncherWindow()
        {
            InitializeComponent();
        }

        private void BtnGUI_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        private void BtnConsole_Click(object sender, RoutedEventArgs e)
        {
            AllocConsole();

            IntPtr handle = CreateFile(
                "CONOUT$",
                0x40000000,  // GENERIC_WRITE
                2,           // FILE_SHARE_WRITE
                IntPtr.Zero,
                3,           // OPEN_EXISTING
                0,
                IntPtr.Zero);

            var fs = new System.IO.FileStream(handle, System.IO.FileAccess.Write);
            var writer = new System.IO.StreamWriter(fs);
            writer.AutoFlush = true;
            Console.SetOut(writer);

            ConsoleDemo.Run();

            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool SetConsoleTitle(string title);
    }
}
