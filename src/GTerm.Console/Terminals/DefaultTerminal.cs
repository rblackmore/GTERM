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
        public const string ConfigurationName = "Terminals:DefaultTerminal";

        public DefaultTerminal(SerialPort port, IOptions<PortOptions> portOptions)
            : base(port, portOptions)
        {
        }

        public override async Task<bool> Run()
        {

            if (!port.IsOpen)
                return false;

            bool exit = false;

            this.TerminalStartWelcomeMessage();

            this.AddListener();

            await Task.Delay(500);


            do
            {

                var input = Console.ReadKey(true);

                exit = this.ExitKeyPressed(input);
                
                if (ClearIfClearKeyPressed(input))
                    continue;

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
