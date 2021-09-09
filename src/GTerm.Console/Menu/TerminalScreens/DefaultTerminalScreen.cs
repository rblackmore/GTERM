using System;
using System.Threading.Tasks;

using GTerm.NET.Contracts;
using GTerm.NET.Terminals;

using Spectre.Console;

using Microsoft.Extensions.DependencyInjection;

namespace GTerm.NET.Menu.TerminalScreens
{
    // TODO: THis class should take in an ITerminal. At Runtime i need to decide which implementation to use.
    // A factory Pattern might be appropriate here, instead of injecting the terminal, inject a factory.
    public class DefaultTerminalScreen : IScreen, IDisposable
    {
        private ScreenResult result = new();

        private BaseTerminal terminal;

        public DefaultTerminalScreen(BaseTerminal terminal)
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
