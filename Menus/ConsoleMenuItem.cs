using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Menus
{
    public class ConsoleMenuItem : MenuItem
    {
        public ConsoleColor color;
        public ConsoleColor backgroundColor;
        public ConsoleColor highlightColor;
        public ConsoleColor highlightBackgroundColor;

        private void SetDefaultColors()
        {
            color = ConsoleColor.White;
            backgroundColor = ConsoleColor.Black;
            highlightColor = ConsoleColor.DarkCyan;
            highlightBackgroundColor = ConsoleColor.Black;
        }

        public ConsoleMenuItem(string name) : base(name)
        {
            SetDefaultColors();
        }

        public ConsoleMenuItem(string name, Action action) : base(name, action)
        {
            SetDefaultColors();
        }

        public ConsoleMenuItem(Menu menu) : base(menu)
        {
            SetDefaultColors();
        }

        public static ConsoleMenuItem CreateBackButton(ConsoleMenu target)
        {
            return new ConsoleMenuItem(target)
            {
                name = $"Back ({target.name})"
            };
        }

        public override void Draw(bool highlighted)
        {
            if (highlighted)
            {
                ConsoleDisplay.Instance.WriteLine($"-> {name}", highlightColor, highlightBackgroundColor);
            }
            else
            {
                ConsoleDisplay.Instance.WriteLine($"   {name}", color, backgroundColor);
            }
        }
    }
}
