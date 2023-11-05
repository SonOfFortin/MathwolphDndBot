using System;

namespace MathwolphDndBot.Class
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
        public string Name { get; set; }
        public Access Access { get; set; }

        public User() 
        {
            Name = string.Empty;
            Access = Access.None;
        }
    }
}
