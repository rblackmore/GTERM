using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTerm.NET.Menu
{
    public struct MenuOption
    {
        public string DisplayText { get; set; }

        public Func<Task> Action { get; set; }

        public MenuOption(string displayText, Func<Task> action) : this()
        {
            this.DisplayText = displayText;
            this.Action = action;
        }

    }
}
