using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Shimakaze.Struct.Csf;
using Shimakaze.Struct.Csf.Helper;
using Shimakaze.ToolKit.Csf.Annotations;

namespace Shimakaze.ToolKit.Csf.ViewModel
{
    public class CsfDocumentViewModel : INotifyPropertyChanged
    {
        private int _language;
        private int _version;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Version
        {
            get => this._version;
            set => this._version = this.OnPropertyChanged(value);
        }

        public int Language
        {
            get => this._language;
            set => this._language = this.OnPropertyChanged(value);
        }

        public ObservableCollection<CsfLabelViewModel> Classes { get; }

        public CsfDocumentViewModel(int version, int language, IEnumerable<CsfLabelViewModel> classes, int unknown = 0)
        {
            this.Version = version;
            this.Language = language;
            this.Classes = new ObservableCollection<CsfLabelViewModel>(classes);
        }


        [NotifyPropertyChangedInvocator]
        protected virtual T OnPropertyChanged<T>(T t, [CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return t;
        }

        public CsfLabelViewModel CreateLabel(CsfLabelViewModel lbl)
        {
            var result = lbl is null
                             ? new CsfLabelViewModel()
                             : lbl.Class.Equals(CsfLabelViewModel.DEFAULT_STRING, StringComparison.OrdinalIgnoreCase)
                                 ? new CsfLabelViewModel(string.Empty)
                                 : new CsfLabelViewModel(lbl.Class);
            this.Classes.Add(result);
            return result;
        }

        public void DropLabel(CsfLabelViewModel lbl) => this.Classes.Remove(lbl);

        public CsfLabelViewModel CopyLabel(CsfLabelViewModel lbl)
        {
            var result = lbl.Clone();
            this.Classes.Add(result);
            return result;
        }
    }
}
