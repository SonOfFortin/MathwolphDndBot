using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathwolphDndBot.MVVM.Model
{
    class ServerModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Country {  get; set; }
    }
}
