using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTerm.NET.Contracts
{
    public interface ITerminal
    {
        Task<bool> Open();

        Task<bool> Run();

    }
}
