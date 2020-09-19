using System;
using System.Collections.Generic;
using Shimakaze.Struct.Csf;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Linq;
using Shimakaze.ToolKit.Csf.Annotations;
using Shimakaze.Struct.Csf.Helper;

namespace Shimakaze.ToolKit.Csf.ViewModel
{
    public class CsfLabelViewModel : INotifyPropertyChanged
    {
        private string @class;
        private string name;
        private CsfLabelViewModel(string name, ObservableCollection<CsfStringViewModel> value)
        {
            this.Name = name;
            this.Contents = value;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public const string DEFAULT_STRING = "(Default)";
        public string Class
        {
            get => this.@class.ToUpper();
            set
            {
                var a = this.name.IndexOf(this.@class, StringComparison.OrdinalIgnoreCase);
                this.name = this.name.Remove(a, this.@class.Length).Insert(a, value);
                this.@class = value.ToUpper();
                this.OnPropertyChanged(nameof(this.Name));
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<CsfStringViewModel> Contents { get; }

        public string Name
        {
            get => this.name;
            set
            {
                var cn = value.Split(':');
                this.@class = cn.Length > 1 ? cn[0] : DEFAULT_STRING;
                this.name = value;
                this.OnPropertyChanged(nameof(this.Class));
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public CsfLabelViewModel(CsfLabel label) :
                    this(label.Name, new ObservableCollection<CsfStringViewModel>(
                     label.Select(CsfStringViewModel.CreateInstance)))
        { }
        public static CsfLabelViewModel Create(string name, string value) =>
            new CsfLabelViewModel(name, new ObservableCollection<CsfStringViewModel>
                                      {new CsfStringViewModel(CsfStringHelper.Create(value), 0)});

        public CsfLabelViewModel Clone(string suffix = null)
        {
            var tmp = new CsfStringViewModel[this.Contents.Count];
            this.Contents.CopyTo(tmp, 0);
            return new CsfLabelViewModel(this.Name + suffix, new ObservableCollection<CsfStringViewModel>(tmp));
        }
        public CsfLabel GetLabel()
        {
            var lbl = new CsfLabel()
            {
                Name = this.Name
            };
            lbl.AddRange(this.Contents.Select(i => i.GetString()));
            return lbl;
        }

        public bool NameEquals(CsfLabelViewModel that) => this.Name.Equals(that.Name, System.StringComparison.OrdinalIgnoreCase);
    }
}