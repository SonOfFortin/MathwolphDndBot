using System;

namespace MathwolphDndBot
{
    [Flags]
    public enum Access : ushort
    {
        None = 0,
        Player = 1,
        Admin = 2
    }

    public class User
    {
        public string Name = string.Empty;
        public Access Access = Access.None;
    }
}
