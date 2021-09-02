using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTerm.NET.Contracts;

namespace GTerm.NET.Terminals
{
    public class ReceiveOnlyTerminal : ITerminal
    {
        public Task<bool> Close()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Open()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Run()
        {
            throw new NotImplementedException();
        }
    }
}
