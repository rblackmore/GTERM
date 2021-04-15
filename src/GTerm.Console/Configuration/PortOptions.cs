using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTerm.NET.Configuration
{
    public class PortOptions
    {
        public const string PortSettings = "PortSettings";

        public string Name { get; set; }
        
        public int BaudRate { get; set; }
        
        public int DataBits { get; set; }
        
        public string HandShake { get; set; }

        public float StopBits { get; set; }
        
        public string Parity { get; set; }
    }
}
