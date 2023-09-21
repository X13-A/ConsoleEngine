using ConsoleEngine.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Menus
{
    public enum MenuID { MainMenu, PauseMenu, NotFound };
    public static class MenuStorage
    {
        private static Dictionary<MenuID, ConsoleMenu> menus = new Dictionary<MenuID, ConsoleMenu>();

        public static ConsoleMenu Get(MenuID id)
        {
            menus.TryGetValue(id, out ConsoleMenu? res);
            if (res == null)
            {
                EventManager.Instance.Raise(new ErrorEvent { message = $"Menu with id {id} does not exist in menuStorage" });
                return menus[MenuID.NotFound];
            }
            return res;
        }

        public static void Set(MenuID id, ConsoleMenu menu)
        {
            menus[id] = menu;
        }

        public static void Populate()
        {
            // Main menu
            menus[MenuID.MainMenu] = new ConsoleMenu("Main menu");
            ConsoleMenuItem exitButton = new ConsoleMenuItem("Exit", () => EventManager.Instance.Raise(new ExitAppEvent()));
            exitButton.color = ConsoleColor.White;
            exitButton.highlightColor = ConsoleColor.Red;
            menus[MenuID.MainMenu].AddItem(new ConsoleMenuItem("Start renderer", () => EventManager.Instance.Raise(new StartRendererEvent())));
            menus[MenuID.MainMenu].AddItem(new ConsoleMenuItem($"Edit scene", () => { Utils.OpenFile(Settings.scenePath); }));
            menus[MenuID.MainMenu].AddItem(exitButton);

            // Pause menu
            menus[MenuID.PauseMenu] = new ConsoleMenu("Paused");
            menus[MenuID.PauseMenu].AddItem(new ConsoleMenuItem("Resume", () => EventManager.Instance.Raise(new ResumeGameEvent())));
            menus[MenuID.PauseMenu].AddItem(ConsoleMenuItem.CreateBackButton(menus[MenuID.MainMenu]));

            // Not found menu
            menus[MenuID.NotFound] = new ConsoleMenu("Menu not found");
            menus[MenuID.NotFound].AddItem(new ConsoleMenuItem("Back to main menu", () => EventManager.Instance.Raise(new OpenMenuEvent { menu = menus[MenuID.MainMenu] })));
        }
    }
}
