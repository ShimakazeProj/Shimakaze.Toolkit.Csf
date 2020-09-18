using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Win32;

using Shimakaze.Struct.Csf;
using Shimakaze.Struct.Csf.Helper;
using Shimakaze.ToolKit.Csf.Data;
using Shimakaze.ToolKit.Csf.ViewModel;

namespace Shimakaze.ToolKit.Csf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            Application.Current.DispatcherUnhandledException += this.DispatcherUnhandledException;
            this.InitializeComponent();
            this.I18NInitialize();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.StatusText.Text = "Waiting".GetResource();
            this.ProgressBar.IsIndeterminate = true;
            var ofd = new OpenFileDialog
            {
                Filter = "CSF Document|*.csf",
            };
            if (ofd.ShowDialog() != true)
            {
                this.StatusText.Text = "Cancel".GetResource();
                this.ProgressBar.IsIndeterminate = false;
                return;
            }

            GC.TryStartNoGCRegion(150 * 1024 * 1024);
            this.DocumentView.DataContext =
                await FileManager.OpenFile(ofd.FileName, this.StatusChange, this.ProgressBarChange);

            this.ProgressBar.IsIndeterminate = true;
            this.StatusText.Text = "Sorting".GetResource();
            await Task.Run(this.DocumentView.SortListView);
            try
            {
                GC.EndNoGCRegion();
            }
            catch (Exception ex)
            {
            }
            this.ProgressBar.IsIndeterminate = false;
            this.ProgressBar.Value = 0;
            this.StatusText.Text = "Complete".GetResource();
        }
        private void CopyClassButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CopyLabelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DocumentView.CopyLabel(this.GetCsfDocumentViewModel());
            this.StatusText.Text = "Complete".GetResource();
        }

        private void CopyValueButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            switch (e.Exception)
            {
                case FileNotOpenException _:
                    await this.ShowMessageAsync("Wrong".GetResource(), "Wrong_NotOpenFile".GetResource());
                    this.StatusText.Text = "Cancel".GetResource();
                    break;
                case NotImplementedException _:
                    await this.ShowMessageAsync("Oops!", "This Function are Not Implemented.");
                    break;
                default:
                    await File.AppendAllTextAsync("error.log", DateTime.Now.ToString("o"));
                    await File.AppendAllTextAsync("error.log", Environment.NewLine);
                    await File.AppendAllTextAsync("error.log", e.Exception.ToString());
                    var result = await this.ShowMessageAsync("Fatal Error!", e.Exception.ToString(), MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        e.Handled = false;
                    }
                    break;
            }
        }

        private void DropClassButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DropLabelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DocumentView.DropLabel(this.GetCsfDocumentViewModel());
            this.StatusText.Text = "Complete".GetResource();
        }

        private void DropValueButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExportClassButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NewLabelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DocumentView.CreateLabel(this.GetCsfDocumentViewModel());
            this.StatusText.Text = "Complete".GetResource();
        }

        private CsfDocumentViewModel GetCsfDocumentViewModel()
        {
            if (this.DocumentView.DataContext is CsfDocumentViewModel docvm) return docvm;
            throw new FileNotOpenException();
        }
        private void NewValueButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private Task ProgressBarChange(int value, int max) =>
            Task.Run(() => this.Dispatcher.Invoke(() =>
            {
                this.ProgressBar.Value = value;
                this.ProgressBar.Maximum = max;
            }));

        private void RenameClassButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void SaveTo_Click(object sender, RoutedEventArgs e)
        {
            var docvm = this.GetCsfDocumentViewModel();
            this.StatusText.Text = "Waiting".GetResource();
            this.ProgressBar.IsIndeterminate = true;
            var ofd = new SaveFileDialog
            {
                Filter = "CSF Document|*.csf",
                FileName = "ra2md.csf"
            };
            if (ofd.ShowDialog() != true)
            {
                this.StatusText.Text = "Cancel".GetResource();
                this.ProgressBar.IsIndeterminate = false;
                return;
            }

            GC.TryStartNoGCRegion(150 * 1024 * 1024);
            await FileManager.SaveFile(ofd.FileName, docvm, this.StatusChange, this.ProgressBarChange);
            try
            {
                GC.EndNoGCRegion();
            }
            catch (Exception ex)
            {
            }
        }

        private Task StatusChange(string msg, bool progress) =>
            Task.Run(() => this.Dispatcher.Invoke(() =>
            {
                this.StatusText.Text = msg;
                this.ProgressBar.IsIndeterminate = progress;
            }));
    }
}
