using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTerm.NET.Configuration;
using GTerm.NET.Contracts;

using Microsoft.Extensions.Options;

using Spectre.Console;

namespace GTerm.NET.Terminals
{
    public abstract class BaseTerminal : ITerminal
    {
        protected readonly SerialPort port;
        protected readonly IOptions<PortOptions> portOptions;

        public BaseTerminal(SerialPort port, IOptions<PortOptions> portOptions)
        {
            this.port = port;
            this.portOptions = portOptions;
        }

        private void PortDatea_Received(object sender, SerialDataReceivedEventArgs e)
        {
            var _port = (SerialPort)sender;

            Console.Write(_port.ReadExisting());
        }

        protected void AddListener()
        {
            this.port.DataReceived += this.PortDatea_Received;
        }

        public virtual Task<bool> Open()
        {
            try
            {
                port.Open();
                port.DtrEnable = true;
                AnsiConsole.MarkupLine($"[blue]{port.PortName}[/] Opened [green]Successfully[/].");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Failed to open [blue]{port.PortName}[/] It might be open in another program");
                return Task.FromResult(false);
            }
        }

        public virtual Task<bool> Close()
        {
            if (!port.IsOpen)
                return Task.FromResult(true);

            port.Close();

            return Task.FromResult(!port.IsOpen);
        }

        public abstract Task<bool> Run();
    }
}
