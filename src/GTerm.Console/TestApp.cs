using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GTerm.NET.Terminals;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Spectre.Console;

namespace GTerm.NET
{
    public class TestApp : IHostedService
    {
        private readonly IOptions<ReceiveOnlyTerminalOptions> options;

        public TestApp(IOptions<ReceiveOnlyTerminalOptions> options)
        {
            this.options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            AnsiConsole.WriteLine("Application Closing");

            return Task.CompletedTask;
        }
    }
}
