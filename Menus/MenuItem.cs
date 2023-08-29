using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console3DRenderer.EventSystem;

namespace Console3DRenderer.Menus
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
