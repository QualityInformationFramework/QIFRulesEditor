using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Wpf
{
    /// <summary> Text source interface </summary>
    interface ITextSource
    {
        string GetText();

        event EventHandler TextChanged;
    }
}
