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
using System.Diagnostics;

namespace VirtIO_FS_Win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process process;

        public MainWindow()
        {
            InitializeComponent();
        }

        private static string MakeStrArg(string arg, string key)
        {
            return !string.IsNullOrEmpty(arg) ? $"{key} {arg}" : "";
        }

        private static string MakeBoolArg(bool arg, string key)
        {
            return arg ? key : "";
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            string MountPointArg = MakeStrArg(MountPointTB.Text, "-m");
            string DebugLogFileArg = MakeStrArg(DebugLogFileTB.Text, "-D");
            string DebugFlagsArg = MakeStrArg(DebugFlagsTB.Text, "-d");
            string TagArg = MakeStrArg(TagTB.Text, "-t");
            string CaseInsensitiveArg = MakeBoolArg(CaseInsensitiveCB.IsChecked ?? false, "-i");

            process = new Process();
            process.StartInfo.FileName = PathTB.Text;
            process.StartInfo.Arguments = $"{MountPointArg} {DebugLogFileArg} {DebugFlagsArg} {TagArg} {CaseInsensitiveArg}";

            process.Start();
            process.WaitForExit();
        }

        private void Button_Click_Select(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                PathTB.Text = dialog.FileName;
            }
        }
    }
}
