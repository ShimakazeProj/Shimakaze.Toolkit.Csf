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

using Fluent;

using MahApps.Metro.Controls;

using Microsoft.Win32;

using Shimakaze.Struct.Csf;
using Shimakaze.Struct.Csf.Helper;
using Shimakaze.ToolKit.CSF.ViewModel;

namespace Shimakaze.ToolKit.CSF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.StatusText.Text = "正在等待对话框返回";
            this.ProgressBar.IsIndeterminate = true;
            var ofd = new OpenFileDialog
            {
                Filter = "CSF Document|*.csf",
            };
            if (ofd.ShowDialog() != true)
            {
                this.StatusText.Text = "操作取消";
                return;
            }
            await using var fs = new FileStream(ofd.FileName, FileMode.Open);
            GC.TryStartNoGCRegion(150 * 1024 * 1024);

            this.StatusText.Text = "正在读取文件";
            var csf = await CsfDocumentHelper.DeserializeAsync(fs, this.ProgressBarChange);

            this.StatusText.Text = "正在转换";
            this.ProgressBar.Maximum = csf.Count;
            this.DocumentView.DataContext = new DocumentViewModel(csf.Head.Version, csf.Head.Language, csf.Select(CreateCsfLabelViewModel), csf.Head.Unknown);

            var view = (CollectionView)CollectionViewSource.GetDefaultView(this.ClassView.ItemsSource);
            var groupDescription = new PropertyGroupDescription(nameof(CsfLabelViewModel.Class));
            view.GroupDescriptions?.Add(groupDescription);

            this.StatusText.Text = "正在排序";
            this.ClassView.Items.SortDescriptions.Add(new SortDescription(nameof(CsfLabelViewModel.Class), ListSortDirection.Ascending));
            this.ClassView.Items.SortDescriptions.Add(new SortDescription(nameof(CsfLabelViewModel.Name), ListSortDirection.Ascending));

            try
            {
                GC.EndNoGCRegion();
            }
            catch
            {
                // ignored
            }
            this.StatusText.Text = "完成";

            CsfLabelViewModel CreateCsfLabelViewModel(CsfLabel lbl, int i)
            {
                this.Dispatcher.Invoke(() => this.ProgressBar.Value = i);
                var tmp = lbl.Name.Split(':');
                return new CsfLabelViewModel(lbl, tmp.Length > 1 ? tmp[0] : CsfLabelViewModel.DEFAULT_STRING);
            }
        }
        private async void SaveTo_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.DocumentView.DataContext is DocumentViewModel docvm))
            {
                this.StatusText.Text = "未打开文件";
                return;
            }
            this.ProgressBar.IsIndeterminate = true;
            var ofd = new SaveFileDialog
            {
                Filter = "CSF Document|*.csf",
                FileName = "ra2md.csf"
            };
            if (ofd.ShowDialog() != true)
            {
                this.StatusText.Text = "操作取消";
                return;
            }
            await using var fs = new FileStream(ofd.FileName, FileMode.Create);
            GC.TryStartNoGCRegion(150 * 1024 * 1024);

            var doc = new CsfDocument();

            this.ProgressBar.IsIndeterminate = false;
            this.ProgressBar.Maximum = docvm.Classes.Count;
            this.StatusText.Text = "正在转换";
            doc.AddRange(docvm.Classes.Select((o, i) =>
            {
                this.Dispatcher.Invoke(() => this.ProgressBar.Value = i);
                return o.GetLabel();
            }));
            this.StatusText.Text = "正在计算文件头";
            doc.Head = new CsfHead
            {
                Version = docvm.Version,
                Language = docvm.Language,
                Unknown = 0x5CF6_98A8,
                LabelCount = doc.Count,
                StringCount = doc.Select((n, i) =>
                {
                    this.Dispatcher.Invoke(() => this.ProgressBar.Value = i);
                    return n.Count;
                }).Sum()
            };
            this.StatusText.Text = "正在写入文件";
            await doc.SerializeAsync(fs, this.ProgressBarChange);

            try
            {
                GC.EndNoGCRegion();
            }
            catch
            {
                // ignored
            }
            this.StatusText.Text = "完成";
        }
        private void ProgressBarChange(int value, int max)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ProgressBar.IsIndeterminate = false;
                this.ProgressBar.Value = value;
                this.ProgressBar.Maximum = max;
            });
        }

        private void ClassView_Selected(object sender, RoutedEventArgs e)
        {
            if (this.ValueView.Items.Count > 0) this.ValueView.SelectedIndex = 0;
        }
    }
}
