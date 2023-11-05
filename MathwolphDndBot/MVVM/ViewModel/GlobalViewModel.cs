using MathwolphDndBot.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class GlobalViewModel
    {
        public static GlobalViewModel Instance { get; } = new GlobalViewModel();

		public ObservableCollection<Terminal> Terminals { get; set; }

        private bool _isAwsome;

		public bool IsAwsome
        {
			get { return _isAwsome; }
			set { _isAwsome = value; }
		}

        public GlobalViewModel()
        {
            Terminals = new ObservableCollection<Terminal>();

            //Terminals.Add(new Terminal() 
            //{ 
            //    Message = "Test None",
            //    Type = TerminalType.None,
            //    Moment = DateTime.Now
            //});
            //Terminals.Add(new Terminal()
            //{
            //    Message = "Test Warning",
            //    Type = TerminalType.Warning,
            //    Moment = DateTime.Now
            //});
            //Terminals.Add(new Terminal()
            //{
            //    Message = "Test Succes",
            //    Type = TerminalType.Succes,
            //    Moment = DateTime.Now
            //});
            //Terminals.Add(new Terminal()
            //{
            //    Message = "Test Error",
            //    Type = TerminalType.Error,
            //    Moment = DateTime.Now
            //});
        }
    }
}
