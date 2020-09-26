using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Wpf
{
    /// <summary> Utilities. </summary>
    static class Utilities
    {
        /// <summary> Creates a string based on the specified formats which can be used for open/save file dialogs. </summary>
        /// <param name="formats"> in. Formats </param>
        public static string CreateFilter(IEnumerable<Format> formats)
        {
            var sb = new StringBuilder();

            // add filter for each format
            foreach (var f in formats)
                sb.Append(CreateFilter(f) + "|");
            //sb.Remove(sb.Length - 1, 1);

            // add All files filter
            sb.Append("All Formats|");
            foreach (var f in formats)
                sb.AppendFormat("*.{0};", f.Extension);
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary> Creates a string based on the specified format which can be used for open/save file dialogs. </summary>
        /// <param name="formats"> in. Format </param>
        public static string CreateFilter(Format f)
        {
            return $"{f.Description} (.{f.Extension})|*.{f.Extension}";
        }

        /// <summary> Shows a save file dialog and returns the chosen path. </summary>
        /// <param name="path"> Chosen path </param>
        /// <param name="formats"> Allowed formats </param>
        /// <param name="owner"> Owner window </param>
        /// <returns> True if used has chosen a path </returns>
        public static bool ShowSaveDialog(out string path, IReadOnlyList<Format> formats, System.Windows.Window owner)
        {
            path = string.Empty;
            var dialog = new SaveFileDialog()
            {
                Filter = CreateFilter(formats),
                FilterIndex = formats.Count + 1
            };
            if (!dialog.ShowDialog(owner).GetValueOrDefault(false))
                return false;

            path = dialog.FileName;
            return true;
        }
    }
}
