using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

using Shimakaze.Struct.Csf;
using Shimakaze.Struct.Csf.Helper;
using Shimakaze.ToolKit.Csf.ViewModel;

namespace Shimakaze.ToolKit.Csf.Data
{
    public delegate Task StatusCallbackHandle(string status, bool progressUnknown);
    public delegate Task ProgressCallbackHandle(int progress, int target);

    public static class FileManager
    {
        public static async Task SaveFile(
            string filePath,
            CsfDocumentViewModel docvm,
            StatusCallbackHandle statusCallback = null,
            ProgressCallbackHandle progressCallback = null
            )
        {
            await using var fs = new FileStream(filePath, FileMode.Create);
            var doc = new CsfDocument();

            _ = statusCallback?.Invoke("Converting".GetResource(), false);
            _ = progressCallback?.Invoke(0, docvm.Classes.Count);
            doc.AddRange(docvm.Classes.Select((o, i) =>
            {
                _ = progressCallback?.Invoke(i, docvm.Classes.Count);
                return o.GetLabel();
            }));

            _ = statusCallback?.Invoke("CalculatingHeader".GetResource(), true);
            doc.Head = new CsfHead
            {
                Version = docvm.Version,
                Language = docvm.Language,
                Unknown = 0x5CF6_98A8,
                LabelCount = doc.Count,
                StringCount = doc.Select(n => n.Count).Sum()
            };
            _ = statusCallback?.Invoke("WritingFile".GetResource(), false);
            await doc.SerializeAsync(fs, (i, j) =>
                                         _ = progressCallback?.Invoke(i, j));

            _ = statusCallback?.Invoke("Complete".GetResource(), false);
            _ = progressCallback?.Invoke(0, 1);
        }

        public static async Task<CsfDocumentViewModel> OpenFile(
            string filePath,
            StatusCallbackHandle statusCallback = null,
            ProgressCallbackHandle progressCallback = null
        )
        {
            await using var fs = new FileStream(filePath, FileMode.Open);
            statusCallback?.Invoke("ReadingFile".GetResource(), false);
            var csf = await CsfDocumentHelper.DeserializeAsync(fs, (i, j) => _ = progressCallback?.Invoke(i, j));

            _ = statusCallback?.Invoke("Converting".GetResource(), false);
            return new CsfDocumentViewModel(csf.Head.Version, csf.Head.Language, csf.Select((lbl, i) =>
            {
                _ = progressCallback?.Invoke(i, csf.Count);
                var tmp = lbl.Name.Split(':');
                return new CsfLabelViewModel(lbl, tmp.Length > 1 ? tmp[0] : CsfLabelViewModel.DEFAULT_STRING);
            }));
        }
    }
}