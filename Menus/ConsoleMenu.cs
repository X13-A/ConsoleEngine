using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Menus
{
    public class ConsoleMenu : Menu
    {
        public ConsoleColor color;
        public ConsoleColor backgroundColor;

        public override void AddItem(Menu menu)
        {
            items.Add(new ConsoleMenuItem(menu));
        }

        public ConsoleMenu() : base()
        {
            SetDefaultColors();
        }

        public ConsoleMenu(string name) : base(name)
        {
            SetDefaultColors();
        }

        public ConsoleMenu(string name, List<ConsoleMenuItem> items) : base(name, items.Cast<MenuItem>().ToList())
        {
            SetDefaultColors();
        }

        private void SetDefaultColors()
        {
            color = ConsoleColor.White;
            backgroundColor = ConsoleColor.DarkCyan;
        }

        public static ConsoleMenuItem CreateBackButton(Menu target)
        {
            return new ConsoleMenuItem(target)
            {
                name = $"Back ({target.name})"
            };
        }

        public override void Draw()
        {
            ConsoleDisplay.Instance.WriteLine(name, color, backgroundColor);

            int i = 0;
            foreach (MenuItem item in Items)
            {
                item.Draw(i++ == MenuManager.Instance.CursorIndex);
            }
        }
    }
}
