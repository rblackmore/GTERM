using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTerm.NET.Contracts;
using GTerm.NET.Menu;

namespace GTerm.NET
{
    public class SerialPortConfigScreen : IScreen
    {
        private ScreenResult result = new();

        private readonly SerialPort port;

        public SerialPortConfigScreen(SerialPort port)
        {
            this.port = port;
        }
        public Task<ScreenResult> Display()
        {
            var options = new List<MenuOption>
                {
                    new MenuOption($"1. Select Port({port.PortName})", () => SelectPortName()),
                    new MenuOption($"2. Set Baudrate({port.BaudRate})", () => SelectBaudRate()),
                    new MenuOption($"3. Set Data Bits({port.DataBits})", () => SetDataBits()),
                    new MenuOption($"4. Set Handshake({port.Handshake})", () => SetHandshake()),
                    new MenuOption($"5. Set Parity({port.Parity})", () => SetParity()),
                    new MenuOption($"6. Set StopBits({port.StopBits})", () => SetStopBits()),
                    new MenuOption($"7. Return to Previous Menu", () => Return()),
                };

            var prompt = new SelectionPrompt<MenuOption>()
                .Title("Configure Serial Port:")
                .AddChoices(options);

            prompt.Converter = mo => mo.DisplayText;

            var selection = AnsiConsole.Prompt(prompt);

            selection.Action();

            return Task.FromResult(result);
        }

        private Task Return()
        {
            result.NavigationOption = NavigationOption.Previous;

            return Task.CompletedTask;
        }

        private Task SelectPortName()
        {
            var names = SerialPort.GetPortNames();

            var portName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Choose Port({port.PortName}):")
                        .PageSize(10)
                        .AddChoices(names)
                );

            port.PortName = portName;

            return Task.CompletedTask;
        }

        private Task SelectBaudRate()
        {
            var bauds = new int[]
            {
                300,
                600,
                1200,
                2400,
                4800,
                9600,
                14400,
                19200,
                28800,
                38400,
                56000,
                57600,
                115200,
                128000,
                256000
            };

            var baudRate = AnsiConsole.Prompt(
                    new SelectionPrompt<int>()
                    .Title($"Select BaudRate({port.BaudRate}):")
                    .PageSize(10)
                    .AddChoices(bauds)
                );

            port.BaudRate = baudRate;

            return Task.CompletedTask;
        }
        private Task SetDataBits()
        {
            int[] options = new[] { 5, 6, 7, 8 };

            var dataBits = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .Title($"Data Bits ({ port.DataBits}")
                .AddChoices(options));

            port.DataBits = dataBits;

            return Task.CompletedTask;
        }

        private Task SetHandshake()
        {
            var options = Enum.GetValues<Handshake>();

            var handshake = AnsiConsole.Prompt(
                new SelectionPrompt<Handshake>()
                .Title($"Handshake({port.Handshake}):")
                .AddChoices(options));

            port.Handshake = handshake;

            return Task.CompletedTask;
        }

        private Task SetParity()
        {
            throw new NotImplementedException();
        }


        private Task SetStopBits()
        {
            StopBits[] options = new[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<StopBits>()
                .Title($"Stop Bits ({port.StopBits}):")
                .AddChoices(options));

            port.StopBits = selection;

            return Task.CompletedTask;
        }

        public void SetDefaults()
        {
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Handshake = Handshake.None;
            port.StopBits = StopBits.One;
            port.Parity = Parity.None;
        }

        public Task Exit()
        {
            return Task.CompletedTask;
        }
    }
}
