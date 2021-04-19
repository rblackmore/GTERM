using GTerm.NET.Contracts;
using GTerm.NET.Terminals;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTerm.NET.Menu.TerminalScreens
{
    public class DefaultTerminalScreen : IScreen, IDisposable
    {
        private ScreenResult result = new();

        private readonly DefaultTerminal terminal;

        public DefaultTerminalScreen(DefaultTerminal terminal)
        {
            this.terminal = terminal;
        }

        public async Task<ScreenResult> Display()
        {
            var isOpen = await terminal.Open();

            if (!isOpen)
            {
                AnsiConsole.Markup($"[red]There was a problem opening port[/]");
                result.NavigationOption = NavigationOption.Previous;
                return this.result;
            }

            var success = await terminal.Run();

            if (!success)
            {
                AnsiConsole.Markup($"[red]There was a problem running Terminal[/]");
                result.NavigationOption = NavigationOption.Previous;
                return result;
            }

            result.NavigationOption = NavigationOption.Previous;
            return this.result;
        }


        public void Dispose()
        {
            terminal.Close();
        }
        public Task Exit()
        {
            Dispose();

            return Task.CompletedTask;
        }
    }
}
