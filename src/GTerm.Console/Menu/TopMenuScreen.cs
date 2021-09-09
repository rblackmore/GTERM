using GTerm.NET.Contracts;
using GTerm.NET.Menu.TerminalScreens;
using GTerm.NET.Terminals;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTerm.NET.Menu
{
    public class TopMenuScreen : IScreen, IDisposable
    {

        private ScreenResult result = new();

        private IEnumerable<ITerminal> terminals;

        public TopMenuScreen(IEnumerable<ITerminal> terminals)
        {
            this.terminals = terminals;
        }

        public Task<ScreenResult> Display()
        {
            var options = new List<MenuOption>
            {
                new MenuOption("1. Configure Serial Port", () => LoadSerialPortConfguration()),
                new MenuOption("2. Open Default Terminal", () => OpenAndRunDefaultTerminalScreen()),
                // new MenuOption("3. Open Receive Only Terminal", () => OpenAndRunReceiveOnlyTerminalScreen()),
                new MenuOption("0. Exit", () => Return()),
            };

            var prompt = new SelectionPrompt<MenuOption>()
                .Title("Main Menu")
                .AddChoices(options);

            prompt.Converter = mo => mo.DisplayText;

            var selection = AnsiConsole.Prompt(prompt);

            selection.Action();

            return Task.FromResult(result);
        }

        private async Task OpenAndRunPort()
        {
            var terminal = this.terminals.First(t => t.GetType() == typeof(DefaultTerminal));


            var isOpen = await terminal.Open();

            if (!isOpen)
                return;

            var success = await terminal.Run();
        }

        private Task OpenAndRunDefaultTerminalScreen()
        {
            result.NextScreen = typeof(DefaultTerminalScreen);

            result.NavigationOption = NavigationOption.LoadNew;

            return Task.CompletedTask;
        }

        private Task Return()
        {
            result.NavigationOption = NavigationOption.Previous;

            return Task.CompletedTask;
        }

        private Task LoadSerialPortConfguration()
        {
            result.NextScreen = typeof(SerialPortConfigScreen);

            result.NavigationOption = NavigationOption.LoadNew;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Nothing to Dispose;
        }

        public Task Exit()
        {
            Dispose();

            return Task.CompletedTask;
        }

    }
}
