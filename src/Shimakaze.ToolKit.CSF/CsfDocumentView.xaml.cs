using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using MahApps.Metro.Controls;

using Shimakaze.ToolKit.Csf.ViewModel;

namespace Shimakaze.ToolKit.Csf
{
    /// <summary>
    /// CsfDocumentView.xaml 的交互逻辑
    /// </summary>
    public partial class CsfDocumentView
    {
        public CsfDocumentView()
        {
            this.InitializeComponent();
        }

        private void ClassView_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (this.ValueView.Items.Count > 0) this.ValueView.SelectedIndex = 0;
            this.LabelView.Visibility = this.ClassView.SelectedItem != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SortListView()
        {
            this.ClassView.Dispatcher.Invoke(() =>
            {
                var view = (CollectionView)CollectionViewSource.GetDefaultView(this.ClassView.ItemsSource);
                var groupDescription = new PropertyGroupDescription(nameof(CsfLabelViewModel.Class));
                view.GroupDescriptions?.Add(groupDescription);
                this.ClassView.Items.SortDescriptions.Add(
                    new SortDescription(nameof(CsfLabelViewModel.Class), ListSortDirection.Ascending));
                this.ClassView.Items.SortDescriptions.Add(
                    new SortDescription(nameof(CsfLabelViewModel.Name), ListSortDirection.Ascending));
            });
        }

        private void SplitView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth < 800)
            {
                this.DisplayMode = SplitViewDisplayMode.Overlay;
                this.OpenPane.Visibility = Visibility.Visible;
            }
            else
            {
                this.DisplayMode = SplitViewDisplayMode.CompactInline;
                this.OpenPane.Visibility = Visibility.Collapsed;
            }
        }

        private void OpenPane_Click(object sender, RoutedEventArgs e)
        {
            this.IsPaneOpen = true;
        }

        public void CreateLabel(CsfDocumentViewModel docvm) => this.SelectLabel(docvm.CreateLabel(this.ClassView.SelectedItem as CsfLabelViewModel));

        public void DropLabel(CsfDocumentViewModel docvm)
        {
            for (var i = this.ClassView.SelectedItems.Count - 1; i >= 0; i--)
                docvm.DropLabel(this.ClassView.SelectedItems[i] as CsfLabelViewModel);
        }

        public void CopyLabel(CsfDocumentViewModel docvm)
        {
            var list = new List<CsfLabelViewModel>(this.ClassView.SelectedItems.Count);
            for (var i = this.ClassView.SelectedItems.Count - 1; i >= 0; i--)
                list.Add(docvm.CopyLabel(this.ClassView.SelectedItems[i] as CsfLabelViewModel));
            this.ClassView.SelectedItems.Clear();
            list.ForEach(i => this.ClassView.SelectedItems.Add(i));
        }

        public void SelectLabel(CsfLabelViewModel lbl)
        {
            this.ClassView.ScrollIntoView(this.ClassView.SelectedItem = lbl);
            this.ClassView_Selected(null, null);
        }
        public void SelectLabel(IEnumerable<CsfLabelViewModel> lbl)
        {
            _ = lbl.Select(i => this.ClassView.SelectedItems.Add(i));
            this.ClassView.ScrollIntoView(lbl.Last());
            this.ClassView_Selected(null, null);
        }
    }
}
