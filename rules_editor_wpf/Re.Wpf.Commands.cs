using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Re.Wpf
{
    public class Commands
    {
        public static RoutedCommand CodeCompletion { get; } = new RoutedCommand();
    }
}
