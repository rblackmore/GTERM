using System;
using System.IO.Ports;
using System.Threading.Tasks;

using GTerm.NET.Configuration;

using Microsoft.Extensions.Options;

using Spectre.Console;

namespace GTerm.NET.Terminals
{
    public abstract class BaseTerminal
    {
        protected readonly SerialPort port;
        protected readonly IOptions<PortOptions> portOptions;

        public BaseTerminal(SerialPort port, IOptions<PortOptions> portOptions)
        {
            this.port = port;
            this.portOptions = portOptions;
        }

        private void PortData_Received(object sender, SerialDataReceivedEventArgs e)
        {
            var _port = (SerialPort)sender;

            Console.Write(_port.ReadExisting());
        }

        protected void AddListener(SerialDataReceivedEventHandler handler = null)
        {
            this.port.DataReceived += (handler != null) ? handler : PortData_Received;
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

        protected bool ExitKeyPressed(ConsoleKeyInfo input)
        {
            return input.Modifiers.HasFlag(ConsoleModifiers.Control) && input.Key.HasFlag(ConsoleKey.Q);
        }

        protected bool ClearKeyPressed(ConsoleKeyInfo input)
        {
            return input.Modifiers.HasFlag(ConsoleModifiers.Alt) && input.Key.HasFlag(ConsoleKey.C);
        }

        protected bool ClearIfClearKeyPressed(ConsoleKeyInfo input)
        {
            bool clear = this.ClearKeyPressed(input);

            if (clear)
                Console.Clear();

            return clear;
        }

        protected void TerminalStartWelcomeMessage()
        {
            AnsiConsole.MarkupLine($"[orange1]Terminal Ready. Press [lightgreen]CTRL+Q[/] To Exit or [lightgreen]ALT+C[/] to clear[/]");
        }

        public abstract Task<bool> Run();
    }
}
