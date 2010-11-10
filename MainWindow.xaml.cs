using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Win32;

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

                //update ui
                filename.Content = safeFilePath;
                BtnApply.IsEnabled = true;

                // Show restore button if backup file exists
                if (File.Exists(filePath + "_backup"))
                {
                    BtnRestore.IsEnabled = true;
                }
                else
                {
                    BtnRestore.IsEnabled = false;
                }       
            }
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
			bool patchResponse = false;
			try
			{
				patchResponse = Patch.apply(filePath);
				if (!patchResponse)
				{
					string messageBoxText = "Error patching file: Could not find location to patch. You may have selected the wrong file or the version of Flash is unsupported.";
                    MessageBox.Show(messageBoxText, "Patch Failed", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch (Exception ex)
			{
				string messageBoxText = ex.Message;;
                MessageBox.Show(messageBoxText, "Patch Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			if (patchResponse)
			{
				string messageBoxText = "Patch sucessfully applied! Restart your browser for the changes to take effect.";
                MessageBox.Show(messageBoxText, "Patch Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            bool restoreResponse = false;
            try
            {
                restoreResponse = Patch.restore(filePath);
                if (!restoreResponse)
                {
                    string messageBoxText = "Error restoring file: Could not find backup";
                    MessageBox.Show(messageBoxText, "Patch Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = ex.Message;
                MessageBox.Show(messageBoxText, "Restore Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (restoreResponse)
            {
                string messageBoxText = "Backup file sucessfully restored! Restart your browser for the changes to take effect.";
                MessageBox.Show(messageBoxText, "Restore Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
