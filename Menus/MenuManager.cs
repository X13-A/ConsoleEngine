using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine.EventSystem;

namespace ConsoleEngine.Menus
{
    public class MenuManager : Singleton<MenuManager>
    {
        private Menu? activeMenu;
        public Menu? ActiveMenu => activeMenu;
        public bool MenuOpened => activeMenu == null;

        private int cursorIndex;
        public int CursorIndex => cursorIndex;

        protected override void Init()
        {
            SubscribeEvents();
        }

        public void SubscribeEvents()
        {
            EventManager.Instance.AddListener<OpenMenuEvent>(OpenMenu);
            EventManager.Instance.AddListener<CloseMenuEvent>(CloseMenu);
            EventManager.Instance.AddListener<KeyPressedEvent>(NavigateMenu);
        }

        public void OpenMenu(OpenMenuEvent e)
        {
            activeMenu = e.menu;
            cursorIndex = 0;
        }

        public void CloseMenu(CloseMenuEvent e)
        {
            activeMenu = null;
        }

        public void NavigateMenu(KeyPressedEvent e)
        {
            if (activeMenu == null) return;

            if (e.key == ConsoleKey.UpArrow)
            {
                cursorIndex = Math.Max(0, cursorIndex - 1);
            }
            else if (e.key == ConsoleKey.DownArrow)
            {
                cursorIndex = Math.Min(activeMenu.Items.Count - 1, cursorIndex + 1);
            }
            else if (e.key == ConsoleKey.Enter)
            {
                if (cursorIndex <= activeMenu.Items.Count - 1)
                {
                    activeMenu.Items[cursorIndex].action?.Invoke();
                }
            }
        }
    }
}
