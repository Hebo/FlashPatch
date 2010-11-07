using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;

namespace FlashPatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string filePath;
        static string safeFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
                safeFilePath = ofd.SafeFileName;

                filename.Content = safeFilePath;
            }
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            if (filePath != null)
            {
                bool patchResponse = false;
                try
                {
                    patchResponse = Patch.apply(filePath);
                    if (!patchResponse)
                    {
                        string messageBoxText = "Error patching file: Could not find location to patch";
                        string caption = "Patch Failed";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Error;
                        MessageBox.Show(messageBoxText, caption, button, icon);
                    }
                }
                catch (Exception ex)
                {
                    string messageBoxText = ex.Message;
                    string caption = "Patch Error";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                }

                if (patchResponse)
                {
                    string messageBoxText = "Patch sucessfully applied! Restart your browser for the changes to take effect.";
                    string caption = "Patch Success";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Exclamation;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                }
            }
            else
            {
                string messageBoxText = "Error: No file selected";
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }


        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
