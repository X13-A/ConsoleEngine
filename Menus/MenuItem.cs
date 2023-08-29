using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine.EventSystem;

namespace ConsoleEngine.Menus
{
    public class MenuItem
    {
        public string name;
        public Action action;

        public MenuItem(string name)
        {
            this.name = name;
            action = () => { };
        }

        public MenuItem(string name, Action action)
        {
            this.name = name;
            this.action = action;
        }

        public MenuItem(Menu menu)
        {
            name = menu.name;
            action = () => EventManager.Instance.Raise(new OpenMenuEvent { menu = menu });
        }

        public static MenuItem CreateBackButton(Menu target)
        {
            MenuItem backButton = new MenuItem(target);
            backButton.name = $"Back ({target.name})";
            return backButton;
        }

        public virtual void Draw(bool highlighted) { }
    }
}
