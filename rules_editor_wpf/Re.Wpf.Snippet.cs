using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Wpf
{
    public class Snippet
    {
        public Snippet(string text)
        {
            Text = text;
        }

        public Snippet(Constant constant)
        {
            Text = constant.Name;
        }

        public string Text { get; }
    }
}
