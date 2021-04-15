using GTerm.NET.Contracts;
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

        Task<ScreenResult> IScreen.Display()
        {
            var options = new List<MenuOption>
            {
                new MenuOption("1. Configure Serial Port", () => LoadSerialPortConfguration()),
                new MenuOption("2. Open Default Terminal", () => OpenAndRunPort()),
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
            // Nothing to Dispose.
        }

        public Task Exit()
        {
            Dispose();

            return Task.CompletedTask;
        }

    }
}
