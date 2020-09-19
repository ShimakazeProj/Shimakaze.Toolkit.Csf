using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
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

using Shimakaze.ToolKit.Csf.Data;
using Shimakaze.ToolKit.Csf.ViewModel;

namespace Shimakaze.ToolKit.Csf
{
    /// <summary>
    /// CsfDocumentView.xaml 的交互逻辑
    /// </summary>
    public partial class CsfDocumentView
    {
        private void ClassView_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (this.ValueView.Items.Count > 0) this.ValueView.SelectedIndex = 0;
            this.LabelView.Visibility = this.ClassView.SelectedItem != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public CsfDocumentView()
        {
            this.InitializeComponent();
        }
        public static T GetDescendantByType<T>(DependencyObject element) where T : Visual
        {
            switch (element)
            {
                case null:
                    throw new ArgumentNullException(nameof(element));
                case T result:
                    return result;
                case FrameworkElement frameworkElement:
                    frameworkElement.ApplyTemplate();
                    for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
                    {
                        var result = GetDescendantByType<T>(VisualTreeHelper.GetChild(element, i));
                        if (!(result is null))
                            return result;
                    }
                    return null;
                default:
                    return null;
            }
        }

        public void ClassClone(CsfDocumentViewModel docvm)
        {
            var className = (this.ClassView.SelectedItem as CsfLabelViewModel)?.Class;
            var result = docvm.ClassClone(className);

            this.ClassView.SelectedItems.Clear();
            result.ForEach(i => this.ClassView.SelectedItems.Add(i));
        }

        public void ClassDrop(CsfDocumentViewModel docvm) => docvm.ClassDrop((this.ClassView.SelectedItem as CsfLabelViewModel)?.Class);

        public void ClassRename(CsfDocumentViewModel docvm, string newClassName) => 
            this.SelectLabel(docvm.ClassRename((this.ClassView.SelectedItem as CsfLabelViewModel)?.Class, newClassName));

        public void LabelClone(CsfDocumentViewModel docvm)
        {
            var list = new List<CsfLabelViewModel>(this.ClassView.SelectedItems.Count);
            for (var i = this.ClassView.SelectedItems.Count - 1; i >= 0; i--)
                list.Add(docvm.LabelClone(this.ClassView.SelectedItems[i] as CsfLabelViewModel));
            this.SelectLabel(list);
        }

        public void LabelCreate(CsfDocumentViewModel docvm) => this.SelectLabel(docvm.LabelCreate(this.ClassView.SelectedItem as CsfLabelViewModel));
        public void LabelDrop(CsfDocumentViewModel docvm)
        {
            for (var i = this.ClassView.SelectedItems.Count - 1; i >= 0; i--)
                docvm.LabelDrop(this.ClassView.SelectedItems[i] as CsfLabelViewModel);
        }
        public void SelectLabel(CsfLabelViewModel lbl)
        {
            this.ClassView.ScrollIntoView(this.ClassView.SelectedItem = lbl);
            this.ClassView_Selected(null, null);
        }

        public void SelectLabel(IEnumerable<CsfLabelViewModel> lbl)
        {
            this.ClassView.SelectedItems.Clear();
            _ = lbl.Select(this.ClassView.SelectedItems.Add);
            this.ClassView.ScrollIntoView(lbl.Last());
            this.ClassView_Selected(null, null);
        }

        public void SortListView()
        {
            this.ClassView.Dispatcher.Invoke(() =>
            {
                var view = (CollectionView)CollectionViewSource.GetDefaultView(this.ClassView.ItemsSource);
                var groupDescription = new PropertyGroupDescription();
                groupDescription.StringComparison = StringComparison.OrdinalIgnoreCase;
                groupDescription.SortDescriptions.Add(
                    new SortDescription(nameof(CsfLabelViewModel.Name), ListSortDirection.Ascending));
                
                view.GroupDescriptions?.Add(groupDescription);
                this.ClassView.Items.SortDescriptions.Add(
                    new SortDescription(nameof(CsfLabelViewModel.Class), ListSortDirection.Ascending));
            });
        }
    }
}
