using System;
using System.IO.Ports;
using System.Threading.Tasks;

using GTerm.NET.Configuration;

using Microsoft.Extensions.Options;

using Spectre.Console;

namespace GTerm.NET.Terminals
{
    public class ReceiveOnlyTerminalOptions
    {

        public bool ASCII { get; set; }
        
        public bool HEX { get; set; }

        public bool HexStringToASCII { get; set; }
    }

    public class ReceiveOnlyTerminal : BaseTerminal
    {

        public const string ConfigurationName = "Terminals:ReceiveOnlyTerminal";

        private readonly IOptions<ReceiveOnlyTerminalOptions> terminalOptions;

        public ReceiveOnlyTerminal(
            SerialPort port, 
            IOptions<PortOptions> portOptions, 
            IOptions<ReceiveOnlyTerminalOptions> terminalOptions) 
                : base(port, portOptions)
        {
            this.terminalOptions = terminalOptions;
        }

        private void ReceiveOnlyHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // TODO: Print Different formats according to options.
            var _port = (SerialPort)sender;
            Console.Write("ReceiveOnlyTerminal: ");
            Console.WriteLine(_port.ReadExisting());
        }

        public override async Task<bool> Run()
        {
            if (!this.port.IsOpen)
                return false;

            bool exit = false;

            this.TerminalStartWelcomeMessage();

            this.AddListener(this.ReceiveOnlyHandler);

            await Task.Delay(500);

            do
            {
                var input = Console.ReadKey(true);

                exit = this.ExitKeyPressed(input);

                if (ClearIfClearKeyPressed(input))
                    continue;

                if (exit)
                {
                    Console.WriteLine("Closing Terminal ...");
                    continue;
                }


            } while (!exit);

            return true;
        }
    }
}
