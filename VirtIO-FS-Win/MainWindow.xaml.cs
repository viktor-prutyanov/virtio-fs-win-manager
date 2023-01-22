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
using Microsoft.Win32;

namespace VirtIO_FS_Win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Process process = new Process();

        public MainWindow()
        {
            InitializeComponent();
        }

        private static string GetWinFspVersion()
        {
            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\WinFsp"))
            {
                string installDir = regKey?.GetValue("InstallDir") as string;
                string dllPath = $"{installDir}\\bin\\winfsp-x64.dll";
                FileVersionInfo verInfo = FileVersionInfo.GetVersionInfo(dllPath);

                return $"{verInfo.FileMajorPart}.{verInfo.FileMinorPart}.{verInfo.FileBuildPart}";
            }
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

            process.StartInfo.FileName = PathTB.Text;
            process.StartInfo.Arguments = $"{MountPointArg} {DebugLogFileArg} {DebugFlagsArg} {TagArg} {CaseInsensitiveArg}";

            process.Start();
        }

        private void Button_Click_Select(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                PathTB.Text = dialog.FileName;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.ProgramPath = PathTB.Text;
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PathTB.Text = Properties.Settings.Default.ProgramPath;
            WinFspVersionTB.Text = GetWinFspVersion();
        }

        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
        }
    }
}
