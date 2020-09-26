using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Wpf
{
    /// <summary> Represents a file format. </summary>
    public struct Format
    {
        /// <summary> Constructor. </summary>
        /// <param name="extension"> Extension associated with the format (normally without dot). </param>
        /// <param name="description"> Format description </param>
        public Format(string extension, string description)
        {
            Extension = extension;
            Description = description;
        }

        /// <summary> Checks a file with the specified path has this extension. </summary>
        /// <param name="path"> File to check </param>
        /// <returns> True if the specified file has this extension, otherwise false </returns>
        public bool Check(string path)
        {
            var ext = Path.GetExtension(path).Substring(1).ToLower();
            return ext == Extension;
        }

        /// <summary> Gets or sets extension associated with the format (normally without dot). </summary>
        public string Extension { get; set; }

        /// <summary> Gets or sets format description. </summary>
        public string Description { get; set; }
    }

    /// <summary> Set of known formats. </summary>
    static class Formats
    {
        public static Format Qif { get; } = new Format("qif", "QIF Document");
        public static Format Rml { get; } = new Format("rules", "Rules Markup Language");
    }
}
