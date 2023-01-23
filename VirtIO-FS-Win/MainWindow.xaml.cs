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
using System.Collections.ObjectModel;

namespace VirtIO_FS_Win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<FSProcess> fsProcs = new ObservableCollection<FSProcess>();
        public MainWindow()
        {
            InitializeComponent();

            FSProcListBox.ItemsSource = fsProcs;
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

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            FSProcess fsProcess = new FSProcess(BinaryPathTB.Text, MountPointTB.Text, DebugLogFileTB.Text, DebugFlagsTB.Text, TagTB.Text, CaseInsensitiveCB.IsChecked ?? false);

            if (fsProcess.Start())
            {
                fsProcs.Add(fsProcess);
                FSProcListBox.SelectedIndex = -1;
            }
        }

        private void Button_Click_Select(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                BinaryPathTB.Text = dialog.FileName;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.ProgramPath = BinaryPathTB.Text;
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BinaryPathTB.Text = Properties.Settings.Default.ProgramPath;
            WinFspVersionTB.Text = GetWinFspVersion();
        }

        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            if (fsProcs.Count() != 0)
            {
                int idx = FSProcListBox.SelectedIndex;

                if (idx != -1)
                {
                    fsProcs[idx].Kill();
                    fsProcs.RemoveAt(idx);
                }
            }
        }

        private void FSProcListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = FSProcListBox.SelectedIndex;

            if (idx != -1)
            {
                MountPointTB.Text = fsProcs[idx].mountPoint;
                DebugLogFileTB.Text = fsProcs[idx].debugLogFile;
                DebugFlagsTB.Text = fsProcs[idx].debugFlags;
                TagTB.Text = fsProcs[idx].tag;
                CaseInsensitiveCB.IsChecked = fsProcs[idx].caseInsensitive;
            }
        }
    }
}
