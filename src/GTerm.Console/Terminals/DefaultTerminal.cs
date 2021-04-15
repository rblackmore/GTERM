using GTerm.NET.Configuration;
using GTerm.NET.Contracts;
using Microsoft.Extensions.Options;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTerm.NET.Terminals
{
    public class DefaultTerminal : ITerminal
    {
        private readonly SerialPort port;
        private readonly IOptions<PortOptions> portOptions;

        public DefaultTerminal(SerialPort port, IOptions<PortOptions> portOptions)
        {
            this.port = port;
            this.portOptions = portOptions;
            this.port.DataReceived += this.PortDatea_Received;
        }

        private void PortDatea_Received(object sender, SerialDataReceivedEventArgs e)
        {
            var _port = (SerialPort)sender;

            Console.Write(_port.ReadExisting());
        }
        public Task<bool> Open()
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

        public Task<bool> Run()
        {

            if (!port.IsOpen)
            {
                return Task.FromResult(false);
            }

            bool exit = false;

            AnsiConsole.MarkupLine($"[orange1]Terminal Ready. Press [lightgreen]CTRL+Q[/] To Exit[/]");

            do
            {

                ConsoleKeyInfo input = Console.ReadKey(true);

                exit = input.Modifiers.HasFlag(ConsoleModifiers.Control) && input.Key.HasFlag(ConsoleKey.Q);

                if (exit)
                {
                    Console.WriteLine("Exiting");
                    continue;
                }

                SendCharacters(new char[] { input.KeyChar });

            } while (!exit);

            return Task.FromResult(true);

        }

        private void SendCharacters(char[] chars)
        {
            port.Write(chars, 0, chars.Length);
        }

    }
}
