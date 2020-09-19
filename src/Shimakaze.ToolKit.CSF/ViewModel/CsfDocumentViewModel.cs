using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Shimakaze.Struct.Csf;
using Shimakaze.Struct.Csf.Helper;
using Shimakaze.ToolKit.Csf.Annotations;
using Shimakaze.ToolKit.Csf.Data;

namespace Shimakaze.ToolKit.Csf.ViewModel
{
    public class CsfDocumentViewModel : INotifyPropertyChanged
    {
        private const string _CLONE_SUFFIX = "_CLONE";
        private const string _NEW_CLASS_NAME = "NEW_TYPE";
        private const string _NEW_LABEL_NAME = "NEW_LABEL";
        private const string _NEW_LABEL_VALUE = "new value";
        private int language;
        private int version;
        private static CsfLabelViewModel LabelClassRename(CsfLabelViewModel lbl, string newClassName)
        {
            lbl.Class = newClassName;
            return lbl;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<CsfLabelViewModel> Content { get; }

        public int Language
        {
            get => this.language;
            set
            {
                this.language = value;
                this.OnPropertyChanged();
            }
        }

        public int Version
        {
            get => this.version;
            set
            {
                this.version = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public CsfDocumentViewModel(int version, int language, IEnumerable<CsfLabelViewModel> classes, int unknown = 0)
        {
            this.Version = version;
            this.Language = language;
            this.Content = new ObservableCollection<CsfLabelViewModel>(classes);
        }
        public List<CsfLabelViewModel> ClassClone(string className)
        {
            var sources =
                this.Content
                    .Where(lbl => lbl.Class.Equals(className, StringComparison.OrdinalIgnoreCase))
                    .Select(lbl => LabelClassRename(lbl, lbl.Class + _CLONE_SUFFIX))
                    .ToList();
            sources.ForEach(this.LabelAdd);
            return sources;
        }

        public void ClassDrop(string className) =>
            this.Content
                .Where(lbl => lbl.Class.Equals(className, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .ForEach(this.LabelDrop);

        public List<CsfLabelViewModel> ClassRename(string className, string newClassName) =>
                 this.Content
                    .Where(lbl => lbl.Class.Equals(className, StringComparison.OrdinalIgnoreCase))
                    .Select(lbl => LabelClassRename(lbl, newClassName))
                    .ToList();

        public void LabelAdd(CsfLabelViewModel lbl) => this.Content.Add(lbl);
        public CsfLabelViewModel LabelClone(CsfLabelViewModel lbl)
        {
            var result = lbl.Clone(_CLONE_SUFFIX);
            this.LabelAdd(result);
            return result;
        }

        public CsfLabelViewModel LabelCreate(CsfLabelViewModel lbl)
        {
            CsfLabelViewModel result;
            if (lbl is null) result = CsfLabelViewModel.Create(_NEW_LABEL_NAME + ':' + _NEW_LABEL_NAME, _NEW_LABEL_VALUE);
            else if (lbl.Class.Equals(CsfLabelViewModel.DEFAULT_STRING, StringComparison.OrdinalIgnoreCase))
                result = CsfLabelViewModel.Create(_NEW_LABEL_NAME, _NEW_LABEL_VALUE);
            else result = CsfLabelViewModel.Create(lbl.Class + ':' + _NEW_LABEL_NAME, _NEW_LABEL_VALUE);
            this.LabelAdd(result);
            return result;
        }

        public void LabelDrop(CsfLabelViewModel lbl) => this.Content.Remove(lbl);
    }
}
