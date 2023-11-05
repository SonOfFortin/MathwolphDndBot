using System;

namespace MathwolphDndBot.MVVM.Model
{
    public enum TerminalType
    {
        None = 0,
        Succes = 1,
        Warning = 2,
        Error = 3
    }

    public class Terminal
    {
        public string? Message { get; set; }
        public TerminalType Type { get; set; }
        public DateTime Moment { get; set; }

        //public Terminal(string message, TerminalType type = TerminalType.None) { 
        //    Message = message;
        //    Type = type;
        //    Moment = DateTime.Now;
        //}
    }
}
