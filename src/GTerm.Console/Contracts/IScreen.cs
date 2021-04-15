using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTerm.NET.Contracts
{
    public enum NavigationOption
    {
        None,
        Previous,
        LoadNew
    }

    public class ScreenResult
    {
        public NavigationOption NavigationOption { get; set; } = NavigationOption.None;

        public Type NextScreen { get; set; }
    }

    public interface IScreen
    {
        Task<ScreenResult> Display();

        Task Exit();
    }
}
