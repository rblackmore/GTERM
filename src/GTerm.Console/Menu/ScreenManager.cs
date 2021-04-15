using GTerm.NET.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GTerm.NET.Menu
{
    public class ScreenManager
    {
        private readonly IServiceProvider serviceProvider;

        private Stack<IScreen> history;

        private IScreen currentScreen => history.Peek();

        public ScreenManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            history = new Stack<IScreen>();
        }

        public async Task Run<T>() where T : IScreen
        {
            await LoadScreen<T>();

            do
            {
                var result = await currentScreen.Display();

                switch (result.NavigationOption)
                {
                    case NavigationOption.None:
                        break;
                    case NavigationOption.Previous:
                        await Previous();
                        break;
                    case NavigationOption.LoadNew:
                        await LoadScreen(result.NextScreen);
                        break;
                    default:
                        break;
                }

            } while (history.Any());
        }

        public Task LoadScreen<T>() where T : IScreen
        {
            var newScreen = serviceProvider.CreateScope().ServiceProvider.GetService<T>();

            history.Push(newScreen);

            return Task.CompletedTask;
        }

        public Task LoadScreen(Type screen)
        {
            if (!screen.GetInterfaces().Contains(typeof(IScreen)))
                throw new ArgumentException($"Screen paramter must implement {nameof(IScreen)}");

            var newScreen = serviceProvider.CreateScope().ServiceProvider.GetService(screen) as IScreen;

            history.Push(newScreen);

            return Task.CompletedTask;
        }

        public async Task Previous()
        {
            var previous = history.Pop();

            await previous.Exit();
        }

    }
}
