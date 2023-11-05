using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class ConsoleViewModel
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        public ConsoleViewModel()
        {
            
        }
    }
}
