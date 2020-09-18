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
        public const string DEFAULT_STRING = "(Default)";
        public string Class
        {
            get => this.@class;
            set => this.@class = this.OnPropertyChanged(value.ToUpper());
        }

        public ObservableCollection<CsfStringViewModel> Contents { get; }

        public string Name
        {
            get => this.name;
            set => this.name = this.OnPropertyChanged(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CsfLabelViewModel(CsfLabel label, string type) :
            this(type, label.Name,
                 new ObservableCollection<CsfStringViewModel>(
                     label.Select(CsfStringViewModel.CreateInstance)))
        { }

        public CsfLabelViewModel(string type = "TYPE", string name = "NEW_LABEL", string value = "new value")
        {
            this.Contents = new ObservableCollection<CsfStringViewModel>
                {new CsfStringViewModel(CsfStringHelper.Create(value), 0)};
            if (!string.IsNullOrEmpty(type))
            {
                this.Name += type + ':';
                this.Class = type;
            }
            else this.Class = DEFAULT_STRING;
            this.Name += name;
        }
        private CsfLabelViewModel(string type, string name, ObservableCollection<CsfStringViewModel> value)
        {
            this.Name = name;
            this.Class = type;
            this.Contents = value;
        }

        public CsfLabelViewModel Clone()
        {
            var tmp = new CsfStringViewModel[this.Contents.Count];
            this.Contents.CopyTo(tmp, 0);
            return new CsfLabelViewModel(this.Class, this.Name + "_CLONE", new ObservableCollection<CsfStringViewModel>(tmp));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual T OnPropertyChanged<T>(T t, [CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return t;
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