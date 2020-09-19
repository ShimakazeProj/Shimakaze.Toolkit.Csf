using System.ComponentModel;
using System.Runtime.CompilerServices;

using Shimakaze.Struct.Csf;
using Shimakaze.Struct.Csf.Helper;
using Shimakaze.ToolKit.Csf.Annotations;

namespace Shimakaze.ToolKit.Csf.ViewModel
{
    public class CsfStringViewModel : INotifyPropertyChanged
    {
        private string _content;
        private string _extra;

        public string Content
        {
            get => this._content;
            set
            {
                this._content = value;
                this.OnPropertyChanged();
            }
        }
        public int Index { get; }
        public string Extra
        {
            get => this._extra;
            set
            {
                this._extra = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public CsfStringViewModel(CsfString str, int index)
        {
            this.Content = str.Content;
            if (str is CsfWString wstr) this.Extra = wstr.Extra;
            this.Index = index;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static CsfStringViewModel CreateInstance(CsfString arg, int index) => new CsfStringViewModel(arg, index);

        public CsfString GetString() => string.IsNullOrEmpty(this.Extra) ? CsfStringHelper.Create(this.Content) : CsfStringHelper.Create(this.Content, this.Extra);
    }
}