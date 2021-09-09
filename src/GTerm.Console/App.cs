using GTerm.NET.Configuration;
using GTerm.NET.Contracts;
using GTerm.NET.Menu;
using GTerm.NET.Menu.TerminalScreens;
using GTerm.NET.Terminals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Spectre.Console;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GTerm.NET
{
    public class App : IHostedService
    {

        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;
        private readonly IHostApplicationLifetime applicationLifetime;

        private readonly SerialPort port;
        private readonly IOptions<ApplicationOptions> appConfiguration;

        public App(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IHostApplicationLifetime applicationLifetime,
            IOptions<ApplicationOptions> appConfiguration)
        {
            this.serviceProvider = serviceProvider;
                //.CreateScope()
                //.ServiceProvider;

            this.configuration = configuration;
            this.applicationLifetime = applicationLifetime;
            this.appConfiguration = appConfiguration;

            port = this.serviceProvider.GetService<SerialPort>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var screenManager = this.serviceProvider.GetService<ScreenManager>();

            if (appConfiguration.Value.AutoOpen)
            {
                await screenManager.Run<DefaultTerminalScreen>();
            }
            else
            {
                await screenManager.Run<TopMenuScreen>();
            }

            this.applicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            AnsiConsole.WriteLine("Application Closing");

            if (port.IsOpen)
                port.Close();

            return Task.CompletedTask;
        }
    }
}
