using GTerm.NET.Configuration;
using GTerm.NET.DependencyInjection;
using GTerm.NET.Menu;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

namespace GTerm.NET
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            await CreateHostBuilder(args)
                .RunConsoleAsync().
                ConfigureAwait(false);

            return Environment.ExitCode;
        }

        /// <summary>
        /// Creates a HostBuilder with defauilt services.
        /// For additional application services, add them here.
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<App>();

                    // This code is bad and I should feel bad.
                    services.AddSingleton<SerialPort>(services =>
                    {
                        var options = services.GetService<IOptions<PortOptions>>().Value;
                        var port = new SerialPort();

                        port.PortName = options.Name;
                        port.BaudRate = options.BaudRate;
                        port.DataBits = options.DataBits;
                        port.Handshake = Enum.Parse<Handshake>(options.HandShake);
                        port.Parity = Enum.Parse<Parity>(options.Parity);
                        
                        switch (options.StopBits)
                        {
                            case 1.0f:
                                port.StopBits = StopBits.One;
                                break;
                            case 2.0f:
                                port.StopBits = StopBits.Two;
                                break;
                            case 1.5f:
                                port.StopBits = StopBits.OnePointFive;
                                break;
                            default:
                                port.StopBits = StopBits.None;
                                break;
                        }

                        return port;
                    });

                    services.AddSingleton<ScreenManager>();
                    services.AddScreens();
                    services.AddTerminals();
                    services.Configure<PortOptions>(context.Configuration.GetSection(PortOptions.PortSettings));
                    services.Configure<ApplicationOptions>(context.Configuration.GetSection(ApplicationOptions.App));
                });
    }
}
