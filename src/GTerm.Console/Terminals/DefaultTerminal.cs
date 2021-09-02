using System;
using System.IO.Ports;
using System.Threading.Tasks;

using GTerm.NET.Configuration;

using Microsoft.Extensions.Options;

using Spectre.Console;

namespace GTerm.NET.Terminals
{
    public class DefaultTerminal : BaseTerminal
    {
        public DefaultTerminal(SerialPort port, IOptions<PortOptions> portOptions)
            : base(port, portOptions)
        {
        }

        public override async Task<bool> Run()
        {

            if (!port.IsOpen)
            {
                return false;
            }

            bool exit = false;

            AnsiConsole.MarkupLine($"[orange1]Terminal Ready. Press [lightgreen]CTRL+Q[/] To Exit or [lightgreen]ALT+C[/] to clear[/]");

            this.AddListener();

            await Task.Delay(500);
            do
            {

                ConsoleKeyInfo input = Console.ReadKey(true);

                exit = input.Modifiers.HasFlag(ConsoleModifiers.Control) && input.Key.HasFlag(ConsoleKey.Q);
                bool clear = input.Modifiers.HasFlag(ConsoleModifiers.Alt) && input.Key.HasFlag(ConsoleKey.C);

                if (clear)
                {
                    Console.Clear();
                    continue;
                }

                if (exit)
                {
                    Console.WriteLine("Closing Terminal...");
                    continue;
                }

                SendCharacters(new char[] { input.KeyChar });

            } while (!exit);

            return true;

        }

        private void SendCharacters(char[] chars)
        {
            port.Write(chars, 0, chars.Length);
        }

    }
}
