using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console3DRenderer.EventSystem;

namespace Console3DRenderer.Menus
{
    public class Menu
    {
        public string name;

        protected List<MenuItem> items;
        public List<MenuItem> Items => items;

        #region constructors
        public Menu()
        {
            name = "New menu";
            items = new List<MenuItem>();
        }

        public Menu(string name)
        {
            this.name = name;
            items = new List<MenuItem>();
        }

        public Menu(string name, List<MenuItem> items)
        {
            this.name = name;
            this.items = items;
        }

        #endregion

        #region operations
        public virtual void AddItem(Menu menu)
        {
            items.Add(new MenuItem(menu));
        }
        public virtual void AddItem(MenuItem item)
        {
            items.Add(item);
        }
        public virtual void AddItem(string name, Action action)
        {
            items.Add(new MenuItem(name, action));
        }
        public virtual bool RemoveItem(string name)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].name == name)
                {
                    items.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public virtual bool RemoveItem(Menu menu) => RemoveItem(menu.name);
        public virtual bool RemoveItem(MenuItem item) => items.Remove(item);
        #endregion

        public virtual void Open()
        {
            EventManager.Instance.Raise(new OpenMenuEvent { menu = this });
        }

        public virtual void Draw()
        {

        }
    }
}
