﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathwolphDndBot.MVVM.ViewModel
{
    class MessageViewModel
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        public MessageViewModel()
        {
            
        }
    }
}
