using Shimakaze.Struct.Csf;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Shimakaze.ToolKit.CSF.Annotations;
using System.Collections.ObjectModel;
using System.Linq;

namespace Shimakaze.ToolKit.CSF.ViewModel
{
    public class CsfLabelViewModel : INotifyPropertyChanged
    {
        private string _class;
        private string _name;
        public const string DEFAULT_STRING = "(Default)";
        public string Class
        {
            get => this._class;
            set => this._class = this.OnPropertyChanged(value);
        }

        public ObservableCollection<CsfStringViewModel> Contents { get; }

        public string Name
        {
            get => this._name;
            set => this._name = this.OnPropertyChanged(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CsfLabelViewModel(CsfLabel label, string type)
        {
            this.Name = label.Name;
            this.Class = type.ToUpper();
            this.Contents =
                new ObservableCollection<CsfStringViewModel>(label.Select(CsfStringViewModel.CreateInstance));
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
            lbl.AddRange(this.Contents.Select(i=>i.GetString()));
            return lbl;
        }
    }
}